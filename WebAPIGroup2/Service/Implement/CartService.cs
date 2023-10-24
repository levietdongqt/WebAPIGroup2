using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text;
using System.Text.Encodings.Web;
using WebAPIGroup2.Controllers.UserModule;
using WebAPIGroup2.Models;
using WebAPIGroup2.Models.DTO;
using WebAPIGroup2.Models.POJO;
using WebAPIGroup2.Respository.Inteface;
using WebAPIGroup2.Service.Inteface;

namespace WebAPIGroup2.Service.Implement
{
    public class CartService : ICartService
    {
        private readonly IPurchaseOrderRepo _purchaseOrderRepo;
        private readonly IProductDetailsRepo _productDetailsRepo;
        private readonly IMyImageRepo _myImageRepo;
        private readonly IDeliveryInfoRepo _deliveryInfoRepo;
        private readonly IUtilService _utilService;
        private readonly IContentEmailRepo _contentEmailRepo;
        private readonly IMonthlySpendingRepo _monthlySpendingRepo;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IImageRepo _imageRepo;



        public CartService(IPurchaseOrderRepo purchaseOrderRepo, IProductDetailsRepo productDetailsRepo, IMyImageRepo myImageRepo, IDeliveryInfoRepo deliveryInfoRepo, IUtilService utilService, IContentEmailRepo contentEmailRepo, IMonthlySpendingRepo monthlySpendingRepo, IWebHostEnvironment webHostEnvironment, IImageRepo imageRepo)
        {
            _purchaseOrderRepo = purchaseOrderRepo;
            _productDetailsRepo = productDetailsRepo;
            _myImageRepo = myImageRepo;
            _deliveryInfoRepo = deliveryInfoRepo;
            _utilService = utilService;
            _contentEmailRepo = contentEmailRepo;
            _monthlySpendingRepo = monthlySpendingRepo;
            _webHostEnvironment = webHostEnvironment;
            _imageRepo = imageRepo;
        }
        public async Task<bool> AddToCart(OrderDTO orderDTO)
        {
            using (var context = new MyImageContext())
            {
                var materialPage = context.MaterialPages.FirstOrDefaultAsync(t => t.Id == orderDTO.materialPageId);
                var purchaseOrder = await _purchaseOrderRepo.getPurchaseOrder(orderDTO.userID, PurchaseStatus.InCart);
                if (purchaseOrder == null)
                {
                    purchaseOrder = new PurchaseOrder()
                    {
                        CreateDate = DateTime.Now,
                        Status = PurchaseStatus.InCart,
                        UserId = orderDTO.userID
                    };
                    if (!await _purchaseOrderRepo.InsertAsync(purchaseOrder))
                    {
                        return false;
                    };
                }

                await Task.WhenAll(materialPage);
                if (materialPage.Result == null)
                {
                    return false;
                }
                var myImage = await _myImageRepo.GetByIDAsync(orderDTO.myImageID);
                if (myImage == null)
                {
                    return false;
                }
                myImage.PurchaseOrderId = purchaseOrder.Id;
                await _myImageRepo.UpdateAsync(myImage);

                float priceOne = orderDTO.imageArea.Value * (float)(materialPage.Result.PricePerInch) + (float)myImage.Template.PricePlusPerOne;
                float imagesNumber = (float)myImage.Images.Count;
                decimal price = (decimal)(priceOne * imagesNumber);

                var sameProduct = await _productDetailsRepo.GetByMyImageId(orderDTO);
                if (sameProduct == null)
                {
                    ProductDetail productDetail = new ProductDetail()
                    {
                        MyImageId = myImage.Id,
                        TemplateSizeId = orderDTO.temlateSizeId,
                        MaterialPageId = materialPage.Result.Id,
                        CreateDate = DateTime.Now,
                        Price = price,
                        Quantity = orderDTO.quantity
                    };
                    await _productDetailsRepo.InsertAsync(productDetail);
                    return true;
                }
                sameProduct.Quantity += orderDTO.quantity;
                await _productDetailsRepo.UpdateAsync(sameProduct);
                return true;
            }

        }

        public async Task<DeliveryInfo> createDeliveryInfo(CartController.PurchaseDTO purchaseDTO)
        {
            var deliveryInfo = await _deliveryInfoRepo.getByAdress(purchaseDTO.userId, purchaseDTO.address);
            if (deliveryInfo != null)
            {
                return deliveryInfo;
            }
            DeliveryInfo newDelivery = new DeliveryInfo()
            {
                UserId = purchaseDTO.userId,
                DeliveryAddress = purchaseDTO.address,
                Email = purchaseDTO.email,
                Phone = purchaseDTO.phone,
                CustomName = purchaseDTO.fullName
            };
            await _deliveryInfoRepo.InsertAsync(newDelivery);
            return newDelivery;
        }

        public async Task<PurchaseOrder> createOrder(CartController.PurchaseDTO purchaseDTO, DeliveryInfo deliveryInfo, String status)
        {
            var purchaseOrder = await _purchaseOrderRepo.getPurchaseOrder(purchaseDTO.userId, PurchaseStatus.InCart);
            if (purchaseOrder == null)
            {
                return null;
            }
            purchaseOrder.Status = status;
            purchaseOrder.PriceTotal = purchaseDTO.totalPrice;
            purchaseOrder.CreateDate = DateTime.Now;
            purchaseOrder.Note = purchaseDTO.note;
            purchaseOrder.DeliveryInfoId = deliveryInfo.Id;
            await _purchaseOrderRepo.UpdateAsync(purchaseOrder);
            MonthlySpending monthlySpending = new MonthlySpending()
            {
                TimeDetail = DateTime.Now,
                UserId = purchaseDTO.userId,
                Total = purchaseDTO.totalPrice
            };
            await _monthlySpendingRepo.InsertAsync(monthlySpending);
            return purchaseOrder;

        }

        public async Task<bool> deleteAllCart(List<int> productIdList)
        {
            List<ProductDetail> list = _productDetailsRepo.getByIdList(productIdList);
            if(list == null)
            {
                return false;
            }
            if(await _productDetailsRepo.DeleteAllAsync(list))
            {
                return true;
            }
            return false;
        }

        public async Task<bool> deleteFolder(int purchaseID)
        {
            var myImages = await _myImageRepo.getByOrder(purchaseID);
            var root = _webHostEnvironment.WebRootPath;
            List<Image> images = new List<Image>();
            foreach (var image in myImages)
            {
                images.AddRange(image.Images);
                if (image.TemplateId == 1)
                {
                    string filePath = image.Images.First().ImageUrl.Substring(1);
                    var fileUrl = Path.Combine(root, filePath);
                    if (File.Exists(fileUrl))
                    {
                        File.Delete(fileUrl);
                    }

                    Console.WriteLine("Xoá File thành công.");
                }
                else
                {
                    string folderName = image.Images.First().FolderName.Substring(1);
                    var uploadsFolder = Path.Combine(root, "MyImage", folderName);
                    if (Directory.Exists(uploadsFolder))
                    {
                        Directory.Delete(uploadsFolder, true); // Đặt giá trị thứ hai là true để xoá toàn bộ nội dung bên trong thư 
                    }
                    Console.WriteLine("Xoá thư mục thành công.");
                }
            }
            await _imageRepo.DeleteAllAsync(images);
            return true;
        }

        public async Task<bool> deleteProductDetail(int productDetailID)
        {
            var product = await _productDetailsRepo.GetByIDAsync(productDetailID);
            if (product == null)
            {
                return false;
            }
            if (await _productDetailsRepo.DeleteAsync(product))
            {
                return true;
            };
            return false;

        }

        public async Task<List<CartResponseDTO>> LoadCart(int userID)
        {
            List<MyImage> list = await _myImageRepo.loadInCart(userID);
            if (list == null)
            {
                return null;
            }
            List<CartResponseDTO> result = new List<CartResponseDTO>();
            foreach (MyImage item in list)
            {
                var product = item.ProductDetails.Select(x => new CartResponseDTO()
                {
                    length = x.TemplateSize.PrintSize.Length,
                    width = x.TemplateSize.PrintSize.Width,
                    images = item.Images.Select(x => x.ImageUrl).ToList(),
                    quantity = x.Quantity,
                    materialPage = x.MaterialPage.Name,
                    templateName = item.Template.Id != 1 ? item.Template.Name : "Simple Prints",
                    price = x.Price,
                    productId = x.Id,
                }
                 ).ToList();
                result.AddRange(product);
            }
            return result;
        }

        public async Task<bool> sendMail(CartController.PurchaseDTO purchaseDTO, DeliveryInfo deliveryInfo)
        {
            StringBuilder mailBody = new StringBuilder();
            mailBody.Append($"<p>Dear {purchaseDTO.fullName} </p> ")
                    .Append("<p> We would like to express our sincere gratitude for choosing to use our services. </br>")
                    .Append("   We will contact you as soon as possible for your order. </br>")
                    .Append(" Below are the details of your payment:</p>")
                    .Append("<ul> ")
                    .Append($"<li><strong>Total Price:</strong> $ {purchaseDTO.totalPrice} </li>")
                    .Append($"<li><strong>Order Date:</strong> {DateTime.Now} </li>")
                    .Append($"<li><strong>Delivery Address:</strong>{purchaseDTO.address} </li>")
                    .Append("</ul>");
            var mailContent = new MailContent(purchaseDTO.email, "MyImages - Order notification", mailBody.ToString(), "Notification");
            var mailContented = _utilService.SendEmailAsync(mailContent);

            ContentEmail contentEmail = new ContentEmail();
            contentEmail.SubjectEmail = "Notification";
            contentEmail.BodyEmail = mailBody.ToString();
            contentEmail.DeliveryInfoId = deliveryInfo.Id;
            var task1 = _contentEmailRepo.InsertAsync(contentEmail);
            await Task.WhenAll(mailContented, task1);
            if (mailContented.Result == null)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> UpdateCart(int productDetailID, int quantity)
        {
            var oldProduct = await _productDetailsRepo.GetByIDAsync(productDetailID);
            if (oldProduct == null)
            {
                return false;
            }
            oldProduct.Quantity = quantity;
            await _productDetailsRepo.UpdateAsync(oldProduct);
            return true;
        }
    }
}

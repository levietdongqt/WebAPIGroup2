using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebAPIGroup2.Models;
using WebAPIGroup2.Models.DTO;
using WebAPIGroup2.Models.POJO;
using WebAPIGroup2.Respository.Inteface;
using WebAPIGroup2.Service.Inteface;

namespace WebAPIGroup2.Service.Implement
{
    public class UpLoadService : IUpLoadService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IImageRepo _imageRepo;
        private readonly IPurchaseOrderRepo _purchaseOrderRepo;
        private readonly IProductDetailsRepo _productDetailsRepo;
        private readonly IUserRepo _userRepo;
        private readonly ITemplateRepo _templateRepo;
        private readonly IMyImageRepo _myImageRepo;

        public UpLoadService(IWebHostEnvironment webHostEnvironment, IImageRepo imageRepo, IPurchaseOrderRepo purchaseOrderRepo, IProductDetailsRepo productDetailsRepo, ITemplateRepo templateRepo, IUserRepo userRepo, IMyImageRepo myImageRepo)

        {
            _webHostEnvironment = webHostEnvironment;
            _imageRepo = imageRepo;
            _purchaseOrderRepo = purchaseOrderRepo;
            _productDetailsRepo = productDetailsRepo;
            _templateRepo = templateRepo;
            _userRepo = userRepo;
            _myImageRepo = myImageRepo;
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

                var sameProduct =await _productDetailsRepo.GetByMyImageId(orderDTO);
                if(sameProduct == null)
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

        public async Task<List<CartResponseDTO>> LoadCart(int userID)
        {
           List<MyImage> list =  await _myImageRepo.loadInCart(userID);
            List<CartResponseDTO> result = new List<CartResponseDTO>();
            foreach (MyImage item in list)
            {
               var product= item.ProductDetails.Select(x => new CartResponseDTO()
                {
                    length = x.TemplateSize.PrintSize.Length,
                    width = x.TemplateSize.PrintSize.Width,
                    image = item.Images.First().ImageUrl,
                    quantity= x.Quantity,
                    materialPage = x.MaterialPage.Name,
                    templateName = item.Template.Id != 1? item.Template.Name : "None",
                    price = x.Price
                }
                ).ToList();
                result.AddRange(product);
            }
            return result;
        }

        public async Task<List<MyImage>> LoadMyImages(int userID)
        {
            List<MyImage> listProduct = await _myImageRepo.getByUserId(userID);
            if (listProduct == null)
            {
                return null;
            }
            return listProduct;
        }

        public async Task<List<string>> SaveImages(string folderName, int? templateID, IFormFile[] files)
        {
            List<string> imagesUrl = new List<string>();
            string templateFolder = "Template_" + templateID;
            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "MyImage", folderName, templateFolder);
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }
            foreach (var file in files)
            {
                var fileName = file.FileName;
                var imagePath = Path.Combine(uploadsFolder, fileName);
                //Upload
                var stream = new FileStream(imagePath, FileMode.Create);

                await file.CopyToAsync(stream);
                //Set URL Static
                var urlFilePath = $"/MyImage/{folderName}/{templateFolder}/{fileName}";
                imagesUrl.Add(urlFilePath);
            }
            return imagesUrl;
        }

        public async Task<MyImage> SaveToDBTemporary(string folderName, UpLoadDTO upLoadDTO, List<string> imagesUrls)
        {

            var temPurchaseOrder = await _purchaseOrderRepo.getPurchaseOrder(upLoadDTO.userID, PurchaseStatus.Temporary);
            if (temPurchaseOrder == null)
            {
                temPurchaseOrder = new PurchaseOrder()
                {
                    CreateDate = DateTime.Now,
                    Status = PurchaseStatus.Temporary,
                    UserId = upLoadDTO.userID
                };
                if (!await _purchaseOrderRepo.InsertAsync(temPurchaseOrder))
                {
                    return null;
                };
            }
            MyImage myImage = new MyImage()
            {
                PurchaseOrderId = temPurchaseOrder.Id,
                Status = true,
                TemplateId = upLoadDTO.templateID,
            };
            if (!await _myImageRepo.InsertAsync(myImage))
            {
                return null;
            };

            List<Image> images = new List<Image>();
            string templateFolder = $"Template_{upLoadDTO.templateID}";
            imagesUrls.ForEach(imageUrl =>
            {
                Image image = new Image()
                {
                    FolderName = $"/{folderName}/{templateFolder}",
                    CreateDate = DateTime.Now,
                    MyImagesId = myImage.Id,
                    Status = true,
                    ImageUrl = imageUrl,

                };
                images.Add(image);
            });
            if (!await _imageRepo.InsertAllAsync(images))
            {
                return null;
            };
            return myImage;

        }

        public async Task<List<int>> SaveToDBWithNoTemplate(string folderName, UpLoadDTO upLoadDTO, List<string> imagesUrls)
        {
            var temPurchaseOrder = await _purchaseOrderRepo.getPurchaseOrder(upLoadDTO.userID, PurchaseStatus.Temporary);
            if (temPurchaseOrder == null)
            {
                temPurchaseOrder = new PurchaseOrder()
                {
                    CreateDate = DateTime.Now,
                    Status = PurchaseStatus.Temporary,
                    UserId = upLoadDTO.userID
                };
                if (!await _purchaseOrderRepo.InsertAsync(temPurchaseOrder))
                {
                    return null;
                };
            }
            string templateFolder = $"Template_{upLoadDTO.templateID}";
            List<int> myImageIDs = new List<int>();
            foreach (var imageUrl in imagesUrls)
            {
                MyImage myImage = new MyImage()
                {
                    PurchaseOrderId = temPurchaseOrder.Id,
                    Status = true,
                    TemplateId = upLoadDTO.templateID,
                };
                if (!await _myImageRepo.InsertAsync(myImage))
                {
                    return null;
                };
                myImageIDs.Add(myImage.Id);
                Image image = new Image()
                {
                    FolderName = $"/{folderName}/{templateFolder}",
                    CreateDate = DateTime.Now,
                    MyImagesId = myImage.Id,
                    Status = true,
                    ImageUrl = imageUrl,

                };
                await _imageRepo.InsertAsync(image);

            };
            return myImageIDs;
        }

        public async Task<bool> UpdateCart(int productDetailID,int quantity)
        {
            var oldProduct =await _productDetailsRepo.GetByIDAsync(productDetailID);
            if (oldProduct == null)
            {
                return false;
            }
            oldProduct.Quantity = quantity;
           await _productDetailsRepo.UpdateAsync(oldProduct);
            return true;
        }

        public async Task<bool> ValidateFiles(IFormFile[] files)
        {
            List<string> allowExtensions = new List<string>()
            {
                ".jpg",
                ".jpeg",
                ".png"
            };

            foreach (var file in files)
            {
                if (file == null || file.Length == 0)
                {
                    return false;
                }
                var extension = Path.GetExtension(file.FileName);
                if (extension.IsNullOrEmpty())
                {
                    return false;
                }
                if (!allowExtensions.Contains(extension))
                {
                    return false;
                }
            }
            return true;
        }


        public async Task<string> ValidateRequestData(UpLoadDTO upLoadDTO)
        {
            using (var context = new MyImageContext())
            {
                var checkUser = _userRepo.GetByIDAsync(upLoadDTO.userID);
                // var checkTemplate = context.Templates.FirstOrDefaultAsync(t => t.Id == upLoadDTO.templateID);
                await Task.WhenAll(checkUser);
                if (checkUser.Result != null && true)
                {
                    var email = checkUser.Result.Email;
                    return email.Substring(0, email.IndexOf("@"));
                }
                return null;
            }
        }
    }
}

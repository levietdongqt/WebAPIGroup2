using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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

        public UpLoadService(IWebHostEnvironment webHostEnvironment, IImageRepo imageRepo, IPurchaseOrderRepo purchaseOrderRepo, IProductDetailsRepo productDetailsRepo, ITemplateRepo templateRepo, IUserRepo userRepo)

        {
            _webHostEnvironment = webHostEnvironment;
            _imageRepo = imageRepo;
            _purchaseOrderRepo = purchaseOrderRepo;
            _productDetailsRepo = productDetailsRepo;
            _templateRepo = templateRepo;
            _userRepo = userRepo;
        }

        public async Task<bool> CreateOrder(OrderDTO orderDTO)
        {
            using (var context = new MyImageContext())
            {
                var materialPage = context.MaterialPages.FirstOrDefaultAsync(t => t.Id == orderDTO.materialPageId);
                var purchaseOrder = await _purchaseOrderRepo.getPurchaseOrder(orderDTO.userID, PurchaseStatus.OrderPlaced);
                if (purchaseOrder == null)
                {
                    purchaseOrder = new PurchaseOrder()
                    {
                        CreateDate = DateTime.Now,
                        Status = PurchaseStatus.OrderPlaced,
                        UserId = orderDTO.userID
                    };
                    if (!await _purchaseOrderRepo.InsertAsync(purchaseOrder))
                    {
                        return false;
                    };
                }
                var producDetail = await _productDetailsRepo.GetByIDAsync(orderDTO.productDetailID);
                if (producDetail == null)
                {
                    return false;
                }
                await Task.WhenAll(materialPage);
                if (materialPage.Result == null)
                {
                    return false;
                }

                float priceOne = orderDTO.imageArea * (float)(materialPage.Result.PricePerInch);
                float imagesNumber = (float)producDetail.Images.Count;
                producDetail.PurchaseOrderId = purchaseOrder.Id;
                producDetail.Price = (decimal)(priceOne * imagesNumber);
                producDetail.Quantity = orderDTO.quantity;
                producDetail.MaterialPageId= materialPage.Id;
                await _productDetailsRepo.UpdateAsync(producDetail);
                return true;
            }
               
        }

        public async Task<List<ProductDetail>> LoadProductDetails(int userID)
        {
            var status = PurchaseStatus.Temporary;
            PurchaseOrder temppPurchase = await _purchaseOrderRepo.getPurchaseOrder(userID, status);
            if (temppPurchase == null)
            {
                return null;
            }
            List<ProductDetail> listProduct = await _productDetailsRepo.getProductDetailByOrder(temppPurchase.Id);
            if (listProduct == null)
            {
                return null;
            }
            return listProduct;
        }

        public async Task<List<string>> SaveImages(string folderName, int templateID, IFormFile[] files)
        {
            List<string> imagesUrl = new List<string>();
            string templateFolder = "Template_" + templateID;
            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Image", folderName, templateFolder);
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
                var urlFilePath = $"/Image/{folderName}/{templateFolder}/{fileName}";
                imagesUrl.Add(urlFilePath);
            }
            return imagesUrl;
        }

        public async Task<ProductDetail> SaveToDBTemporary(string folderName, UpLoadDTO upLoadDTO, List<string> imagesUrls)
        {
            using (var context = new MyImageContext())
            {

                //var materialPage = context.MaterialPages.FirstOrDefaultAsync(t => t.Id == upLoadDTO.materialPageId);
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
                //await Task.WhenAll(materialPage);
                //if(materialPage.Result == null)
                //{
                //    return false;
                //}

                //float priceOne = upLoadDTO.imageArea * (float)(materialPage.Result.PricePerInch);
                //float imagesNumber = (float)imagesUrls.Count;
                ProductDetail productDetail = new ProductDetail()
                {
                    CreateDate = DateTime.Now,
                   // MaterialPageId = upLoadDTO.materialPageId,
                    PurchaseOrderId = temPurchaseOrder.Id,
                    Status = true,
                    //Price = (decimal)(priceOne * imagesNumber),
                    TemplateId = upLoadDTO.templateID,
                };
                if (!await _productDetailsRepo.InsertAsync(productDetail))
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
                        ProductDetailId = productDetail.Id,
                        Status = true,
                        ImageUrl = imageUrl,

                    };
                    images.Add(image);
                });
                if (!await _imageRepo.InsertAllAsync(images))
                {
                    return null;
                };
                return productDetail;
            }

        }

        public async Task<bool> ValidateFiles(IFormFile[] files)
        {
            var allowExtension = ".jpeg";

            foreach (var file in files)
            {
                if (file == null || file.Length == 0)
                {
                    return false;
                }
                if (!allowExtension.Contains(Path.GetExtension(file.FileName)))
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

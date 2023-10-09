using Microsoft.AspNetCore.Http;
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

        public UpLoadService(IWebHostEnvironment webHostEnvironment, IImageRepo imageRepo, IPurchaseOrderRepo purchaseOrderRepo, IProductDetailsRepo productDetailsRepo)

        {
            _webHostEnvironment = webHostEnvironment;
            _imageRepo = imageRepo;
            _purchaseOrderRepo = purchaseOrderRepo;
            _productDetailsRepo = productDetailsRepo;
        }
        public async Task<List<string>> SaveImages(int userID, int templateID, IFormFile[] files)
        {
            List<string> imagesUrl = new List<string>();
            string userFolder = "UserFolder" + userID;
            string templateFolder = "TemplateFolder" + templateID;
            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Image", userFolder, templateFolder);
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
                var urlFilePath = $"/Image/{userFolder}/{templateFolder}/{fileName}";
                imagesUrl.Add(urlFilePath);
            }
            return imagesUrl;
        }

        public async Task<bool> SaveProductDetails(UpLoadDTO upLoadDTO, List<string> imagesUrls)
        {
            PurchaseOrder purchaseOrder = new PurchaseOrder()
            {
                CreateDate = DateTime.Now,
                Status = PurchaseStatus.Temporary,
                UserId = upLoadDTO.userID
            };
            if (!await _purchaseOrderRepo.InsertAsync(purchaseOrder))
            {
                return false;
            };

            float areaImage = 2f;
            float imagesNumber = (float)imagesUrls.Count;
            ProductDetail productDetail = new ProductDetail()
            {
                CreateDate = DateTime.Now,
                MaterialPageId = upLoadDTO.templateID,
                PurchaseOrderId = purchaseOrder.Id,
                Status = true,
                Price = (decimal)(areaImage * imagesNumber),
                TemplateId = upLoadDTO.templateID,
            };
            if (!await _productDetailsRepo.InsertAsync(productDetail))
            {
                return false;
            };

            List<Image> images = new List<Image>();
            string userFolder = $"UserFolder{upLoadDTO.userID}";
            string templateFolder = $"TemplateFolder{upLoadDTO.templateID}";
            imagesUrls.ForEach(imageUrl =>
            {
                Image image = new Image()
                {
                    FolderName = $"/{userFolder}/{templateFolder}",
                    CreateDate = DateTime.Now,
                    ProductDetailId = productDetail.Id,
                    Status = true,
                    ImageUrl = imageUrl,

                };
                images.Add(image);
            });
            if (!await _imageRepo.InsertAllAsync(images))
            {
                return false;
            };
            return true;
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
    }
}

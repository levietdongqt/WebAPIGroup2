using Microsoft.AspNetCore.Http;
using WebAPIGroup2.Models.POJO;
using WebAPIGroup2.Respository.Inteface;
using WebAPIGroup2.Service.Inteface;

namespace WebAPIGroup2.Service.Implement
{
    public class UpLoadService : IUpLoadService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IImageRepo _imageRepo;
        public UpLoadService (IWebHostEnvironment webHostEnvironment,IImageRepo imageRepo)

        {
            _webHostEnvironment = webHostEnvironment;
            _imageRepo = imageRepo;
        }
        public async Task<List<string>> SaveImages(int userID, int templateID, IFormFile[] files)
        {
            List<string> imagesUrl = new List<string>();
            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Image", userID.ToString(), templateID.ToString());
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
                var urlFilePath = $"/Image/{userID}/{templateID}/{fileName}";
                imagesUrl.Add(urlFilePath);
            }
            return imagesUrl;
        }

        public async Task<bool> SaveProductDetails(int userID,int templateID, List<string> imagesUrls)
        {
            float areaImage = 2f;
            float imagesNumber = (float) imagesUrls.Count;
            ProductDetail productDetail = new ProductDetail()
            {
                CreateDate = DateTime.Now,
                MaterialPageId = templateID,
                PurchaseOrderId = userID,
                Status = true,
                Price = (decimal) (areaImage*imagesNumber),
                
            };

            List<Image> images = new List<Image>();
            imagesUrls.ForEach(imageUrl =>
            {
                Image image = new Image()
                {
                    FolderName = templateID.ToString(),
                    CreateDate = DateTime.Now,
                    ProductDetailId = productDetail.Id,
                    Status = true
                    
                };
                images.Add(image);  
            });
          await  _imageRepo.InsertAllAsync(images);

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

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

        public async Task<List<MyImagesResponseDTO>> LoadNoTemplate(int userID)
        {
            List<MyImage> listProduct = await _myImageRepo.getByUserId(userID);
            if (listProduct == null)
            {
                return null;
            }
            List<MyImagesResponseDTO> list = listProduct.Where(t => t.Template.Id == 1).Select(x => new MyImagesResponseDTO()
            {
                createDate = x.CreateDate,
                Id = x.Id,
                images = x.Images.Select(x => new Image
                {
                    Id = x.Id,
                    FolderName = x.FolderName,
                    ImageUrl = x.ImageUrl
                }).ToList(),
                printSizes = x.Template.TemplateSizes.Select(x => new PrintSizeDTO()
                {
                    Id = x.PrintSize.Id,
                    Length = x.PrintSize.Length,
                    Width = x.PrintSize.Width,
                    templateSizeID = x.Id,

                }).ToList(),
                templateName = x.Template.Name,
                templateId = x.TemplateId,
                pricePlusPerOne = x.Template.PricePlusPerOne

            }).ToList();
            if (list.Count < 1)
            {
                return null;
            }
            return list;
        }
        public async Task<List<MyImagesResponseDTO>> LoadMyImages(int userID)
        {
            List<MyImage> listProduct = await _myImageRepo.getByUserId(userID);
            if (listProduct == null)
            {
                return null;
            }
            List<MyImagesResponseDTO> list = listProduct.Where(t => t.Template.Id != 1).Select(x => new MyImagesResponseDTO()
            {
                createDate = x.CreateDate,
                Id = x.Id,
                images = x.Images.Select(x => new Image
                {
                    Id = x.Id,
                    FolderName = x.FolderName,
                    ImageUrl = x.ImageUrl
                }).ToList(),
                printSizes = x.Template.TemplateSizes.Select(x => new PrintSizeDTO()
                {
                    Id = x.PrintSize.Id,
                    Length = x.PrintSize.Length,
                    Width = x.PrintSize.Width,
                    templateSizeID = x.Id,

                }).ToList(),
                templateName = x.Template.Name,
                templateId = x.TemplateId,
                pricePlusPerOne = x.Template.PricePlusPerOne

            }).ToList();
            MyImagesResponseDTO myImagesResponseDTO = new MyImagesResponseDTO();
            bool checkTrue = false;
            foreach (var item in listProduct)
            {
                if (item.Template.Id == 1)
                {
                    if (!checkTrue)
                    {
                        myImagesResponseDTO.Id = item.Id;
                        myImagesResponseDTO.createDate = item.CreateDate;
                        myImagesResponseDTO.templateName = "No Template";
                        myImagesResponseDTO.templateId = 1;
                        myImagesResponseDTO.printSizes = item.Template.TemplateSizes.Select(x => new PrintSizeDTO()
                        {
                            Id = x.PrintSize.Id,
                            Length = x.PrintSize.Length,
                            Width = x.PrintSize.Width,
                            templateSizeID = x.Id,

                        }).ToList();
                    }
                    myImagesResponseDTO.images.AddRange(item.Images.Select(x => new Image
                    {
                        Id = x.Id,
                        FolderName = x.FolderName,
                        ImageUrl = x.ImageUrl
                    }).ToList());
                    checkTrue = true;
                }
            }
            if (checkTrue)
            {
                list.Add(myImagesResponseDTO);
            }
            return list.OrderByDescending(t => -t.templateId).ToList();
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
                stream.Close();

                //Set URL Static
                var urlFilePath = $"/MyImage/{folderName}/{templateFolder}/{fileName}";
                imagesUrl.Add(urlFilePath);
            }
            return imagesUrl;
        }

        public async Task<MyImage> SaveToDBTemporary(string folderName, UpLoadDTO upLoadDTO, List<string> imagesUrls)
        {
            bool isNew = false;
            var temPurchaseOrder = await _purchaseOrderRepo.getPurchaseOrder(upLoadDTO.userID, PurchaseStatus.Temporary);
            if (temPurchaseOrder == null)
            {
                isNew = true;
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
            MyImage myImage = new MyImage();
            if (!isNew)
            {
                var list = await _myImageRepo.getByUserId(upLoadDTO.userID);
                var oldMyImage = list.FirstOrDefault(t => t.TemplateId == upLoadDTO.templateID);
                if (oldMyImage == null)
                {
                    myImage = new MyImage()
                    {
                        PurchaseOrderId = temPurchaseOrder.Id,
                        Status = true,
                        TemplateId = upLoadDTO.templateID,
                        CreateDate = DateTime.Now,
                    };
                    if (!await _myImageRepo.InsertAsync(myImage))
                    {
                        return null;
                    };
                }
                else
                {
                    myImage = oldMyImage;
                }
            }
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
                    CreateDate = DateTime.Now,
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

        public async Task<bool> deleteMyImage(int myImagesId)
        {
            var myImage = await _myImageRepo.GetByIDAsync(myImagesId);
            if (myImage == null)
            {
                return false;
            }
            var images = myImage.Images.ToList();
            var task1 = _imageRepo.DeleteAllAsync(images);
            var task2 = deleteFiles(images);
            await Task.WhenAll(task1, task2);
            if (await _myImageRepo.DeleteAsync(myImage))
            {
                return true;
            }
            return false;
        }
        public async Task<bool> deleteFiles(List<Image> images)
        {
            foreach (var image in images)
            {
                string filePath = image.ImageUrl.Substring(1);
                var fileUrl = Path.Combine(_webHostEnvironment.WebRootPath, filePath);
                if (File.Exists(fileUrl))
                {
                    File.Delete(fileUrl);
                }

                Console.WriteLine("Xoá File thành công.");

            }
            return true;
        }
    }
}

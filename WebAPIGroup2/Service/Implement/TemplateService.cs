using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using System.Dynamic;
using System.Globalization;
using System.Text.RegularExpressions;
using WebAPIGroup2.Models.DTO;
using WebAPIGroup2.Models.POJO;
using WebAPIGroup2.Respository.Inteface;
using WebAPIGroup2.Service.Inteface;

namespace WebAPIGroup2.Service.Implement
{
    public class TemplateService : ITemplateService
    {
        private readonly ITemplateRepo templateRepo;
        private readonly IDescriptionTemplateRepo descriptionTemplateRepo;
        private readonly ITemplateImageRepo imageRepo;
        private readonly IUtilService utilService;
        private readonly ICollectionRepo collectionRepo;
        private readonly ISizeRepo sizeRepo;
        private readonly ITemplateSizeRepo templateSizeRepo;
        private readonly ICollectionTemplateRepo collectionTemplateRepo;
        private readonly IProductDetailsRepo productDetailsRepo;
        private readonly IMyImageRepo myImageRepo;
        private readonly IPurchaseOrderRepo purchaseOrderRepo;
        private readonly IUserRepo userRepo;
        private readonly ITemplateImageRepo templateImageRepo;
        private readonly IMapper mapper;
        public TemplateService(ITemplateRepo templateRepo, IMapper mapper, IDescriptionTemplateRepo descriptionTemplateRepo, ITemplateImageRepo imageRepo, IUtilService utilService, ICollectionRepo collectionRepo, ISizeRepo sizeRepo, ICollectionTemplateRepo collectionTemplateRepo, ITemplateSizeRepo templateSizeRepo, IProductDetailsRepo productDetailsRepo, IMyImageRepo myImageRepo, IPurchaseOrderRepo purchaseOrderRepo,IUserRepo userRepo,ITemplateImageRepo templateImageRepo)
        {
            this.templateRepo = templateRepo;
            this.mapper = mapper;
            this.descriptionTemplateRepo = descriptionTemplateRepo;
            this.imageRepo = imageRepo;
            this.utilService = utilService;
            this.collectionRepo = collectionRepo;
            this.sizeRepo = sizeRepo;
            this.templateSizeRepo = templateSizeRepo;
            this.collectionTemplateRepo = collectionTemplateRepo;
            this.productDetailsRepo = productDetailsRepo;
            this.myImageRepo = myImageRepo;
            this.purchaseOrderRepo = purchaseOrderRepo;
            this.userRepo = userRepo;
            this.templateImageRepo = templateImageRepo;
        }
        public async  Task<TemplateDTO> AddDescriptionByTemplateIdAsync(int templateId, List<DescriptionTemplateDTO> descriptionTemplateDTOs)
        {
            foreach (var templateDTO in descriptionTemplateDTOs)
            {
                templateDTO.TemplateId = templateId;
            }
            var descriptionTemplates = mapper.Map<List<DescriptionTemplate>>(descriptionTemplateDTOs);
            var result = await descriptionTemplateRepo.InsertAllAsync(descriptionTemplates);
            if (!result)
            {
                return null;
            }
            var template = await templateRepo.GetByIDAsync(templateId);
            return mapper.Map<TemplateDTO>(template);
        }

        public async Task<TemplateDTO> AddImageByTemplateIdAsync(int templateId, IFormFile[] formFiles)
        {
            var urlList = await utilService.UploadMany(formFiles);
            var imageDTOs = new List<TemplateImageDTO>();
            foreach (var i in urlList)
            {
                var imageDTO = new TemplateImageDTO()
                {
                    ImageUrl = i,
                    TemplateId = templateId,
                };
                imageDTOs.Add(imageDTO);
            }
            var templateImages = mapper.Map<List<TemplateImage>>(imageDTOs);
            var result = await imageRepo.InsertAllAsync(templateImages);
            if (!result)
            {
                return null;
            }
            var template = await templateRepo.GetByIDAsync(templateId);
            return mapper.Map<TemplateDTO>(template);
        }

        public async Task<TemplateDTO> CreateAsync(AddTemplateDTO addTemplateDTO)
        {
            CultureInfo culture = new CultureInfo("en-US");
            var templateDomain = mapper.Map<Template>(addTemplateDTO);
            templateDomain.CreateDate = DateTime.Now;
            double output;
            double.TryParse(addTemplateDTO.PricePlusPerOne, NumberStyles.Any, culture, out output);
            templateDomain.PricePlusPerOne = output;
            var result = await templateRepo.InsertAsync(templateDomain);
            if (!result)
            {
                return null;
            }
            var templateDTO = mapper.Map<TemplateDTO>(templateDomain);

            //Luu vao bang TemplateImage
            var urlList = await utilService.UploadMany(addTemplateDTO.formFileList);
            var imageDTOs = new List<TemplateImageDTO>();
            foreach (var i in urlList)
            {
                var imageDTO = new TemplateImageDTO()
                {
                    ImageUrl = i,
                    TemplateId = templateDTO.Id,
                };
                imageDTOs.Add(imageDTO);
            }
            var templateImages = mapper.Map<List<TemplateImage>>(imageDTOs);
            var r2 = await imageRepo.InsertAllAsync(templateImages);

            //Luu vao bang phu Category Template
            var collectionTemplateDTOs = new List<CollectionTemplateDTO>();
            foreach(var i in addTemplateDTO.collectionDTOs)
            {
                var collectionTemplateDTO = new CollectionTemplateDTO()
                {
                    CollectionId = i.Id,
                    TemplateId = templateDTO.Id,
                };
                collectionTemplateDTOs.Add(collectionTemplateDTO);
            }
            var collectionTemplate = mapper.Map<List<CollectionTemplate>>(collectionTemplateDTOs);
            var r3 = await collectionTemplateRepo.InsertAllAsync(collectionTemplate);
            //Luu vao bang phu Template Size
            var sizeTemplateDTOs = new List<TemplateSizeDTO>();
            foreach (var i in addTemplateDTO.sizeDTOs)
            {
                var sizeTemplate = new TemplateSizeDTO()
                {
                    PrintSizeId = i.Id,
                    TemplateId = templateDTO.Id,
                };
                sizeTemplateDTOs.Add(sizeTemplate);
            }
            var sizeTemplateDomain = mapper.Map<List<TemplateSize>>(sizeTemplateDTOs);
            var r4 = await templateSizeRepo.InsertAllAsync(sizeTemplateDomain);

            return templateDTO;
        }

        public async Task<List<TemplateDTO>> GetBestSeller(bool status = true)
        {
            var best = await templateRepo.GetBestSellerTemplateAsync(status);
            var templateDTOs = mapper.Map<List<TemplateDTO>>(best);
            foreach (var item in templateDTOs)
            {
                var templateImages = new List<TemplateImageDTO>(); 
                foreach (var x in item.TemplateImages)
                {
                    var image = await imageRepo.GetByIDAsync(x.Id);
                    var imageDto = mapper.Map<TemplateImageDTO>(image);
                    templateImages.Add(imageDto);
                } 
                item.TemplateImages = templateImages; 
            }
            return templateDTOs;
        }

        public async  Task<List<TemplateDTO>> GetAllAsync(string? filterOn = null, string? filterQuery = null, string? sortBy = null, bool isAscending = true, bool status = true, int pageNumber = 1, int pageSize = 1000)
        {
            var templates = await templateRepo.GetAllTemplateAsync(filterOn,filterQuery,sortBy,isAscending,status,pageNumber,pageSize);
            var templateDTOs =  mapper.Map<List<TemplateDTO>>(templates);
            foreach (var templateDTO in templateDTOs)
            {
                foreach (var item in templateDTO.CollectionTemplates)
                {
                    var collection = await collectionRepo.GetByIDAsync(item.CollectionId);
                    var collectionDTO = mapper.Map<CollectionDTO>(collection);
                    templateDTO.CollectionsDTO.Add(collectionDTO);
                }
                foreach (var item in templateDTO.TemplateSizes)
                {
                    var size = await sizeRepo.GetByIDAsync(item.PrintSizeId);
                    var sizeDTO = mapper.Map<SizeDTO>(size);
                    templateDTO.SizesDTO.Add(sizeDTO);
                }
            }
            return templateDTOs;
        }

        public async  Task<TemplateDTO> GetByIDAsync(int id)
        {
            var templateDomain = await templateRepo.GetByIDAsync(id);
            if(templateDomain == null)
            {
                return null;
            }
            var templateDTO =  mapper.Map<TemplateDTO>(templateDomain);
            foreach (var item in templateDTO.CollectionTemplates)
            {
                var collection = await collectionRepo.GetByIDAsync(item.CollectionId);
                var collectionDTO = mapper.Map<CollectionDTO>(collection);
                templateDTO.CollectionsDTO.Add(collectionDTO);
            }
            foreach (var item in templateDTO.TemplateSizes)
            {
                var size = await sizeRepo.GetByIDAsync(item.PrintSizeId);
                var sizeDTO = mapper.Map<SizeDTO>(size);
                templateDTO.SizesDTO.Add(sizeDTO);
            }
            return templateDTO;
        }

        public async Task<List<TemplateDTO>> UpdateAllStatusAsync(int[] id)
        {
            List<TemplateDTO> templateDTOs = new List<TemplateDTO>();
            foreach (var i in id)
            {
                var templateDTO = await UpdateStatusAsync(i);
                templateDTOs.Add(templateDTO);
            }
            return templateDTOs;
        }

        public async Task<TemplateDTO> UpdateAsync(int id,AddTemplateDTO updateTemplateDTO)
        {
            CultureInfo culture = new CultureInfo("en-US");

            var updatedTemplate = await templateRepo.GetByIDAsync(id);
            if (updatedTemplate == null)
            {
                return null;
            }
            updatedTemplate.Name = updateTemplateDTO.Name;
            double output;
            double.TryParse(updateTemplateDTO.PricePlusPerOne, NumberStyles.Any, culture, out output);
            updatedTemplate.PricePlusPerOne = output;
            var result = await templateRepo.UpdateAsync(updatedTemplate);
            var result2 = await UpdateDescriptionByTemplateIdAsync(id, updateTemplateDTO.DescriptionTemplates);
            if (!result && result2 == null)
            {
                return null;
            }
            if(updateTemplateDTO.formFileList != null)
            {
                var urlList = await utilService.UploadMany(updateTemplateDTO.formFileList);
                var imageDTOs = new List<TemplateImageDTO>();
                foreach (var i in urlList)
                {
                    var imageDTO = new TemplateImageDTO()
                    {
                        ImageUrl = i,
                        TemplateId = id,
                    };
                    imageDTOs.Add(imageDTO);
                }
                var templateImages = mapper.Map<List<TemplateImage>>(imageDTOs);
                var r2 = await imageRepo.InsertAllAsync(templateImages);
            }      
            var templateDTO = mapper.Map<TemplateDTO>(updatedTemplate);
            return templateDTO;

        }

        public async Task<TemplateDTO> UpdateDescriptionByTemplateIdAsync(int templateId, List<DescriptionTemplateDTO> descriptionTemplateDTOs)
        {
            var template = await templateRepo.GetByIDAsync(templateId);
            if (template == null)
            {
                return null;
            }
            foreach (var templateDescription in template.DescriptionTemplates)
            {
                foreach (var descriptionTemplateDTO in descriptionTemplateDTOs)
                {
                    if(descriptionTemplateDTO.Id == templateDescription.Id)
                    {
                        templateDescription.Title = descriptionTemplateDTO.Title;
                        templateDescription.Description = descriptionTemplateDTO.Description;
                        templateDescription.TemplateId = templateId;
                    }
                }
            }
            var result = await descriptionTemplateRepo.UpdateAllAsync(template.DescriptionTemplates.ToList());
            if (!result)
            {
                return null;
            }
            var templateDTO = mapper.Map<TemplateDTO>(template);
            return templateDTO;

        }

        public async Task<PaginationDTO<TemplateDTO>> GetByNameAsync(string? name,int page = 1,int limit = 1, bool status = true)
        {
            var templateDto = await templateRepo.GetTemplateByNameAsync(name,page,limit,status);
            var templateDTOs = mapper.Map<PaginationDTO<TemplateDTO>>(templateDto);
            return templateDTOs;
        }

        public async Task<TemplateDTO> UpdateStatusAsync(int id)
        {
            var template = await templateRepo.GetByIDAsync(id);
            if(template == null)
            {
                return null;
            }
            if(template.Status == true)
            {
                template.Status = false;
            }
            else
            {
                template.Status = true;
            }         
            var result = await templateRepo.UpdateAsync(template);
            if(!result)
            {
                return null;
            }
            return mapper.Map<TemplateDTO>(template);
        }

        public async  Task<dynamic> GetReportForAdmin()
        {
            
            var countPoInWeek = await purchaseOrderRepo.CountPurchaseInWeek();
            var sumPdInWeek = await productDetailsRepo.SumItemInWeek();
            var users = await userRepo.GetAllAsync();
            var myImage = await myImageRepo.GetAllAsync();
            var countUsersNormal = await userRepo.CountUserNormal();
            var countUsersGG = await userRepo.CountUserGoogle();
            dynamic report = new ExpandoObject();
            report.po = countPoInWeek;
            report.pd = sumPdInWeek;
            report.myImage = myImage.Count();
            report.countUsers = users.Count();
            report.normal = countUsersNormal;
            report.google = countUsersGG;

            return report;


        }

        public async Task<List<SizeDTO>> AddSizeByTemplateIdAsync(int templateId, List<SizeDTO> sizeDTOs)
        {
            var sizes = mapper.Map<List<SizeDTO>>(sizeDTOs);
            var template = await templateRepo.GetByIDAsync(templateId);
            if(template == null)
            {
                return null;
            }
            var sizesToRemove = new List<TemplateSize>(); // Tạo danh sách trung gian để lưu trữ các phần tử cần xóa

            foreach (var templateSize in template.TemplateSizes.ToList()) // Sử dụng ToList để tạo một bản sao của danh sách
            {
                if (!sizes.Any(size => size.Id == templateSize.Id))
                {
                    var result = await templateSizeRepo.DeleteAsync(templateSize);
                    if (!result)
                    {
                        return null;
                    }
                    sizesToRemove.Add(templateSize); // Thêm phần tử cần xóa vào danh sách trung gian
                }
            }

            // Xóa các phần tử cần xóa từ tập hợp chính
            foreach (var sizeToRemove in sizesToRemove)
            {
                template.TemplateSizes.Remove(sizeToRemove);
            }
            foreach (var size in sizes)
            {
                if (template.TemplateSizes.All(templateSize => templateSize.Id != size.Id))
                {
                    var sizeCreated = new TemplateSize
                    {
                        PrintSizeId = size.Id,
                        TemplateId = template.Id,
                    };
                    var result = await templateSizeRepo.InsertAsync(sizeCreated);
                    if (!result)
                    {
                        return null;
                    }
                }
            }
            return sizeDTOs;
        }

        public async Task<bool> DeleteByIdAsync(int id)
        {
           var image = await templateImageRepo.GetByIDAsync(id);
            if(image == null)
            {
                return false;
            }
            var result = await templateImageRepo.DeleteAsync(image);
            if (!result)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> DeleteAllByIdAsync(int[] arrayId)
        {
            var images = new List<TemplateImage>();
            foreach (var i in arrayId)
            {
                var image = await templateImageRepo.GetByIDAsync(i);
                if(image == null)
                {
                    return false;
                }
                images.Add(image);
            }
            var result = await templateImageRepo.DeleteAllAsync(images);
            if (!result)
            {
                return false;
            }
            return true;
        }

        public async Task<PaginationDTO<TemplateDTO>> GetAllTemplateAsync(int page = 1, int limit = 1, bool status = true)
        {
            
            var templates = await templateRepo.getAlltemplateAsync2(page, limit, status);
            if (templates == null) return null;
            var templateDTOs = mapper.Map<PaginationDTO<TemplateDTO>>(templates);
            return templateDTOs;
        }
    }
}

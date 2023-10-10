using AutoMapper;
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
        private readonly IMapper mapper;
        public TemplateService(ITemplateRepo templateRepo, IMapper mapper, IDescriptionTemplateRepo descriptionTemplateRepo, ITemplateImageRepo imageRepo, IUtilService utilService, ICollectionRepo collectionRepo, ISizeRepo sizeRepo, ICollectionTemplateRepo collectionTemplateRepo, ITemplateSizeRepo templateSizeRepo)
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
            var templateDomain = mapper.Map<Template>(addTemplateDTO);
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

        public async Task<List<TemplateDTO>> GetBestSeller()
        {
            var best = await templateRepo.GetBestSellerTemplateAsync();
            var templateDTOs = mapper.Map<List<TemplateDTO>>(best);
            foreach (var item in templateDTOs)
            {
                var templateImages = new List<TemplateImageDTO>(); 
                foreach (var x in item.TemplateImages)
                {
                    var image = await imageRepo.GetByIDAsync(x.TemplateId);
                    var imageDto = mapper.Map<TemplateImageDTO>(image);
                    templateImages.Add(imageDto);
                } 
                item.TemplateImages = templateImages; 
            }
            return templateDTOs;
        }

        public async  Task<List<TemplateDTO>> GetAllAsync(string? filterOn = null, string? filterQuery = null, string? sortBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 1000)
        {
            var templates = await templateRepo.GetAllTemplateAsync(filterOn,filterQuery,sortBy,isAscending,pageNumber,pageSize);
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
            var updatedTemplate = await templateRepo.GetByIDAsync(id);
            if (updatedTemplate == null)
            {
                return null;
            }
            updatedTemplate.Name = updateTemplateDTO.Name;
            updatedTemplate.Status = updateTemplateDTO.Status;
            updatedTemplate.CreateDate = updateTemplateDTO.CreateDate;
            updatedTemplate.QuantitySold = updateTemplateDTO.QuantitySold;
            updatedTemplate.PricePlusPerOne = updateTemplateDTO.PricePlus;
            var result = await templateRepo.UpdateAsync(updatedTemplate);
            if (!result)
            {
                return null;
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
                        templateDescription.TemplateId = descriptionTemplateDTO.TemplateId;
                    }
                }
            }
            await descriptionTemplateRepo.UpdateAllAsync(template.DescriptionTemplates.ToList());
            var templateDTO = mapper.Map<TemplateDTO>(template);
            return templateDTO;

        }

        public async Task<TemplateDTO> UpdateStatusAsync(int id)
        {
            var template = await templateRepo.GetByIDAsync(id);
            if(template == null)
            {
                return null;
            }
            template.Status = false;
            var result = await templateRepo.UpdateAsync(template);
            if(!result)
            {
                return null;
            }
            return mapper.Map<TemplateDTO>(template);
        }
    }
}

﻿using AutoMapper;
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
        private readonly ICategoryRepo categoryRepo;
        private readonly ISizeRepo sizeRepo;
        private readonly ITemplateSizeRepo templateSizeRepo;
        private readonly ICategoryTemplateRepo categoryTemplateRepo;

        private readonly IMapper mapper;
        public TemplateService(ITemplateRepo templateRepo, IMapper mapper, IDescriptionTemplateRepo descriptionTemplateRepo, ITemplateImageRepo imageRepo, IUtilService utilService,ICategoryRepo categoryRepo, ISizeRepo sizeRepo,ICategoryTemplateRepo categoryTemplateRepo,ITemplateSizeRepo templateSizeRepo)
        {
            this.templateRepo = templateRepo;
            this.mapper = mapper;
            this.descriptionTemplateRepo = descriptionTemplateRepo;
            this.imageRepo = imageRepo;
            this.utilService = utilService;
            this.categoryRepo = categoryRepo;
            this.sizeRepo = sizeRepo;
            this.templateSizeRepo = templateSizeRepo;
            this.categoryTemplateRepo = categoryTemplateRepo;
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

            //Luu vao bang DescriptionTemplate
            foreach (var item in addTemplateDTO.DescriptionTemplates)
            {
                item.TemplateId = templateDTO.Id;
            }
            var descriptionTemplates = mapper.Map<List<DescriptionTemplate>>(addTemplateDTO.DescriptionTemplates.ToList());
            var r = await descriptionTemplateRepo.InsertAllAsync(descriptionTemplates);

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
            var categoryTemplateDTOs = new List<CategoryTemplateDTO>();
            foreach(var i in addTemplateDTO.categoryDTOs)
            {
                var categoryTemplateDTO = new CategoryTemplateDTO()
                {
                    CategoryId = i.Id,
                    TemplateId = templateDTO.Id,
                };
                categoryTemplateDTOs.Add(categoryTemplateDTO);
            }
            var categoryTemplate = mapper.Map<List<CategoryTemplate>>(categoryTemplateDTOs);
            var r3 = await categoryTemplateRepo.InsertAllAsync(categoryTemplate);
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

        public async  Task<List<TemplateDTO>> GetAllAsync(string? filterOn = null, string? filterQuery = null, string? sortBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 1000)
        {
            var templates = await templateRepo.GetAllTemplateAsync(filterOn,filterQuery,sortBy,isAscending,pageNumber,pageSize);
            var templateDTOs =  mapper.Map<List<TemplateDTO>>(templates);
            foreach (var templateDTO in templateDTOs)
            {
                foreach (var item in templateDTO.CategoryTemplates)
                {
                    var category = await categoryRepo.GetByIDAsync(item.CategoryId);
                    var categoryDTO = mapper.Map<CategoryDTO>(category);
                    templateDTO.CategoriesDTO.Add(categoryDTO);
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
            foreach (var item in templateDTO.CategoryTemplates)
            {
                var category = await categoryRepo.GetByIDAsync(item.CategoryId);
                var categoryDTO = mapper.Map<CategoryDTO>(category);
                templateDTO.CategoriesDTO.Add(categoryDTO);
            }
            foreach (var item in templateDTO.TemplateSizes)
            {
                var size = await sizeRepo.GetByIDAsync(item.PrintSizeId);
                var sizeDTO = mapper.Map<SizeDTO>(size);
                templateDTO.SizesDTO.Add(sizeDTO);
            }
            return templateDTO;
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
            updatedTemplate.PricePlus = updateTemplateDTO.PricePlus;
            var result = await templateRepo.UpdateAsync(updatedTemplate);
            if (!result)
            {
                return null;
            }
            var templateDTO = mapper.Map<TemplateDTO>(updatedTemplate);
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

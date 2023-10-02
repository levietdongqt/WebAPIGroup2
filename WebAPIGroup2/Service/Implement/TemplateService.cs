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

        private readonly IMapper mapper;
        public TemplateService(ITemplateRepo templateRepo, IMapper mapper, IDescriptionTemplateRepo descriptionTemplateRepo, ITemplateImageRepo imageRepo, IUtilService utilService)
        {
            this.templateRepo = templateRepo;
            this.mapper = mapper;
            this.descriptionTemplateRepo = descriptionTemplateRepo;
            this.imageRepo = imageRepo;
            this.utilService = utilService;
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
            foreach (var item in addTemplateDTO.DescriptionTemplates)
            {
                item.TemplateId = templateDTO.Id;
            }
            var descriptionTemplates = mapper.Map<List<DescriptionTemplate>>(addTemplateDTO.DescriptionTemplates.ToList());


            var urlList = await utilService.UploadMany(addTemplateDTO.formFileList);

            var r = await descriptionTemplateRepo.InsertAllAsync(descriptionTemplates);

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
        
            return templateDTO;
        }

        public async  Task<List<TemplateDTO>> GetAllAsync(string? filterOn = null, string? filterQuery = null, string? sortBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 1000)
        {
            var templates = await templateRepo.GetAllTemplateAsync(filterOn,filterQuery,sortBy,isAscending,pageNumber,pageSize);
            return mapper.Map<List<TemplateDTO>>(templates);
        }

        public async  Task<TemplateDTO> GetByIDAsync(int id)
        {
            var templateDomain = await templateRepo.GetByIDAsync(id);
            if(templateDomain == null)
            {
                return null;
            }
            return mapper.Map<TemplateDTO> (templateDomain);
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
    }
}

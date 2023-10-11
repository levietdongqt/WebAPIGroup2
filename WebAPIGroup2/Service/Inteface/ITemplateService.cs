using WebAPIGroup2.Models.DTO;

namespace WebAPIGroup2.Service.Inteface
{
    public interface ITemplateService
    {
        Task<List<TemplateDTO>> GetBestSeller();
        Task<List<TemplateDTO>> GetAllAsync(string? filterOn = null, string? filterQuery = null, string? sortBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 1000);

        Task<TemplateDTO> CreateAsync(AddTemplateDTO addTemplateDTO);

        Task<TemplateDTO> UpdateAsync(int id,AddTemplateDTO updateTemplateDTO);

        Task<TemplateDTO> GetByIDAsync(int id);

        Task<TemplateDTO> UpdateStatusAsync(int id);

        Task<List<TemplateDTO>> UpdateAllStatusAsync(int[] id);

        Task<TemplateDTO> AddDescriptionByTemplateIdAsync(int templateId,List<DescriptionTemplateDTO> descriptionTemplateDTOs);

        Task<TemplateDTO> AddImageByTemplateIdAsync(int templateId, IFormFile[] formFiles);

        Task<TemplateDTO> UpdateDescriptionByTemplateIdAsync(int templateId, List<DescriptionTemplateDTO> descriptionTemplateDTOs);

    }
}

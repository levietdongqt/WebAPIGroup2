using WebAPIGroup2.Models.DTO;

namespace WebAPIGroup2.Service.Inteface
{
    public interface ITemplateService
    {
        Task<List<TemplateDTO>> GetAllAsync(string? filterOn = null, string? filterQuery = null, string? sortBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 1000);

        Task<TemplateDTO> CreateAsync(AddTemplateDTO addTemplateDTO);

        Task<TemplateDTO> UpdateAsync(int id,AddTemplateDTO updateTemplateDTO);

        Task<TemplateDTO> GetByIDAsync(int id);
    }
}

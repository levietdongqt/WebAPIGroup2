using WebAPIGroup2.Models.DTO;

namespace WebAPIGroup2.Service.Inteface;

public interface ITemplateService 
{
    Task<IEnumerable<TemplateDTO>?> GetAllAsync();
    Task<TemplateWithCategoryDTO?> GetTemplateById(int id);
    Task<TemplateDTO> CreateTemplate(TemplateDTO template);
    Task<bool> updateTemplate(TemplateDTO template);
    Task<bool> deleteTemplate(int id);
    
}
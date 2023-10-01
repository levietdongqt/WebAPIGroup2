using WebAPIGroup2.Models.DTO;
using WebAPIGroup2.Models.POJO;

namespace WebAPIGroup2.Respository.Inteface;

public interface ITemplateRepo : ISharedRepository<Template,int>
{
    Task<TemplateWithCategoryDTO?> GetTemplateByID(int id);
}
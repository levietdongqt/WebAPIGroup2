using WebAPIGroup2.Models.DTO;
using WebAPIGroup2.Models.POJO;

namespace WebAPIGroup2.Respository.Inteface;

public interface ICategoryRepo : ISharedRepository<Category,int>
{
    Task<CategoryWithTemplateDTO?> GetCategoryWithTemplate(int id);
}
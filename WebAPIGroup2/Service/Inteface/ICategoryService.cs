using WebAPIGroup2.Models.DTO;
using WebAPIGroup2.Models.POJO;

namespace WebAPIGroup2.Service.Inteface;

public interface ICategoryService
{
    Task<IEnumerable<CategoryDTO>?> GetAllasync();
    Task<CategoryDTO?> GetCategoryById(int id);
    Task<CategoryWithTemplateDTO> GetCategoryWithTemplate(int id);
    Task<bool> UpdateCategory(CategoryDTO category);
    Task<bool> DeleteCategory(int id);
    Task<CategoryDTO> CreateCategory(CategoryDTO category);
}
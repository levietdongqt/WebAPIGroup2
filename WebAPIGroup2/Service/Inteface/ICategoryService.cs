using WebAPIGroup2.Models.DTO;


namespace WebAPIGroup2.Service.Inteface;

public interface ICategoryService
{
    Task<List<CategoryDTO>> GetAllCategories();
    Task<CategoryDTO?> GetCategoryById(int id);
    Task<CategoryDTO?> UpdateCategory(int id,CategoryDTO category);
    Task<bool> DeleteCategory(int id);
    Task<CategoryDTO?> CreateCategory(CategoryDTO category);
}

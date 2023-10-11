using WebAPIGroup2.Models.POJO;


namespace WebAPIGroup2.Respository.Inteface;

public interface ICategoryRepo : IBaseRepository<Category, int>
{
    Task<List<Category>> GetAllCategoryAsync();
}

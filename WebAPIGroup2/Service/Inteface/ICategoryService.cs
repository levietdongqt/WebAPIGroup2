using WebAPIGroup2.Models.DTO;

namespace WebAPIGroup2.Service.Inteface
{
    public interface ICategoryService
    {
        Task<List<CategoryDTO>> GetAll();
    }
}

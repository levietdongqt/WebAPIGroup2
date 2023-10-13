using WebAPIGroup2.Models.DTO;
using WebAPIGroup2.Models.POJO;

namespace WebAPIGroup2.Respository.Inteface
{
    public interface IProductDetailsRepo : IBaseRepository<ProductDetail, int>
    {
        Task<ProductDetail?> GetByMyImageId(OrderDTO orderDTO);
    }
}

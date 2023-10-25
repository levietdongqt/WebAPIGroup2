using WebAPIGroup2.Models.DTO;
using WebAPIGroup2.Models.POJO;

namespace WebAPIGroup2.Respository.Inteface
{
    public interface IProductDetailsRepo : IBaseRepository<ProductDetail, int>
    {
        List<ProductDetail> getByIdList(List<int> productIdList);
        Task<ProductDetail?> GetByMyImageId(OrderDTO orderDTO);

        Task<int?> SumItemInWeek();
    }
}

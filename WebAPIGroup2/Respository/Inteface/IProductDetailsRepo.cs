using WebAPIGroup2.Models.POJO;

namespace WebAPIGroup2.Respository.Inteface
{
    public interface IProductDetailsRepo : IBaseRepository<ProductDetail, int>
    {
        Task<List<ProductDetail>> getProductDetailByOrder(int OrderID);
    }
}

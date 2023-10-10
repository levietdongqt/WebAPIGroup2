using WebAPIGroup2.Models;
using WebAPIGroup2.Models.POJO;
using WebAPIGroup2.Respository.Inteface;

namespace WebAPIGroup2.Respository.Implement
{
    public class ProductDetailRepo : GenericRepository<ProductDetail>, IProductDetailsRepo
    {
        public ProductDetailRepo(MyImageContext context) : base(context)
        {
        }

        public Task<ProductDetail?> GetByIDAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}

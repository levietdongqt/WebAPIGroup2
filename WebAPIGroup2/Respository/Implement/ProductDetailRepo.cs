using MailKit.Search;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Core;
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
            try
            {
               return _context.ProductDetails.FirstOrDefaultAsync(x => x.Id == id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        
    }
}

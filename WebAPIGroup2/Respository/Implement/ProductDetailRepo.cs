using MailKit.Search;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Core;
using WebAPIGroup2.Models;
using WebAPIGroup2.Models.DTO;
using WebAPIGroup2.Models.POJO;
using WebAPIGroup2.Respository.Inteface;

namespace WebAPIGroup2.Respository.Implement
{
    public class ProductDetailRepo : GenericRepository<ProductDetail>, IProductDetailsRepo
    {
        public ProductDetailRepo(MyImageContext context) : base(context)
        {
        }

        public async Task<ProductDetail?> GetByIDAsync(int id)
        {
            try
            {
               return await _context.ProductDetails.FirstOrDefaultAsync(x => x.Id == id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public async Task<ProductDetail?> GetByMyImageId(OrderDTO orderDTO)
        {
            try
            {
                var product = await _context.ProductDetails.FirstOrDefaultAsync(t => t.MyImageId == orderDTO.myImageID && t.MaterialPageId == orderDTO.materialPageId && t.TemplateSizeId == orderDTO.temlateSizeId);
                return product;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        
    }
}

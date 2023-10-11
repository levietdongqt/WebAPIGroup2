using Microsoft.EntityFrameworkCore;
using WebAPIGroup2.Models;
using WebAPIGroup2.Models.POJO;
using WebAPIGroup2.Respository.Inteface;

namespace WebAPIGroup2.Respository.Implement
{
    public class MyImageRepo : GenericRepository<MyImage>, IMyImageRepo
    {
        public MyImageRepo(MyImageContext context) : base(context)
        {
        }

        public async Task<MyImage?> GetByIDAsync(int id)
        {
           return await _context.MyImages.Include(t => t.Images).Include(t => t.Template).FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<List<MyImage>> getByOrder(int purchaseOrderId)
        {
            try
            {
                var list = await _context.MyImages.Where(t => t.PurchaseOrderId == purchaseOrderId).ToListAsync();
                if(list.Count == 0)
                {
                    return null;
                }
                return list;
            }
            catch (Exception e )
            {
                Console.WriteLine(e);
                return null;
            }
        }
    }
}

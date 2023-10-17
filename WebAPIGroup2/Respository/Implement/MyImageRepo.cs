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
                var list = await _context.MyImages.Include(t=> t.PurchaseOrder).Include(t=>t.Images).Where(t => t.PurchaseOrderId == purchaseOrderId).ToListAsync();
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

        public async Task<List<MyImage>> getByUserId(int userID)
        {
            try
            {
                var listPurchase = await _context.PurchaseOrders.Include(t => t.MyImages).ThenInclude(t=> t.Images).Where(t => t.UserId == userID && (t.Status == PurchaseStatus.Temporary || t.Status == PurchaseStatus.InCart)).ToListAsync();
                if(listPurchase.Count == 0)
                {
                    return null;
                }
                int[] purchaseIDs = new int[listPurchase.Count];
                listPurchase.ForEach(purchase =>
                {
                    purchaseIDs[0] = purchase.Id;
                });
                List<MyImage> list = await _context.MyImages.Include(t=> t.Images).Include(t=> t.Template).ThenInclude(t=> t.TemplateSizes).ThenInclude(t=> t.PrintSize).Where(t=> purchaseIDs.Contains(t.Id)).ToListAsync();
                if(list.Count == 0)
                {
                    return null;
                }
                return list;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public async Task<List<MyImage>> loadInCart(int userID)
        {
            try
            {
                var puchase = await _context.PurchaseOrders.Include(t => t.MyImages).FirstOrDefaultAsync(t => t.UserId == userID && t.Status == PurchaseStatus.InCart);
                if (puchase == null)
                {
                    return null;
                }
                var myImages = await _context.MyImages
                    .Include(t=> t.Images)
                    .Include(t=> t.Template)
                    .Include(t=> t.ProductDetails)
                        .ThenInclude(t=> t.MaterialPage)
                    .Include(t=> t.ProductDetails)
                        .ThenInclude(pd=> pd.TemplateSize).ThenInclude(ps=> ps.PrintSize)
                    .Where(t => t.PurchaseOrderId == puchase.Id).ToListAsync();
                if(myImages.Count == 0)
                {
                    return null;
                }
                return myImages;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
    }
}

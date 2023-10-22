using Microsoft.EntityFrameworkCore;
using WebAPIGroup2.Models;
using WebAPIGroup2.Models.POJO;
using WebAPIGroup2.Respository.Inteface;

namespace WebAPIGroup2.Respository.Implement
{
    public class PurchaseOrderRepo : GenericRepository<PurchaseOrder>, IPurchaseOrderRepo
    {
        public PurchaseOrderRepo(MyImageContext context) : base(context)
        {
        }

        public Task<PurchaseOrder?> GetByIDAsync(int id)
        {
            return _context.PurchaseOrders.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<PurchaseOrder> getPurchaseOrder(int userID, string status)
        {
            try
            {   
                if(status.Equals(PurchaseStatus.Temporary))
                {
                    var purchase = await _context.PurchaseOrders.FirstOrDefaultAsync(t => t.UserId == userID && (t.Status == status || t.Status == PurchaseStatus.InCart));
                    if (purchase == null)
                    {
                        throw new Exception("Purchare is not exist!");
                    }
                    return purchase;
                }
                var purchase2 =  await _context.PurchaseOrders.FirstOrDefaultAsync(t => t.UserId == userID && t.Status == status);
                if(purchase2 == null)
                {
                    throw new Exception("Purchare is not exist!");
                }
                return purchase2;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
    }
}

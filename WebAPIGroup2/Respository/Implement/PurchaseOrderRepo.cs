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

        public async Task<PurchaseOrder?> GetByIDAsync(int id)
        {
            var pur = await _context.PurchaseOrders.FirstOrDefaultAsync(x => x.Id == id);
            return pur; // Trả về kết quả tìm thấy hoặc null nếu không tìm thấy
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

        public async Task<IEnumerable<PurchaseOrder>> GetPurchaseOrdersByStatus(int userID, List<string> statuses)
        {
            try
            {
                var purchases = await _context.PurchaseOrders
                    .Where(po => po.UserId == userID && statuses.Contains(po.Status))
                    .ToListAsync();

                return purchases;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

 
    }
}

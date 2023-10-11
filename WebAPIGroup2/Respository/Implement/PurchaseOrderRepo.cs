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
            throw new NotImplementedException();
        }

        public async Task<PurchaseOrder> getPurchaseOrder(int userID, string status)
        {
            try
            {
                var purchase =  await _context.PurchaseOrders.FirstOrDefaultAsync(t => t.UserId == userID && t.Status == status);
                if(purchase == null)
                {
                    throw new Exception("Purchare is not exist!");
                }
                else { return purchase; }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
    }
}

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
    }
}

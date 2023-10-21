using WebAPIGroup2.Respository.Inteface;
using WebAPIGroup2.Service.Inteface;

namespace WebAPIGroup2.Service.Implement
{
    public class PurchaseOrderService : IPurchaseOrderService
    {
        private readonly IPurchaseOrderRepo _purchaseOrderRepo;

        public PurchaseOrderService(IPurchaseOrderRepo purchaseOrderRepo)
        {
            _purchaseOrderRepo = purchaseOrderRepo;
        }
        public async Task<dynamic> GetPurchaseOrderByMonth()
        {
            var po = await _purchaseOrderRepo.GetSumPriceTotalByMonth();
            if(po == null)
            {
                return null;
            }
            return po;
        }
    }
}

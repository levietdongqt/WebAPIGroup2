using WebAPIGroup2.Models.DTO;

namespace WebAPIGroup2.Service.Inteface
{
    public interface IPurchaseOrderService
    {
        public Task<IEnumerable<PurchaseOrderDTO>> GetPurchaseOrderAll();
        public  Task<PurchaseOrderDTO?>  GetPurchaseOrderByUserId(int userId,string status);
        Task<dynamic> GetPurchaseOrderByMonth();

    }
}

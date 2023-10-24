using WebAPIGroup2.Models.DTO;

namespace WebAPIGroup2.Service.Inteface
{
    public interface IPurchaseOrderService
    {
        Task<IEnumerable<PurchaseOrderDTO>?> GetAllAsync(string? search, string? st, int page, int pageSize);
        Task<IEnumerable<PurchaseOrderDTO>?> GetPurchaseOrdersByStatus(int userID, List<string> statuses);
        Task<PurchaseOrderDTO> GetPurchaseOrdersByIDAsync(int id);
        Task<PurchaseOrderDTO> UpdatePurchaseOrder(PurchaseOrderDTO purchaseOrderDTO);
         Task<PurchaseOrderDTO?>  GetPurchaseOrderByUserId(int userId,string status);
        Task<dynamic> GetPurchaseOrderByMonth();

    }
}

using WebAPIGroup2.Models.POJO;

namespace WebAPIGroup2.Respository.Inteface
{
    public interface IPurchaseOrderRepo : IBaseRepository<PurchaseOrder, int>
    {
        Task<PurchaseOrder> getPurchaseOrder(int userID, string status);
        
    }
}

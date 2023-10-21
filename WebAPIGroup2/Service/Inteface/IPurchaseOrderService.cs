namespace WebAPIGroup2.Service.Inteface
{
    public interface IPurchaseOrderService
    {
        Task<dynamic> GetPurchaseOrderByMonth();

    }
}

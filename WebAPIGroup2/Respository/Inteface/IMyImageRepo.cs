using WebAPIGroup2.Models.POJO;

namespace WebAPIGroup2.Respository.Inteface
{
    public interface IMyImageRepo : IBaseRepository<MyImage, int>
    {
        Task<List<MyImage>> getByOrder(int purchaseOrderId);
        Task<MyImage> getByPurchaseTemplate(int id, int? templateID);
        Task<List<MyImage>> getByUserId(int userID);
        Task<List<MyImage>> getNoTemplate(int userID);
        Task<List<MyImage>> loadInCart(int userID);
    }
}

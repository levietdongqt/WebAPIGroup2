using WebAPIGroup2.Models.POJO;

namespace WebAPIGroup2.Respository.Inteface
{
    public interface IMyImageRepo : IBaseRepository<MyImage, int>
    {
        Task<List<MyImage>> getByOrder(int purchaseOrderId);
        Task<List<MyImage>> getByUserId(int userID);
        Task<List<MyImage>> loadInCart(int userID);
    }
}

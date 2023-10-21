using WebAPIGroup2.Models.POJO;

namespace WebAPIGroup2.Respository.Inteface
{
    public interface IFeedBackRepo : IBaseRepository<FeedBack,int>
    {
        Task<dynamic> GetFeedBackTake5News();
    }
}

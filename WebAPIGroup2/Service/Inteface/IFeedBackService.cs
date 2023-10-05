using WebAPIGroup2.Models.DTO;

namespace WebAPIGroup2.Service.Inteface
{
    public interface IFeedBackService
    {
        Task<List<FeedBackDTO>> GetAllAsync(string? filterOn = null, string? filterQuery = null, string? sortBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 1000);
    }
}

using WebAPIGroup2.Models.DTO;
using WebAPIGroup2.Respository.Inteface;
using WebAPIGroup2.Service.Inteface;

namespace WebAPIGroup2.Service.Implement
{
    public class FeedBackService : IFeedBackService
    {
        public Task<List<FeedBackDTO>> GetAllAsync(string? filterOn = null, string? filterQuery = null, string? sortBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 1000)
        {
            throw new NotImplementedException();
        }
    }
}

using Microsoft.EntityFrameworkCore;
using WebAPIGroup2.Models;
using WebAPIGroup2.Models.POJO;
using WebAPIGroup2.Respository.Inteface;

namespace WebAPIGroup2.Respository.Implement
{
    public class FeedBackRepo : GenericRepository<FeedBack>, IFeedBackRepo
    {
        public FeedBackRepo(MyImageContext context) : base(context)
        {
        }

        public async Task<FeedBack?> GetByIDAsync(int id)
        {
            return await _context.FeedBacks.FirstOrDefaultAsync(x=>x.Id == id);
        }
    }
}

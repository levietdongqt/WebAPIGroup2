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

        public async Task<dynamic> GetFeedBackTake5News()
        {
            var query = from fb in _context.FeedBacks
                        join user in _context.Users
                        on fb.UserId equals user.Id
                        orderby fb.FeedBackDate descending
                        select new
                        {
                            name = $"{user.FullName}-{user.Email}",
                            image = user.Avatar,
                            createDate = fb.FeedBackDate,
                            content = fb.Content
                        };
            return await query.Take(5).ToListAsync();

        }
    }
}

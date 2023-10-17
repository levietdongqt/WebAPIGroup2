using Microsoft.EntityFrameworkCore;
using WebAPIGroup2.Models;
using WebAPIGroup2.Models.POJO;
using WebAPIGroup2.Respository.Inteface;

namespace WebAPIGroup2.Respository.Implement
{
    public class ReviewRepo : GenericRepository<Review>, IReviewRepo
    {
        public ReviewRepo(MyImageContext context) : base(context)
        {
        }
        
        public async Task<Review?> GetByIDAsync(int id)
        {
            return await _context.Reviews.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}

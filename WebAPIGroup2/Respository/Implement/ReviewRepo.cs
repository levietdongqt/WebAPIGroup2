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

        public Task<Review?> GetByIDAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}

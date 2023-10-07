using Microsoft.EntityFrameworkCore;
using WebAPIGroup2.Models;
using WebAPIGroup2.Models.POJO;
using WebAPIGroup2.Respository.Inteface;

namespace WebAPIGroup2.Respository.Implement
{
    public class CategoryRepo : GenericRepository<Category>, ICategoryRepo
    {
        public CategoryRepo(MyImageContext context) : base(context)
        {
        }

        public async Task<List<Category>> GetAll()
        {
            return await _context.Categories.Include(co=>co.Collections).ToListAsync();
        }

        public Task<Category?> GetByIDAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}

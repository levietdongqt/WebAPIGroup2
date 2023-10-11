using Microsoft.EntityFrameworkCore;
using WebAPIGroup2.Models;
using WebAPIGroup2.Models.POJO;
using WebAPIGroup2.Respository.Inteface;


namespace WebAPIGroup2.Respository.Implement;

public class CategoryRepo : GenericRepository<Category>, ICategoryRepo
{

    public CategoryRepo(MyImageContext context) : base(context)
    {
        
    }
    
    
    public async Task<Category?> GetByIDAsync(int id)
    {
        return await _context.Categories.Include(c => c.Collections).ThenInclude(c => c.CollectionTemplates).FirstOrDefaultAsync(c => c.Id == id);
    }

    

    public async Task<List<Category>> GetAllCategoryAsync()
    {
        var result = await _context.Categories.Include(c => c.Collections).ToListAsync();
        return result;
    }
}


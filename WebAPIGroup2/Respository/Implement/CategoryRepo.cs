using Microsoft.EntityFrameworkCore;
using WebAPIGroup2.Models;
using WebAPIGroup2.Models.DTO;
using WebAPIGroup2.Models.POJO;
using WebAPIGroup2.Respository.Inteface;

namespace WebAPIGroup2.Respository.Implement;

public class CategoryRepo : GenericRepository<Category> , ICategoryRepo
{
    public CategoryRepo(MyImageContext context) : base(context)
    {
    }

    public async Task<Category?> GetByIDAsync(int id)
    {
        var category = await _context.Categories.FirstOrDefaultAsync(s => s.Id == id);
        if (category == null)
        {
            return null;
        }
        return category;
    }

    public async Task<CategoryWithTemplateDTO?> GetCategoryWithTemplate(int id)
    {
        var category = await _context.Categories.Where(s => s.Id == id)
            .Select(s => new CategoryWithTemplateDTO()
            {
                Id = s.Id,
                Name = s.Name,
                TemplateNames = s.CategoryTemplates.Select(c => c.Template.Name).ToList()
            }).FirstOrDefaultAsync();
        return category;
    }

    
    
}
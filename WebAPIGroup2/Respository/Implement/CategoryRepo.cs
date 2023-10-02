using Microsoft.EntityFrameworkCore;
using WebAPIGroup2.Models;
using WebAPIGroup2.Models.DTO;
using WebAPIGroup2.Models.POJO;
using WebAPIGroup2.Respository.Inteface;

namespace WebAPIGroup2.Respository.Implement;

public class CategoryRepo : GenericRepository<Category> , ICategoryRepo
{
    private readonly MyImageContext _context;
    private readonly GenericRepository<Category> _Repository;

    public CategoryRepo(MyImageContext context, GenericRepository<Category> repository) : base(context)
    {
        _context = context;
        _Repository = repository;
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

    public new async Task<IEnumerable<Category>?> GetAllAsync()
    {
        return await _Repository.GetAllAsync();
    }
    public async Task<bool> InsertAsync(Category entity)
    {
        return await _Repository.InsertAsync(entity);
    }
    public async Task<bool> InsertAllAsync(List<Category> list)
    {
        return await _Repository.InsertAllAsync(list);
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

    public async Task<bool> UpdateAsync(Category entity)
    {
        return await _Repository.UpdateAsync(entity);
    }
    public async Task<bool> UpdateAllAsync(List<Category> list)
    {
        return await _Repository.UpdateAllAsync(list);
    }

    public async Task<bool> DeleteAsync(Category entity)
    {
        return await _Repository.DeleteAsync(entity);
    }
    public async Task<bool> DeleteAllAsync(List<Category> list)
    {
        return await _Repository.DeleteAllAsync(list);
    }
    
}
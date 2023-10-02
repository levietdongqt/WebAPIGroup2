using Microsoft.EntityFrameworkCore;
using WebAPIGroup2.Models;
using WebAPIGroup2.Models.DTO;
using WebAPIGroup2.Models.POJO;
using WebAPIGroup2.Respository.Inteface;

namespace WebAPIGroup2.Respository.Implement;

public class TemplateRepo : GenericRepository<Template>, ITemplateRepo
{
    private readonly MyImageContext _context;
    private readonly GenericRepository<Template> _templateRepository;
    public TemplateRepo(MyImageContext context, GenericRepository<Template> templateRepository) : base(context)
    {
        _context = context;
        _templateRepository = templateRepository;
    }

    public async Task<Template?> GetByIDAsync(int id)
    {
        var template = await _context.Templates.SingleOrDefaultAsync(s=> s.Id == id);
        if (template == null)
        {
            return null;
        }
        return template;
        
    }

    public async Task<TemplateWithCategoryDTO?> GetTemplateByID(int id)
    {
        var _categoryTemplate = await _context.Templates.Where(s => s.Id == id)
            .Select(n => new TemplateWithCategoryDTO()
            {
                Id = n.Id,
                Name = n.Name,
                PricePlus = n.PricePlus,
                Status = n.Status,
                QuantitySold = n.QuantitySold,
                CreateDate = n.CreateDate,
                CategoryNames = n.CategoryTemplates.Select(n=> n.Category.Name).ToList()
            }).FirstOrDefaultAsync();
        return _categoryTemplate;
    }
    public async Task<IEnumerable<Template>?> GetAllAsync()
    {
        return await _templateRepository.GetAllAsync();
    }

    public async Task<bool> InsertAsync(Template entity)
    {
        return await _templateRepository.InsertAsync(entity);
    }

    public async Task<bool> InsertAllAsync(List<Template> list)
    {
        return await _templateRepository.InsertAllAsync(list);
    }

    public async Task<bool> UpdateAsync(Template entity)
    {
        return await _templateRepository.UpdateAsync(entity);
    }

    public async Task<bool> UpdateAllAsync(List<Template> list)
    {
        return await _templateRepository.UpdateAllAsync(list);
    }

    public async Task<bool> DeleteAsync(Template entity)
    {
        return await _templateRepository.DeleteAsync(entity);
    }

    public async Task<bool> DeleteAllAsync(List<Template> list)
    {
        return await _templateRepository.DeleteAllAsync(list);
    }
}
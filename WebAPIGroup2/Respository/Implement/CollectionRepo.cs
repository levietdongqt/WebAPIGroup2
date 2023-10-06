using Microsoft.EntityFrameworkCore;
using WebAPIGroup2.Models;
using WebAPIGroup2.Models.DTO;
using WebAPIGroup2.Models.POJO;
using WebAPIGroup2.Respository.Inteface;

namespace WebAPIGroup2.Respository.Implement;

public class CollectionRepo : GenericRepository<Collection> , ICollectionRepo
{
    public CollectionRepo(MyImageContext context) : base(context)
    {
    }

    public async Task<Collection?> GetByIDAsync(int id)
    {
        var collection = await _context.Collections.FirstOrDefaultAsync(s => s.Id == id);
        if (collection == null)
        {
            return null;
        }
        return collection;
    }

    public async Task<CollectionWithTemplateDTO?> GetCollectionWithTemplate(int id)
    {
        var collection = await _context.Collections.Where(s => s.Id == id)
            .Select(s => new CollectionWithTemplateDTO()
            {
                Id = s.Id,
                Name = s.Name,
                TemplateNames = s.CollectionTemplates.Select(c => c.Template.Name).ToList()
            }).FirstOrDefaultAsync();
        return collection;
    }

    
    
}
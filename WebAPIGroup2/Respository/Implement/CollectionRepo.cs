using Microsoft.AspNetCore.Mvc.ViewFeatures;
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

    public async Task<PaginationDTO<CollectionWithTemplateDTO?>> GetCollectionWithTemplate(int id, int page =1 , int limit = 1,bool status = true)
    {
        var collection = _context.Collections.AsQueryable();
        if (id != 0)
        {
            collection = collection.Where(x => x.Id.Equals(id));
        }
        
        var item = await collection.Select(s=> new CollectionWithTemplateDTO
        {
            Id = s.Id,
            Name = s.Name,
            TemplateNames = s.CollectionTemplates.Select( c=> new TemplateDTO()
            {
                Id = c.Template.Id,
                Name = c.Template.Name,
                PricePlusPerOne = c.Template.PricePlusPerOne,
                Status = c.Template.Status,
                QuantitySold = c.Template.QuantitySold,
            }).Where(x=>x.Status == status).ToList()
        }).Skip((page - 1) * limit).Take(limit).ToListAsync();
        int totalRows = item.Sum(x => x.TemplateNames.Count);
        var paginations = new PaginationDTO<CollectionWithTemplateDTO>()
        {
            limit = limit,
            Page = page,
            totalRows = totalRows,
            Items = item
        };
        return paginations;
    }

    public async Task<List<Collection>> getCollectionFeatures()
    {
        var randomFeatures = await _context.Collections.Include(x => x.Category)
            .OrderBy(x => Guid.NewGuid()) // Randomize the order of the collections
            .Take(4) // Retrieve 5 random collections
            .ToListAsync();

        return randomFeatures;
    }
}
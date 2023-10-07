using Microsoft.EntityFrameworkCore;
using WebAPIGroup2.Models;
using WebAPIGroup2.Models.POJO;
using WebAPIGroup2.Respository.Inteface;

namespace WebAPIGroup2.Respository.Implement
{
    public class TemplateRepo : GenericRepository<Template>, ITemplateRepo
    {
        public TemplateRepo(MyImageContext context) : base(context)
        {
        }
        
        public async Task<Template?> GetByIDAsync(int id)
        {
            return await _context.Templates.Include(i => i.TemplateImages).Include(d => d.DescriptionTemplates).Include(c=>c.CollectionTemplates).Include(s=>s.TemplateSizes).Include(r=>r.Reviews).ThenInclude(u=>u.User).FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<List<Template>> GetAllTemplateAsync(string? filterOn = null, string? filterQuery = null, string? sortBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 1000)
        {
            var list = _context.Templates.Include(i => i.TemplateImages).Include(d => d.DescriptionTemplates).Include(c => c.CollectionTemplates).Include(s => s.TemplateSizes).Include(r => r.Reviews).ThenInclude(u => u.User).AsQueryable();

            //Filter
            if (string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false)
            {
                if (filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    list = list.Where(x => x.Name.Contains(filterQuery));
                }
                else if (filterOn.Equals("Status", StringComparison.OrdinalIgnoreCase))
                {
                    list = list.Where(x => x.Status == Boolean.Parse(filterQuery));
                }
                else if (filterOn.Equals("PricePlus", StringComparison.OrdinalIgnoreCase))
                {
                    list = list.Where(x => x.PricePlus >= Decimal.Parse(filterQuery));
                }
            }

            //Sort
            if ((string.IsNullOrWhiteSpace(sortBy) == false))
            {
                if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    list = isAscending ? list.OrderBy(x => x.Name) : list.OrderByDescending(x => x.Name);
                }
                else if (sortBy.Equals("PricePlus", StringComparison.OrdinalIgnoreCase))
                {
                    list = isAscending ? list.OrderBy(x => x.PricePlus) : list.OrderByDescending(x => x.PricePlus);
                }
                else if (sortBy.Equals("QuantitySold", StringComparison.OrdinalIgnoreCase))
                {
                    list = isAscending ? list.OrderBy(x => x.QuantitySold) : list.OrderByDescending(x => x.QuantitySold);
                }
            }
            //Pagination
            var skipResult = (pageNumber - 1) * pageSize;

            return await list.Skip(skipResult).Take(pageSize).ToListAsync();
        }

        public async Task<List<Template>> GetBestSellerTemplateAsync()
        {
            return await _context.Templates.Include(c=>c.TemplateImages).OrderByDescending(t => t.QuantitySold).Take(8).ToListAsync();
        }
    }
}


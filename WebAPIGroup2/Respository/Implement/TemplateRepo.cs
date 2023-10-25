using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebAPIGroup2.Models;
using WebAPIGroup2.Models.DTO;
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

        public async Task<List<Template>> GetAllTemplateAsync(string? filterOn = null, string? filterQuery = null, string? sortBy = null, bool isAscending = true, bool status = true, int pageNumber = 1, int pageSize = 1000)
        {
            var list = _context.Templates.Include(i => i.TemplateImages).AsQueryable();

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

                    list = list.Where(x => x.PricePlusPerOne >= float.Parse(filterQuery));
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
                    list = isAscending ? list.OrderBy(x => x.PricePlusPerOne) : list.OrderByDescending(x => x.PricePlusPerOne);
                }
                else if (sortBy.Equals("QuantitySold", StringComparison.OrdinalIgnoreCase))
                {
                    list = isAscending ? list.OrderBy(x => x.QuantitySold) : list.OrderByDescending(x => x.QuantitySold);
                }
                else if (sortBy.Equals("CreateDate", StringComparison.OrdinalIgnoreCase))
                {
                    list = isAscending ? list.OrderBy(x => x.CreateDate) : list.OrderByDescending(x => x.CreateDate);
                }
            }
            list = list.Where(x => x.Status == status);
            //Pagination
            var skipResult = (pageNumber - 1) * pageSize;

            return await list.Skip(skipResult).Take(pageSize).ToListAsync();
        }

        public async Task<List<Template>> GetBestSellerTemplateAsync(bool status = true)
        {
            var templates = _context.Templates
                .Include(t => t.TemplateImages);

            return await templates
                .Where(x => x.Status == status)
                .OrderByDescending(t => t.QuantitySold)
                .Take(8)
                .ToListAsync();
        }

        public async Task<PaginationDTO<Template>> GetTemplateByNameAsync(string? name, int page = 1, int limit = 1,bool status = true)
        {
            var query = _context.Templates.AsQueryable();
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(x => x.Name.Contains(name) && x.Status == status);
            }

            int totalRows = await query.Where(x => x.Status == status).CountAsync(); 

            var items = await query.Skip((page - 1) * limit).Take(limit).ToListAsync();

            var paginations = new PaginationDTO<Template>()
            {
                limit = limit,
                Page = page,
                totalRows = totalRows,
                Items = items
            };
            return paginations;
        }

        public async Task<PaginationDTO<Template>> getAlltemplateAsync2(int page = 1, int limit = 1, bool status = true)
        {
            var query = _context.Templates.Include(s => s.TemplateImages).AsQueryable();
            
            query = query.Where(x => x.Status == status);
            int totalRows = await query.Where(x => x.Status == status).CountAsync();
            var items = await query.Skip((page - 1) * limit).Take(limit).ToListAsync();
            var paginations = new PaginationDTO<Template>()
            {
                limit = limit,
                Page = page,
                totalRows = totalRows,
                Items = items
            };
            return paginations;
        }
    }
}



using WebAPIGroup2.Models.POJO;

namespace WebAPIGroup2.Respository.Inteface
{
    public interface ITemplateRepo : IBaseRepository<Template, int>
    {
        public Task<List<Template>> GetAllTemplateAsync(string? filterOn = null, string? filterQuery = null, string? sortBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 1000);
        public Task<List<Template>> GetBestSellerTemplateAsync();
    }
}
    


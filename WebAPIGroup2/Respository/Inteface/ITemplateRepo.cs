
using WebAPIGroup2.Models.DTO;
using WebAPIGroup2.Models.POJO;

namespace WebAPIGroup2.Respository.Inteface
{
    public interface ITemplateRepo : IBaseRepository<Template, int>
    {
        public Task<List<Template>> GetAllTemplateAsync(string? filterOn = null, string? filterQuery = null, string? sortBy = null, bool isAscending = true, bool status = true, int pageNumber = 1, int pageSize = 1000);
        public Task<List<Template>> GetBestSellerTemplateAsync(bool status = true);
        public Task<PaginationDTO<Template>> GetTemplateByNameAsync(string? name, int page = 1,int  limit = 1, bool status = true);

        public Task<PaginationDTO<Template>> getAlltemplateAsync2(int page = 1, int limit = 1, bool status = true);
    }
}
    


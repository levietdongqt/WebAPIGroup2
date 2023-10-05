using WebAPIGroup2.Models;
using WebAPIGroup2.Models.POJO;
using WebAPIGroup2.Respository.Inteface;

namespace WebAPIGroup2.Respository.Implement;

public class CategoryTemplateRepo : GenericRepository<CategoryTemplate>, ICategoryTemplateRepo
{
    public CategoryTemplateRepo(MyImageContext context) : base(context)
    {
    }

    public async Task<CategoryTemplate?> GetByIDAsync(int id)
    {
        throw new NotImplementedException();
    }
}
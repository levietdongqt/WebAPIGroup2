using WebAPIGroup2.Models;
using WebAPIGroup2.Models.POJO;
using WebAPIGroup2.Respository.Inteface;

namespace WebAPIGroup2.Respository.Implement;

public class CategoryTemplateRepo : GenericRepository<CategoryTemplate>, ICategoryTemplateRepo
{
    private readonly GenericRepository<CategoryTemplate> _genericRepository;
    public CategoryTemplateRepo(MyImageContext context, GenericRepository<CategoryTemplate> genericRepository) : base(context)
    {
        _genericRepository = genericRepository;
    }

    public async Task<bool> InsertAsync(CategoryTemplate categoryTemplate)
    {
        return await _genericRepository.InsertAsync(categoryTemplate);
    }
    public async Task<CategoryTemplate?> GetByIDAsync(int id)
    {
        throw new NotImplementedException();
    }
}
using WebAPIGroup2.Models;
using WebAPIGroup2.Models.POJO;
using WebAPIGroup2.Respository.Inteface;

namespace WebAPIGroup2.Respository.Implement;

public class CollectionTemplateRepo : GenericRepository<CollectionTemplate>, ICollectionTemplateRepo
{
    public CollectionTemplateRepo(MyImageContext context) : base(context)
    {
    }

    public async Task<CollectionTemplate?> GetByIDAsync(int id)
    {
        throw new NotImplementedException();
    }
}
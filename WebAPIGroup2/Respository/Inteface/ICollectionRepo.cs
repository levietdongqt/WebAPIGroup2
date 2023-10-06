using WebAPIGroup2.Models.DTO;
using WebAPIGroup2.Models.POJO;

namespace WebAPIGroup2.Respository.Inteface;

public interface ICollectionRepo : IBaseRepository<Collection,int>
{
    Task<CollectionWithTemplateDTO?> GetCollectionWithTemplate(int id);
}
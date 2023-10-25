using WebAPIGroup2.Models.DTO;
using WebAPIGroup2.Models.POJO;

namespace WebAPIGroup2.Respository.Inteface;

public interface ICollectionRepo : IBaseRepository<Collection,int>
{
    Task<PaginationDTO<CollectionWithTemplateDTO?>> GetCollectionWithTemplate(int id, int page =1 , int limit = 1,bool status = true);

    Task<List<Collection>> getCollectionFeatures();
}
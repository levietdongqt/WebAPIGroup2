using WebAPIGroup2.Models.DTO;
using WebAPIGroup2.Models.POJO;

namespace WebAPIGroup2.Service.Inteface;

public interface ICollectionService
{
    Task<IEnumerable<CollectionDTO>?> GetAllasync();
    Task<CollectionDTO?> GetCollectionById(int id);
    Task<CollectionWithTemplateDTO> GetCollectionWithTemplate(int id);
    Task<bool> UpdateCollection(CollectionDTO collection);
    Task<bool> DeleteCollection(int id);
    Task<CollectionDTO> CreateCollection(CollectionDTO collection);
}
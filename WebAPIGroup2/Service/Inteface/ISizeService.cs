using WebAPIGroup2.Models.DTO;

namespace WebAPIGroup2.Service.Inteface;

public interface ISizeService
{
    Task<IEnumerable<SizeDTO>?> GetAll();
    Task<SizeDTO?> GetSizeById(int id);
    Task<bool> UpdateSize(SizeDTO size);
    Task<bool> DeleteSize(int id);
    Task<SizeDTO> CreateSize(SizeDTO size);
}
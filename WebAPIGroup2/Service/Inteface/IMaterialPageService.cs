using WebAPIGroup2.Models.DTO;
using WebAPIGroup2.Models.POJO;

namespace WebAPIGroup2.Service.Inteface
{
    public interface IMaterialPageService
    {
        Task<List<MaterialPageDTO>> GetAllAsync();

        Task<MaterialPageDTO> GetByIdAsync(int id);

        Task<MaterialPageDTO> CreateAsync(MaterialPageDTO materialPageDTO);

        Task<MaterialPageDTO> UpdateAsync(int id,MaterialPageDTO materialPageDTO);
    }
}

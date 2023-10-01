using System.Security.Claims;
using WebAPIGroup2.Models;
using WebAPIGroup2.Models.DTO;
using WebAPIGroup2.Models.POJO;

namespace WebAPIGroup2.Service.Inteface
{
    public interface IUserService
    {
        Task <UserDTO> CreateUser(UserDTO userDTO);   
        Task <bool> UpdateUser(UserDTO userDTO);
        Task<IEnumerable<UserDTO>?> GetAllAsync(string? search,string? st, int page, int pageSize); 
        Task<bool> ChangePassword(UserDTO userDTO, string oldPassword);

        Task<UserDTO> GetUserByIDAsync(int id);

        Task<UserDTO> UpdateConfirmEmailAsync(UserDTO userDTO);
    }
}

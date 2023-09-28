using System.Security.Claims;
using WebAPIGroup2.Models;
using WebAPIGroup2.Models.DTO;
using WebAPIGroup2.Models.POJO;

namespace WebAPIGroup2.Service.Inteface
{
    public interface IUserService
    {
        Task<IEnumerable<UserDTO>?> GetAllAsync(string? search);
        Task <UserDTO> CreateUser(UserDTO userDTO);   
        Task <bool> UpdateUser(UserDTO userDTO);
        Task<bool> ChangePassword(UserDTO userDTO, string oldPassword);

        Task<UserDTO> GetUserByIDAsync(int id);

        Task<UserDTO> UpdateConfirmEmailAsync(UserDTO userDTO);
    }
}

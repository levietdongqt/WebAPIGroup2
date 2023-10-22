using System.Security.Claims;
using WebAPIGroup2.Models;
using WebAPIGroup2.Models.DTO;
using WebAPIGroup2.Models.POJO;

namespace WebAPIGroup2.Service.Inteface
{
    public interface IUserService
    {
        Task <UserDTO> CreateUser(UserDTO userDTO);   
        Task<UserDTO> UpdateUser(AddUserDTO addUserDTO);
        Task<IEnumerable<UserDTO>?> GetAllAsync(string? search,string? st, int page, int pageSize);
        Task<UserDTO> ChangePassword(AddUserDTO addUserDTO );
        Task<UserDTO> PasswordRecovery(AddUserDTO addUserDTO );

        Task<UserDTO> GetUserByIDAsync(int id);
        Task<UserDTO> GetUserByEmailAsync(string email);

        Task<UserDTO> UpdateConfirmEmailAsync(UserDTO userDTO);

        Task<List<DeliveryInfoDTO>> GetDeliveryInfoByUserIDAsync(int userId);

        Task<DeliveryInfoDTO> CreateDeliveryInfoOfUser(int userId,DeliveryInfoDTO deliveryInfoDTO);
        Task<UserDTO?> GetOrderByUserId(int id);
    }
}

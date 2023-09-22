using System.Security.Claims;
using WebAPIGroup2.Models.DTO;
using WebAPIGroup2.Models.POJO;

namespace WebAPIGroup2.Service.Inteface
{
    public interface ILoginService
    {
        public string GenerateToken(UserDTO user);
        public Task<UserDTO> checkUser(string userName, string password);
    }
}
 
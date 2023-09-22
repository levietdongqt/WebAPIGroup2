using WebAPIGroup2.Models;
using WebAPIGroup2.Models.DTO;
using WebAPIGroup2.Models.POJO;

namespace WebAPIGroup2.Respository.Inteface
{
    public interface IUserRepo :  ISharedRepository<User, string>
    {
        public Task<User> GetUser(UserDTO userDTO);
    }
}

using WebAPIGroup2.Models;
using WebAPIGroup2.Models.POJO;

namespace WebAPIGroup2.Respository.Inteface
{
    public interface IUserRepo :  IBaseRepository<User, int>
    {
        public Task<User> GetUser(LoginRequestDTO loginRequest);
        public Task<User> GetUserByEmail(string? userEmail);

    }
}

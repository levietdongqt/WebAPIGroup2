using WebAPIGroup2.Models;
using WebAPIGroup2.Models.POJO;

namespace WebAPIGroup2.Respository.Inteface
{
    public interface IUserRepo :  ISharedRepository<User, int>
    {
        public Task<User> GetUser(LoginRequestDTO loginRequest);
        Task<User> GetUserByEmail(string? userEmail);
    }
}

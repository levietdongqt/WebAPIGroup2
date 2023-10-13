using Microsoft.EntityFrameworkCore;
using WebAPIGroup2.Models;
using WebAPIGroup2.Models.POJO;
using WebAPIGroup2.Respository.Inteface;

namespace WebAPIGroup2.Respository.Implement
{
    public class UserRepo : GenericRepository<User>, IUserRepo
    {
        public UserRepo(MyImageContext context) : base(context)
        {
        }

        public async Task<User?> GetByIDAsync(int id)
        {
            var user = await _context.Users.Include(d=>d.DeliveryInfos).ThenInclude(c=>c.ContentEmails).FirstOrDefaultAsync(user => user.Id == id);
            if(user == null)
            {
                return null;
            }
            return user;
        }
        public async Task<User?>  GetUser(LoginRequestDTO loginRequest)
        {
            string role = loginRequest.isClient ? "user" : "admin";
            var user = await _context.Users.FirstOrDefaultAsync(t => t.Email.Equals(loginRequest.email) && t.Password
            .Equals(loginRequest.password) && t.Role.Equals(role));
            return user;
        }
        public async Task<User> GetUserByEmail(string? userEmail)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email.Equals(userEmail));
        }
    }
}

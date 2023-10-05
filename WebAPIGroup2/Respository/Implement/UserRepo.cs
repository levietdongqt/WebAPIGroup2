using Microsoft.EntityFrameworkCore;
using WebAPIGroup2.Models;
using WebAPIGroup2.Models.DTO;
using WebAPIGroup2.Models.POJO;
using WebAPIGroup2.Respository.Inteface;

namespace WebAPIGroup2.Respository.Implement
{
    public class UserRepo : GenericRepository<User>, IUserRepo
    {
        private readonly MyImageContext _context;
        private readonly GenericRepository<User> _repository;
        public UserRepo(MyImageContext context,GenericRepository<User> genericRepository) : base(context)
        {
            _context = context;
            _repository = genericRepository;
        }

        public async Task<bool> DeleteAsync(User entity)
        {
           return await _repository.DeleteAsync(entity);
        }
        public async Task<bool> DeleteAllAsync(List<User> list)
        {
            return await _repository.DeleteAllAsync(list);  
        }

        public async Task<IEnumerable<User>?> GetAllAsync()
        {
            return await _repository.GetAllAsync();
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

        public async Task<bool> InsertAsync(User entity)
        {
            return await _repository.InsertAsync(entity);
        }

        public async Task<bool> InsertAllAsync(List<User> list)
        {
            return await _repository.InsertAllAsync(list);
        }

        public async Task<bool> UpdateAsync(User entity)
        {
           return await _repository.UpdateAsync(entity);
        }

        public async Task<bool> UpdateAllAsync(List<User> list)
        {
            return await _repository.UpdateAllAsync(list);
        }

        public async Task<User?>  GetUser(LoginRequestDTO loginRequest)
        {
            var user = await _context.Users.FirstOrDefaultAsync(t => t.Email.Equals(loginRequest.email) && t.Password
            .Equals(loginRequest.password));
            return user;
        }
        public async Task<User> GetUserByEmail(string? userEmail)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email.Equals(userEmail));
        }
    }
}

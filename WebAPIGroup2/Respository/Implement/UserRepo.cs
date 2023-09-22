using Microsoft.EntityFrameworkCore;
using WebAPIGroup2.Models;
using WebAPIGroup2.Models.DTO;
using WebAPIGroup2.Models.POJO;
using WebAPIGroup2.Respository.Inteface;

namespace WebAPIGroup2.Respository.Implement
{
    public class UserRepo : GenericRepository<User>, IUserRepo
    {
        private readonly Dbsem3G2Context _context;
        private readonly GenericRepository<User> _repository;
        public UserRepo(Dbsem3G2Context context,GenericRepository<User> genericRepository) : base(context)
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

        public Task<User?> GetByIDAsync(string id)
        {
            throw new NotImplementedException();
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

        public async Task<User?>  GetUser(UserDTO userDTO)
        {
            var user = await _context.Users.FirstOrDefaultAsync(t => t.UserName.Equals(userDTO.UserName) && t.Password
            .Equals(userDTO.Password));
            return user;
        }
    }
}

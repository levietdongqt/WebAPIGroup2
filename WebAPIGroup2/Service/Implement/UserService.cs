using Microsoft.EntityFrameworkCore;
using WebAPIGroup2.Models;
using WebAPIGroup2.Models.DTO;
using WebAPIGroup2.Models.POJO;
using WebAPIGroup2.Respository.Implement;
using WebAPIGroup2.Respository.Inteface;
using WebAPIGroup2.Service.Inteface;
using System.Text.Json;
using AutoMapper;

namespace WebAPIGroup2.Service.Implement
{
    public class UserService : IUserService
    {
        private readonly IUserRepo _useRepo;
        private readonly Dbsem3G2Context _context;
        private readonly IMapper _mapper;

        public UserService(Dbsem3G2Context context, IUserRepo userRepo, IMapper mapper)
        {
            _context = context;
            _useRepo = userRepo;
            _mapper = mapper;
        }

        public async Task<bool> ChangePassword(UserDTO userDTO, string oldPassword)
        {
            var existingUser = await _context.Users.SingleOrDefaultAsync(u => u.Id == userDTO.Id && u.Password == oldPassword);

            if (existingUser == null)
            {
                return false; 
            }

            existingUser.Password = userDTO.Password;
            var update = await _useRepo.UpdateAsync(existingUser);

            return update;
        }




        public async Task<UserDTO> CreateUser(UserDTO userDTO)
        {
            var user = _mapper.Map<User>(userDTO);
            user.Status = UserStatus.Pending;
            user.Password = userDTO.Password;
            var success = await _useRepo.InsertAsync(user);
            if (success)
            {
                var createdUserDTO = _mapper.Map<UserDTO>(user);
                return createdUserDTO;
            }
            return null;

        }





        public async Task<IEnumerable<UserDTO>?> GetAllAsync(string? search)
        {
            List<UserDTO> list = new List<UserDTO>();
            var users = await _useRepo.GetAllAsync();
            if (users != null)
            {

                if (!string.IsNullOrEmpty(search))
                {
                    users = users.Where(sd => sd.Email.Contains(search) || sd.PhoneNumber.Contains(search));
                }
          
                list = _mapper.Map<List<UserDTO>>(users);
            }

            return list;

        }

        public async Task<UserDTO> GetUserByIDAsync(int id)
        {
            var user = await _useRepo.GetByIDAsync(id);
            var userDTO = _mapper.Map<UserDTO>(user);
            return userDTO;
        }

        public async Task<UserDTO> UpdateConfirmEmailAsync(UserDTO userDTO)
        {
            var existingUser = await _context.Users.SingleOrDefaultAsync(u => u.Id == userDTO.Id);
            if (existingUser == null)
            {
                return null;
            }
            existingUser.Status = UserStatus.Enabled;
            existingUser.EmailConfirmed = true;
            var updated = await _useRepo.UpdateAsync(existingUser);
            if (!updated)
            {
                return null;
            }
            return _mapper.Map<UserDTO>(existingUser);


        }

        public async Task<bool> UpdateUser(UserDTO userDTO)
        {
            var existingUser = await _context.Users.SingleOrDefaultAsync(u => u.Id == userDTO.Id);

            if (existingUser != null)
            {
                existingUser.Address = userDTO.Address;
                existingUser.PhoneNumber = userDTO.PhoneNumber;
                existingUser.Role = userDTO.Role;
                existingUser.DateOfBirth = userDTO.DateOfBirth;
                existingUser.Status = userDTO.Status;

                var update = await _useRepo.UpdateAsync(existingUser);
                return update;
            }
            else
            {
                return false;
            }
        }

    }







}

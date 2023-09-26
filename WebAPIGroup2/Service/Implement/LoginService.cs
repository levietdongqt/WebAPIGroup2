using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using WebAPIGroup2.Models;
using WebAPIGroup2.Models.DTO;
using WebAPIGroup2.Models.POJO;
using WebAPIGroup2.Respository.Inteface;
using WebAPIGroup2.Service.Inteface;

namespace WebAPIGroup2.Service.Implement
{
    public class LoginService : ILoginService
    {
        private readonly IConfiguration _config;
        private readonly IUserRepo _userRepo;
        private readonly IMapper _mapper;
        public LoginService(IConfiguration config, IUserRepo userRepo, IMapper mapper)
        {
            _config = config;
            _userRepo = userRepo;
            _mapper = mapper;
        }

        public async Task<UserDTO> checkLoggedByGoogle(string? userEmail)
        {
            User user = await _userRepo.GetUserByEmail(userEmail);
            if (user != null)
            {
                return _mapper.Map<UserDTO>(user);
            }
            else
            {
                return null;
            }
        }

        public async Task<UserDTO> checkUser(LoginRequestDTO loginRequest)
        {
            //var passwordHash = BCrypt.Net.BCrypt.HashPassword(loginRequest.password);
            //loginRequest.password = passwordHash;
            var user = await _userRepo.GetUser(loginRequest);
            return _mapper.Map<UserDTO>(user);

        }

        public async Task<UserDTO> CreateUser(string? fullName, string? userEmail)
        {
            User user = new User();
            user.FullName = fullName;
            user.Email = userEmail;
            user.UserName = userEmail;
            user.EmailConfirmed = true;
            user.Status = UserStatus.Enabled;
            await _userRepo.InsertAsync(user);
            var userSaved = await _userRepo.GetUserByEmail(userEmail);
            return _mapper.Map<UserDTO>(userSaved);
        }

        public string GenerateToken(UserDTO user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
                        {
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(ClaimTypes.Role,user.Role)
            };
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: credentials);


            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
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
        public LoginService(IConfiguration config,IUserRepo userRepo,IMapper mapper)
        {
            _config = config;
            _userRepo = userRepo;
            _mapper = mapper;
        }

        public async Task<UserDTO> checkUser(string userName, string password)
        {
            var userDTO = new UserDTO();
            userDTO.UserName = userName;
            userDTO.Password = password;
            var user = await _userRepo.GetUser(userDTO);
            return  _mapper.Map<UserDTO>(user);
            
        }

        public string GenerateToken(UserDTO user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
                        {
                new Claim(ClaimTypes.NameIdentifier,user.UserName),
                new Claim(ClaimTypes.Role,"normal")
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

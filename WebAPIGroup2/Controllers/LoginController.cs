using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using WebAPIGroup2.Models;
using WebAPIGroup2.Models.DTO;
using WebAPIGroup2.Models.POJO;
using WebAPIGroup2.Service.Inteface;

namespace WebAPIGroup2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _loginService;
        public LoginController( ILoginService loginService)
        {
            _loginService = loginService;
        }
        [HttpGet("")]
        public async Task<ResponseDTO<UserDTO>> Login(string userName, string password)
        {
            var user = await _loginService.checkUser(userName,password);
            ResponseDTO<UserDTO> response;
            if (user == null)
            {
                response = new ResponseDTO<UserDTO>(HttpStatusCode.BadRequest, "Fail", null , null);
                return response;
            }
            var token = _loginService.GenerateToken(user);
            response = new ResponseDTO<UserDTO>(HttpStatusCode.OK,"Success", token, user);
            return response;
        }
    }
}

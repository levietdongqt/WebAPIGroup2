using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
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
        [HttpGet("login-google")]
        public IActionResult LoginWithGoogle()
        {
            var authenticationProperties = new AuthenticationProperties
            {
                RedirectUri = Url.Action(nameof(HandleGoogleResponse))
            };

            return Challenge(authenticationProperties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("handle-google-response")]
        public async Task<ResponseDTO<UserDTO>> HandleGoogleResponse()
        {
            ResponseDTO<UserDTO> response;
            // Xử lý phản hồi từ Google và lấy thông tin người dùng
            var authenticateResult = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

            // Lấy thông tin người dùng từ Principal
            var userPrincipal = authenticateResult.Principal;

            // Lấy tên người dùng
            var userName = userPrincipal.FindFirst(ClaimTypes.Name)?.Value;

            // Lấy địa chỉ email
            var userEmail = userPrincipal.FindFirst(ClaimTypes.Email)?.Value;

            await HttpContext.SignOutAsync("Cookies");

            var userLogged =await _loginService.checkLoggedByGoogle(userEmail);
            if (userLogged == null)
            {
               userLogged =  await _loginService.CreateUser(userName, userEmail);
            }
            var token = _loginService.GenerateToken(userLogged);
            response = new ResponseDTO<UserDTO>(HttpStatusCode.OK, "Success", token, userLogged);
            return response;
        }
    }
}

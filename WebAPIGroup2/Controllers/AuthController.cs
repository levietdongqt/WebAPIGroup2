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
using Microsoft.AspNetCore.Http.HttpResults;

namespace WebAPIGroup2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILoginService _loginService;
        public AuthController( ILoginService loginService)
        {
            _loginService = loginService;
        }
        [HttpPost("login")]
        public async Task<JsonResult> Login([FromBody] LoginRequestDTO loginRequest)
        {
            var user = await _loginService.checkUser(loginRequest);
            ResponseDTO<UserDTO> response;
            if (user == null)
            {
                response = new ResponseDTO<UserDTO>(HttpStatusCode.BadRequest, "Fail", null , null);
                return new JsonResult(response);
            }
            var token = _loginService.GenerateToken(user);
            response = new ResponseDTO<UserDTO>(HttpStatusCode.OK,"Success", token, user);
            //Response.Cookies.Append("access_token", token, new CookieOptions
            //{
            //    HttpOnly = true,
            //    SameSite = SameSiteMode.None, // Thay đổi theo yêu cầu của bạn
            //    Secure = false // Yêu cầu kết nối bảo mật (HTTPS)
            //    //               // Nếu bạn muốn đặt domain hoặc path, bạn có thể thực hiện ở đây
            //});
            return new JsonResult(response);
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
            var fullName = userPrincipal.FindFirst(ClaimTypes.Name)?.Value;

            // Lấy địa chỉ email
            var userEmail = userPrincipal.FindFirst(ClaimTypes.Email)?.Value;

            await HttpContext.SignOutAsync("Cookies");

            var userLogged =await _loginService.checkLoggedByGoogle(userEmail);
            if (userLogged == null)
            {
               userLogged =  await _loginService.CreateUser(fullName, userEmail);
            }
            var token = _loginService.GenerateToken(userLogged);
            response = new ResponseDTO<UserDTO>(HttpStatusCode.OK, "Success", token, userLogged);
            return response;
        }
        [HttpGet("AccessDenied")]
        public async Task<JsonResult> AccessDenied()
        {
            ResponseDTO<String> response = new ResponseDTO<string>(HttpStatusCode.Forbidden, "AccessDenied",null,null);

            return new JsonResult(response);    
        }
    }
}

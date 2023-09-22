using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;
using WebAPIGroup2.Models.POJO;

namespace WebAPIGroup2.Controllers.UserModule
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet]
        [Route("admins")]
        [Authorize(Roles = "normal")]
        public IActionResult AdminEndPoint()
        {
           // var currentUser = GetCurrentUser();
            return Ok($"Hi you are ");
        }
        //private User GetCurrentUser()
        //{
        //    var a1 = HttpContext.User;
        //    var identity = HttpContext.User.Identity as ClaimsIdentity;
        //    if (identity != null)
        //    {
        //        var userClaims = identity.Claims;
        //        return new User
        //        {
        //            Username = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value,
        //            Role = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value
        //        };
        //    }
        //    return null;
        //}
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
        public async Task<IActionResult> HandleGoogleResponse()
        {

            // Xử lý phản hồi từ Google và lấy thông tin người dùng
            var authenticateResult = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

            // Lấy thông tin người dùng từ Principal
            var userPrincipal = authenticateResult.Principal;

            // Lấy tên người dùng
            var userName = userPrincipal.FindFirst(ClaimTypes.Name)?.Value;

            // Lấy địa chỉ email
            var userEmail = userPrincipal.FindFirst(ClaimTypes.Email)?.Value;

            // Sử dụng userName và userEmail theo nhu cầu của bạn


            // Thực hiện xử lý và đăng nhập người dùng vào ứng dụng của bạn

            // Redirect hoặc trả về thông tin người dùng
            await HttpContext.SignOutAsync("Cookies");
            return Ok(new { userName = userName, userEmail = userEmail });
        }
    }
    
}

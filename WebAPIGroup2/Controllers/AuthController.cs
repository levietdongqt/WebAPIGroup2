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
using Microsoft.AspNetCore.Identity;
using Azure.Core;

namespace WebAPIGroup2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private class GoogleResponse
        {
            public string Email { get; set; }
            public string Name { get; set; }
            public string Picture { get; set; }
        }
        public class OAuthRequest
        {
            public string access_token { get; set; }
        }
        private readonly ILoginService _loginService;
        public AuthController(ILoginService loginService)
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
                response = new ResponseDTO<UserDTO>(HttpStatusCode.BadRequest, "Login Fail", null, null);
                return new JsonResult(response);
            }
            var token = _loginService.GenerateToken(user);
            response = new ResponseDTO<UserDTO>(HttpStatusCode.OK, "Success", token, user);
            //Response.Cookies.Append("access_token", token, new CookieOptions
            //{
            //    HttpOnly = true,
            //    SameSite = SameSiteMode.None, // Thay đổi theo yêu cầu của bạn
            //    Secure = false // Yêu cầu kết nối bảo mật (HTTPS)
            //    //               // Nếu bạn muốn đặt domain hoặc path, bạn có thể thực hiện ở đây
            //});
            return new JsonResult(response);
        }
        [HttpPost("handle-google-response")]
        public async Task<ResponseDTO<UserDTO>> HandleGoogleResponse([FromBody] OAuthRequest oAuthRequest)
        {
            ResponseDTO<UserDTO> response = null;

            var httpClient = new HttpClient();
            string userInfoUrl = "https://www.googleapis.com/oauth2/v1/userinfo";


            // Gửi yêu cầu kiểm tra access token
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {oAuthRequest.access_token}");
            var responseGoogle = await httpClient.GetAsync(userInfoUrl);

            if (responseGoogle.IsSuccessStatusCode)
            {
                var userInfoJson = await responseGoogle.Content.ReadAsStringAsync();
                GoogleResponse userInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<GoogleResponse>(userInfoJson);

                var userLogged = await _loginService.checkLoggedByGoogle(userInfo.Email);
                if (userLogged == null)
                {
                    userLogged = await _loginService.CreateUser(userInfo.Name, userInfo.Email);
                }
                var token = _loginService.GenerateToken(userLogged);
                response = new ResponseDTO<UserDTO>(HttpStatusCode.OK, "Success", token, userLogged);
            }
            return response;
        }
        [HttpGet("AccessDenied")]
        public async Task<JsonResult> AccessDenied()
        {
            ResponseDTO<String> response = new ResponseDTO<string>(HttpStatusCode.Forbidden, "AccessDenied", null, null);

            return new JsonResult(response);
        }
    }
}

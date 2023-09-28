using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Net;
using System.Security.Claims;
using WebAPIGroup2.Models;
using WebAPIGroup2.Models.DTO;
using WebAPIGroup2.Models.POJO;
using WebAPIGroup2.Respository.Inteface;
using WebAPIGroup2.Service.Inteface;

namespace WebAPIGroup2.Controllers.UserModule
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly Dbsem3G2Context _context;
        private readonly IUserService _userService;

        public UserController(Dbsem3G2Context context , IUserService userService) 
        { 
            _context = context;
            _userService = userService;
        }

        [HttpGet]
        [Route("admins")]
        [Authorize(Roles ="admin")]
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

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(string? search , string? st, int page, int pageSize)
        {
            var users = await _userService.GetAllAsync(search,st, page, pageSize);

            if (users != null)
            {
                // Chúng ta sử dụng danh sách người dùng (users) trong ResponseDTO
                var response = new ResponseDTO<IEnumerable<UserDTO>>(
                    HttpStatusCode.OK,
                    "Getall ok",
                    null, 
                    users 
                );

                return Ok(response);
            }
            else
            {
                return new NotFoundObjectResult(new ResponseDTO<IEnumerable<UserDTO>>(
                    HttpStatusCode.NotFound,
                    "No users found",
                    null, // Token (nếu cần)
                    null  // Không có dữ liệu
                ));
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(UserDTO userDTO)
        {
            bool success = await _userService.CreateUser(userDTO);

            if (success)
            {
                return Ok(new ResponseDTO<UserDTO>(HttpStatusCode.Created, "Create ok", null, userDTO));
            }
            else
            {
                return BadRequest(new ResponseDTO<string>(HttpStatusCode.BadRequest, "Failed to create user", null, "Failed"));
            }
        }

        [HttpPut("Edit")]
        public async Task<IActionResult> Update(UserDTO userDTO)
        {
            bool success = await _userService.UpdateUser(userDTO);
            if (success)
            {
                return Ok(new ResponseDTO<UserDTO>(HttpStatusCode.OK, "Edit ok", null, userDTO));
            }
            else
            {
                return BadRequest(new ResponseDTO<string>(HttpStatusCode.BadRequest, "Failed to create user", null, "Failed"));
            }
        }

        [HttpPut("ChangePass")]
        public async Task<IActionResult> ChangePassWord([FromBody] UserDTO userDTO, string oldPassword)
        {
            bool success = await _userService.ChangePassword(userDTO, oldPassword);
            if (success)
            {
                return Ok(new ResponseDTO<UserDTO>(HttpStatusCode.OK, "Changepass ok", null, userDTO));
            }
            else
            {
                return BadRequest(new ResponseDTO<string>(HttpStatusCode.BadRequest, "Failed to create user", null, "Failed"));
            }
        }    



    }

}

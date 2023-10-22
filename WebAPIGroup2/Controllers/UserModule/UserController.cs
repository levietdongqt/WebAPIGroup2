using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Net;
using System.Text;
using System.Text.Encodings.Web;
using WebAPIGroup2.Models;
using WebAPIGroup2.Models.DTO;
using WebAPIGroup2.Models.POJO;
using WebAPIGroup2.Service.Inteface;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace WebAPIGroup2.Controllers.UserModule
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly MyImageContext _context;
        private readonly IUserService _userService;
        private readonly IUtilService _utilService;
        private readonly ILoginService _loginService;

        public UserController(MyImageContext context, IUserService userService, IUtilService utilService, ILoginService loginService)
        {
            _context = context;
            _userService = userService;
            _utilService = utilService;
            _loginService = loginService;
        }

        [HttpGet]
        [Route("admins")]
        [Authorize(Roles = "admin")]
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
        public async Task<IActionResult> GetAll(string? search, string? st, int page, int pageSize)
        {
            var users = await _userService.GetAllAsync(search, st, page, pageSize);

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

        [HttpGet]
        [Route("Orders/{id:int}")]
        public async Task<JsonResult> GetOrderByUserId(int id)
        {
            var order = await _userService.GetOrderByUserId(id);
            var response = new ResponseDTO<UserDTO?>(HttpStatusCode.OK, "Get ok", null, order);
            return new JsonResult(response);
        }
        

        [HttpPost("Create")]
        public async Task<IActionResult> Create(UserDTO userDTO)
        {
            try
            {
                var email = userDTO.Email;

                // Kiểm tra xem email đã được sử dụng chưa bằng cách gọi phương thức GetUserByEmailAsync
                var existingUserDTO = await _userService.GetUserByEmailAsync(email);

                if (existingUserDTO != null)
                {
                    return BadRequest(new ResponseDTO<string>(HttpStatusCode.BadRequest, "Email is already in use", null, "Failed"));
                }

                var createdUserDTO = await _userService.CreateUser(userDTO);

                if (createdUserDTO != null)
                {
                    var userId = createdUserDTO.Id;
                    var code = _loginService.GenerateToken(createdUserDTO);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                    var callbackUrl = Url.Action(nameof(ConfirmEmail), "User", new { userId = userId, code = code }, Request.Scheme);
                    var callbackUrl2 = $"http://localhost:3000/login/confirm?userId={userId}&code={code}";
                    var mailContent = new MailContent(userDTO.Email, "Confirm your email", $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl2)}'>clicking here</a>.", "Confirmation");

                    var mailContented = await _utilService.SendEmailAsync(mailContent);
                    if (mailContented == null)
                    {
                        return BadRequest(new ResponseDTO<string>(HttpStatusCode.BadRequest, "Failed to Send Mail To User", null, "Failed"));
                    }
                    return Ok(new ResponseDTO<UserDTO>(HttpStatusCode.Created, "Create Ok", null, userDTO));
                }
                else
                {
                    return BadRequest(new ResponseDTO<string>(HttpStatusCode.BadRequest, "Failed to create user", null, "Failed"));
                }
            }
            catch (Exception e)
            {

                return BadRequest(new ResponseDTO<string>(HttpStatusCode.BadRequest, e.Message, null, "Failed"));

            }

        }

        [HttpPut("Edit")]
        public async Task<IActionResult> Update([FromForm] AddUserDTO addUserDTO)
        {
            try
            {
                if (addUserDTO.formFile != null)
                {
                    _utilService.ValiadateFileUpload(addUserDTO.formFile);
                }

                var userDTO = await _userService.UpdateUser(addUserDTO);
                if (userDTO != null)
                {
                    return Ok(new ResponseDTO<UserDTO>(HttpStatusCode.OK, "Edit ok", null, userDTO));
                }
                else
                {
                    return BadRequest(new ResponseDTO<string>(HttpStatusCode.BadRequest, "Failed to update user", null, "Failed"));
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Error updating information: " + e.Message);
                return BadRequest(new ResponseDTO<string>(HttpStatusCode.BadRequest, e.Message, null, "Failed"));
            }
        }

        private string GenerateRandomCode()
        {
            const string characters = "0123456789";
            var random = new Random();
            var result = new string(Enumerable.Repeat(characters, 6)
                .Select(s => s[random.Next(s.Length)]).ToArray());
            return result;
        }



        [HttpPost("SendMailPR")]
        public async Task<IActionResult> SendMailPassReco(string email)
        {
            try
            {

                var existingUserDTO = await _userService.GetUserByEmailAsync(email);

                if (existingUserDTO == null)
                {
                    return BadRequest(new ResponseDTO<string>(HttpStatusCode.BadRequest, "Email does not exist", null, "Failed"));
                }

                var confirmationCode = GenerateRandomCode();

                var mailContent = new MailContent(email, "Confirm your email", $"Your confirmation code is: {confirmationCode}", "Confirmation");

                var mailContented = await _utilService.SendEmailAsync(mailContent);

                if (mailContented == null)
                {
                    return BadRequest(new ResponseDTO<string>(HttpStatusCode.BadRequest, "Failed to Send Mail To User", null, "Failed"));
                }


                return Ok(new ResponseDTO<string>(HttpStatusCode.OK, "Confirmation successful", null, "Success"));
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Error sending email or processing request: " + e.Message);
                return BadRequest(new ResponseDTO<string>(HttpStatusCode.BadRequest, e.Message, null, "Failed"));
            }
        }






        [HttpPut("PassReco")]
        public async Task<IActionResult> PasswordRecovery( AddUserDTO addUserDTO)
        {
            try
            {
                var userDTO = await _userService.PasswordRecovery(addUserDTO);
                if (userDTO != null)
                {
                    return Ok(new ResponseDTO<UserDTO>(HttpStatusCode.OK, "PasswordRecovery ok", null, userDTO));
                }
                else
                {
                    return BadRequest(new ResponseDTO<string>(HttpStatusCode.BadRequest, "Failed to Password Recovery", null, "Failed"));
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Error updating information: " + e.Message);
                return BadRequest(new ResponseDTO<string>(HttpStatusCode.BadRequest, e.Message, null, "Failed"));
            }
        }

       

        [HttpPut("ChangePass")]
        public async Task<IActionResult> ChangePassWord([FromBody] AddUserDTO addUserDTO)
        {
            var userDTO = await _userService.ChangePassword(addUserDTO);
            if (userDTO != null)
            {
                return Ok(new ResponseDTO<UserDTO>(HttpStatusCode.OK, "Changepass ok", null, userDTO));
            }
            else
            {
                return BadRequest(new ResponseDTO<string>(HttpStatusCode.BadRequest, "Failed to changepassword", null, "Failed"));
            }
        }
        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(int userId, string code)
        {
            if (code == null)
            {
                return BadRequest(new ResponseDTO<string>(HttpStatusCode.BadRequest, "Failed to Confirm mail", null, "Failed"));
            }
            var userDTO = await _userService.GetUserByIDAsync(userId);
            if (userDTO == null)
            {
                return NotFound(new ResponseDTO<string>(HttpStatusCode.NotFound, "Failed to Confirm mail", null, "Failed"));
            }
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _utilService.ValidateCodeAsync(code, userDTO);

            if (result == null)
            {
                return BadRequest(new ResponseDTO<string>(HttpStatusCode.BadRequest, "Failed to Confirm mail", null, "Failed"));
            }
            var updatedUser = await _userService.UpdateConfirmEmailAsync(userDTO);
            if (updatedUser == null)
            {
                return BadRequest(new ResponseDTO<string>(HttpStatusCode.BadRequest, "Failed in update user", null, "Failed"));
            }
            var response = new ResponseDTO<UserDTO>(HttpStatusCode.OK, "Success", code, updatedUser);
            return Ok(response);


        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetByID([FromRoute] int id)
        {
            var userDTO = await _userService.GetUserByIDAsync(id);
            var response = new ResponseDTO<UserDTO>(HttpStatusCode.OK, "Success", null, userDTO);
            return Ok(response);
        }

        [HttpGet]
        [Route("get-by-email/{email}")]
        public async Task<IActionResult> GetByEmail([FromBody] string email)
        {
            var userDTO = await _userService.GetUserByEmailAsync(email);

            if (userDTO == null)
            {
                var errorResponse = new ResponseDTO<string>(HttpStatusCode.NotFound, "Email not found", "The specified email address does not exist in the system.", null);
                return NotFound(errorResponse);
            }

            var response = new ResponseDTO<UserDTO>(HttpStatusCode.OK, "Success", null, userDTO);
            return Ok(response);
        }




        [HttpGet]
        [Route("{userId:int}/deliveryInfos")]
        public async Task<JsonResult> GetDeliveryInfoByUserId([FromRoute] int userId)
        {
            var deliveryInfoDTOs = await _userService.GetDeliveryInfoByUserIDAsync(userId);
            if (deliveryInfoDTOs == null)
            {
                return new JsonResult(new ResponseDTO<DeliveryInfoDTO>(HttpStatusCode.NotFound, "User dont exist", null, null));
            }
            var response = new ResponseDTO<List<DeliveryInfoDTO>>(HttpStatusCode.OK, "Get All Successfully", null, deliveryInfoDTOs);
            return new JsonResult(response);
        }

        [HttpPost]
        [Route("{userId:int}/deliveryInfos")]
        public async Task<JsonResult> AddDeliveryInfoByUser([FromRoute] int userId, [FromBody] DeliveryInfoDTO deliveryInfoDTO)
        {
            var deliveryDTO = await _userService.CreateDeliveryInfoOfUser(userId, deliveryInfoDTO);
            if (deliveryDTO == null)
            {
                return new JsonResult(new ResponseDTO<DeliveryInfoDTO>(HttpStatusCode.NotFound, "User dont exist", null, null));
            }
            var response = new ResponseDTO<DeliveryInfoDTO>(HttpStatusCode.Created, "Created Successfully", null, deliveryDTO);
            return new JsonResult(response);
        }
    }




}

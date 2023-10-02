using MailKit.Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using WebAPIGroup2.Models;
using WebAPIGroup2.Models.DTO;
using WebAPIGroup2.Service.Inteface;

namespace TestEmail.Services
{
    public class UtilService : IUtilService
    {
        private readonly MailSetting mailSettings;

        private readonly ILogger<UtilService> logger;

        private readonly IConfiguration _configuration;

        private readonly IWebHostEnvironment _webHostEnvironment;


        // mailSetting được Inject qua dịch vụ hệ thống
        // Có inject Logger để xuất log
        public UtilService(IOptions<MailSetting> _mailSettings, ILogger<UtilService> _logger, IConfiguration configuration, IWebHostEnvironment _webHostEnvironment)
        {
            mailSettings = _mailSettings.Value;
            logger = _logger;
            logger.LogInformation("Create SendMailService");
            _configuration = configuration;
            this._webHostEnvironment = _webHostEnvironment;
        }

        // Gửi email, theo nội dung trong mailContent

        public async Task<MailContent> SendEmailAsync(MailContent mailContent)
        {
            var message = new MimeMessage();
            message.Sender = new MailboxAddress(mailSettings.DisplayName, mailSettings.Mail);
            message.From.Add(new MailboxAddress(mailSettings.DisplayName, mailSettings.Mail));
            message.To.Add(MailboxAddress.Parse(mailContent.Email));
            message.Subject = mailContent.Subject;


            var builder = new BodyBuilder();
            builder.HtmlBody = mailContent.htmlhtmlMessage;
            message.Body = builder.ToMessageBody();

            // dùng SmtpClient của MailKit
            using var smtp = new MailKit.Net.Smtp.SmtpClient();

            try
            {
                smtp.Connect(mailSettings.Host, mailSettings.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(mailSettings.Mail, mailSettings.Password);
                await smtp.SendAsync(message);
            }
            catch (Exception ex)
            {
                // Gửi mail thất bại, nội dung email sẽ lưu vào thư mục mailssave
                System.IO.Directory.CreateDirectory("mailssave");
                var emailsavefile = string.Format(@"mailssave/{0}.eml", Guid.NewGuid());
                await message.WriteToAsync(emailsavefile);

                logger.LogInformation("Send mail fail, saved - " + emailsavefile);
                logger.LogError(ex.Message);
                return null;
            }

            smtp.Disconnect(true);

            logger.LogInformation("Send mail to "+ mailContent.Email );
            return mailContent;
        }

        //Dung de upload file
        public async Task<string> Upload(IFormFile formFile)
        {
            var fileName = formFile.FileName;

            //Upload
            var localFilePath = Path.Combine(_webHostEnvironment.ContentRootPath, "Image", fileName);

            var stream = new FileStream(localFilePath, FileMode.Create);

            await formFile.CopyToAsync(stream);

            //Set URL Static

            var urlFilePath = $"/Image/{fileName}";

            return urlFilePath;
        }

        //DUng de upload nhieu file
        public async Task<List<string>> UploadMany(IFormFile[] formFile)
        {
            List<string> urlFilePathList = new List<string>();
            foreach(var f in formFile)
            {
                var fileName = f.FileName;

                //Upload
                var localFilePath = Path.Combine(_webHostEnvironment.ContentRootPath, "Image", fileName);

                var stream = new FileStream(localFilePath, FileMode.Create);

                await f.CopyToAsync(stream);

                //Set URL Static

                var urlFilePath = $"/Image/{fileName}";
                urlFilePathList.Add(urlFilePath);
            }
            return urlFilePathList;
        }

        //Validate token when ConfirmEmail
        public async Task<string> ValidateCodeAsync(string code,UserDTO user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var tokenValidateParams = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidAudience = _configuration["Jwt:Audience"],
                ValidIssuer = _configuration["Jwt:Issuer"],
                IssuerSigningKey = key,
                ClockSkew = TimeSpan.Zero
            };
            try
            {
                //Verify token with option Params
                var tokenInVerification = await jwtTokenHandler.ValidateTokenAsync(code, tokenValidateParams);
                if(!tokenInVerification.IsValid)
                {
                    return null;
                }

                //Check thuat toan
                if (tokenInVerification.SecurityToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);

                    if (!result)
                    {
                        return null;
                    }
                }
                //Check jti
                var jti = tokenInVerification.Claims.FirstOrDefault(x => x.Key == JwtRegisteredClaimNames.Jti).Value;

                if(jti == null)
                {
                    return null;
                }
                if (!jti.ToString().Equals(user.Email))
                {
                    return null;
                }

            }catch (Exception ex)
            {
                return null;
            }
            return code;

        }
    }

}

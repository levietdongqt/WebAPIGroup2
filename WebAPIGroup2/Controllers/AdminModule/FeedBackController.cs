using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebAPIGroup2.Models;
using WebAPIGroup2.Models.DTO;
using WebAPIGroup2.Service.Implement;
using WebAPIGroup2.Service.Inteface;

namespace WebAPIGroup2.Controllers.AdminModule
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedBackController : ControllerBase
    {
        private readonly IFeedBackService _feedBackService;
        private readonly IUtilService _utilService;
        
        public FeedBackController(IFeedBackService feedBackService, IUtilService utilService)
        {
            _feedBackService = feedBackService;
            _utilService= utilService;
        }

        [HttpPut]
        [Route("UpdateAll")]
        public async Task<JsonResult> Update([FromBody] List<FeedBackDTO> updatedDTO)
        {
            var updateDTO = await _feedBackService.UpdateAllAsync(updatedDTO);
            if (updateDTO == null)
            {
                return new JsonResult(new ResponseDTO<FeedBackDTO>(HttpStatusCode.NotFound, "Not Found", null, null));
            }
            var response = new ResponseDTO<List<FeedBackDTO>>(HttpStatusCode.OK, "Update successfully", null, updateDTO);
            return new JsonResult(response);
        }

        [HttpGet]
        [Route("GetByIsImportant")]
        public async Task<JsonResult> GetFeedbacksByStatus(int userId, bool isImportant)
        {
            var feedbacksDTO = await _feedBackService.GetFeedBackByStatus(userId, isImportant);
            if (feedbacksDTO == null)
            {
                return new JsonResult(new ResponseDTO<FeedBackDTO>(HttpStatusCode.NotFound, "Not Found", null, null));
            }
            var response = new ResponseDTO<List<FeedBackDTO>>(HttpStatusCode.OK, "Get successfully", null, feedbacksDTO);
            return new JsonResult(response);
        }
        [HttpPost]
        [Route("SendMail")]
        public async Task<JsonResult> SendMail([FromBody] MailContent mailContent)
        {
           
            var mailContentSended = await _utilService.SendEmailAsync(mailContent);
            if (mailContentSended == null)
            {
                return new JsonResult(new ResponseDTO<MailContent>(HttpStatusCode.NotFound, "Send Mail Fail", null, mailContentSended));
            }
            var response = new ResponseDTO<MailContent>(HttpStatusCode.Created, "Send Mail successfully", null, mailContentSended);
            return new JsonResult(response);
        }
    }
}

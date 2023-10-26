using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebAPIGroup2.Models.DTO;
using WebAPIGroup2.Models.POJO;
using WebAPIGroup2.Service.Inteface;

namespace WebAPIGroup2.Controllers.AdminModule
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "admin")]
    public class ReportController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IFeedBackService _feedBackService;
        private readonly IPurchaseOrderService _purchaseOrderService;
        private readonly ITemplateService _templateService;

        public ReportController(IUserService userService, IFeedBackService feedBackService, IPurchaseOrderService purchaseOrderService, ITemplateService templateService)
        {
            _userService = userService;
            _feedBackService = feedBackService;
            _purchaseOrderService = purchaseOrderService;
            _templateService = templateService;
        }

        [HttpGet]
        [Route("TotalByMonth")]
        public async Task<JsonResult> GetTotalUsersByMonth()
        {
            var user = await _userService.GetTotalUserByMonth();
            var response = new ResponseDTO<dynamic>(HttpStatusCode.OK, "Get Successfully", null, user);
            return new JsonResult(response);
        }
        [HttpGet]
        [Route("5News")]
        public async Task<JsonResult> GetFeedBackTake5New()
        {
            var feedbacks = await _feedBackService.GetFeedBackTake5News();
            if(feedbacks == null)
            {
                return new JsonResult(new ResponseDTO<dynamic>(HttpStatusCode.BadRequest, "Fail", null, null));
            }
            return new JsonResult(new ResponseDTO<dynamic>(HttpStatusCode.OK, "Get Successfully", null, feedbacks));
        }

        [HttpGet]
        public async Task<JsonResult> GetAll()
        {
            var report = await _templateService.GetReportForAdmin();
            if(report == null)
            {
                return new JsonResult(new ResponseDTO<dynamic>(HttpStatusCode.BadRequest, "Fail", null, null));
            }
            return new JsonResult(new ResponseDTO<dynamic>(HttpStatusCode.OK, "Get Successfully", null, report));
        }

        [HttpGet]
        [Route("Purchase")]
        public async Task<JsonResult> GetPOByMonth()
        {
            var po = await _purchaseOrderService.GetPurchaseOrderByMonth();
            if (po == null)
            {
                return new JsonResult(new ResponseDTO<dynamic>(HttpStatusCode.BadRequest, "Fail", null, null));
            }
            return new JsonResult(new ResponseDTO<dynamic>(HttpStatusCode.OK, "Get Successfully", null, po));
        }
    }
}

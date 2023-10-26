using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPIGroup2.Models.DTO;
using WebAPIGroup2.Service.Inteface;

namespace WebAPIGroup2.Controllers.AdminModule
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IPurchaseOrderService _orderService;

        public OrderController(IPurchaseOrderService orderService)
        {
            _orderService = orderService;
        }
        // GET: api/Order/5
        [HttpGet("{id:int}", Name = "Get")]
        public async Task<JsonResult> Get(int id,[FromQuery] string status)
        {
            var order = await _orderService.GetPurchaseOrderByUserId(id,status);   
            if (order == null)
            {
                var response = new ResponseDTO<PurchaseOrderDTO>(HttpStatusCode.OK, "Failure", null, null);
                return new JsonResult(response);
            }
            else
            {
                var response = new ResponseDTO<PurchaseOrderDTO>(HttpStatusCode.OK, "Success", null, order);
                return new JsonResult(response);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPIGroup2.Models.DTO;
using WebAPIGroup2.Service.Inteface;

namespace WebAPIGroup2.Controllers.AdminModule
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IPurchaseOrderService _orderService;

        public OrderController(IPurchaseOrderService orderService)
        {
            _orderService = orderService;
        }
        // GET: api/Order
        [HttpGet]
        public async Task<JsonResult> Get()
        {
            var order = await _orderService.GetPurchaseOrderAll();
            var response = new ResponseDTO<IEnumerable<PurchaseOrderDTO>>(HttpStatusCode.OK, "Success", null, order);
            return new JsonResult(response);
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

        // POST: api/Order
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Order/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/Order/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

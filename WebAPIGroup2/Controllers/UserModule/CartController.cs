using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebAPIGroup2.Models;
using WebAPIGroup2.Models.DTO;
using WebAPIGroup2.Models.POJO;
using WebAPIGroup2.Service.Implement;
using WebAPIGroup2.Service.Inteface;

namespace WebAPIGroup2.Controllers.UserModule
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        public class PurchaseDTO
        {
            public int userId { get; set; }
            public string fullName { get; set; }

            public string phone { get; set; }

            public string email { get; set; }

            public string address { get; set; }

            public decimal? totalPrice { get; set; }

            public string? note { get; set; }
        }
        public class DeleteDTO
        {
           public List<int> productIdList { get; set; }
        }
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;

        }
        [HttpPost]
        [Route("AddToCart")]
        [Authorize(Roles ="user")]
        public async Task<JsonResult> AddToCart([FromForm] OrderDTO orderDTO)
        {
            bool isSucess = await _cartService.AddToCart(orderDTO);
            if (isSucess)
            {
                return new JsonResult(new ResponseDTO<List<ProductDetail>>(HttpStatusCode.OK, "Request Successfull", null, null));
            }
            return new JsonResult(new ResponseDTO<List<ProductDetail>>(HttpStatusCode.BadRequest, "Request is invalid", null, null));
        }
        [HttpPost]
        [Route("addToCartAllSimple")]
        [Authorize(Roles = "user")]
        public async Task<JsonResult> AddToCartAllSimple([FromForm] OrderDTO orderDTO)
        {
            bool isSucess = await _cartService.AddToCartAllSimple(orderDTO);
            if (isSucess)
            {
                return new JsonResult(new ResponseDTO<List<ProductDetail>>(HttpStatusCode.OK, "Request Successfull", null, null));
            }
            return new JsonResult(new ResponseDTO<List<ProductDetail>>(HttpStatusCode.BadRequest, "Request is invalid", null, null));
        }
        [HttpPut]
        [Route("UpdateCart")]
        [Authorize(Roles = "user")]
        public async Task<JsonResult> UpdateCart([FromQuery] int productDetailID, [FromQuery] int quantity)
        {
            bool isSucess = await _cartService.UpdateCart(productDetailID, quantity);
            var response = new ResponseDTO<List<ProductDetail>>(HttpStatusCode.OK, "Request Successfull", null, null);
            return new JsonResult(response);
        }
        [HttpGet]
        [Route("LoadCart")]
        [Authorize(Roles = "user")]
        public async Task<JsonResult> LoadCart([FromQuery] int userID)
        {
            List<CartResponseDTO> myImages = await _cartService.LoadCart(userID);
            if (myImages == null)
            {
                var response = new ResponseDTO<String>(HttpStatusCode.NoContent, "Cart is empty", null, null);
                return new JsonResult(response);
            }
            var response2 = new ResponseDTO<List<CartResponseDTO>>(HttpStatusCode.OK, "Request Successfull", null, myImages);
            return new JsonResult(response2);
        }
        [HttpPost]
        [Route("directPayment")]
        [Authorize(Roles = "user")]
        public async Task<JsonResult> DirectPayment([FromBody] PurchaseDTO purchaseDTO)
        {
            var deliveryInfo = await _cartService.createDeliveryInfo(purchaseDTO);
            var isSucces2 = await _cartService.sendMail(purchaseDTO, deliveryInfo);
            var purchaseOrder = await _cartService.createOrder(purchaseDTO, deliveryInfo, PurchaseStatus.OrderPlaced);

            if (purchaseOrder == null)
            {
                var response = new ResponseDTO<String>(HttpStatusCode.NoContent, "Fail to create order", null, null);
                return new JsonResult(response);
            }

            var response2 = new ResponseDTO<List<CartResponseDTO>>(HttpStatusCode.OK, "Request Successfull", null, null);
            return new JsonResult(response2);
        }
        [HttpPost]
        [Route("payPalPayment")]
        [Authorize(Roles = "user")]
        public async Task<JsonResult> PayPalPayment([FromBody] PurchaseDTO purchaseDTO)
        {
            var deliveryInfo = await _cartService.createDeliveryInfo(purchaseDTO);
            var isSucces2 = await _cartService.sendMail(purchaseDTO, deliveryInfo);
            var purchaseOrder = await _cartService.createOrder(purchaseDTO, deliveryInfo, PurchaseStatus.OrderPaid);

            if (purchaseOrder == null)
            {
                var response = new ResponseDTO<String>(HttpStatusCode.NoContent, "Fail to create order", null, null);
                return new JsonResult(response);
            }
            var response2 = new ResponseDTO<List<CartResponseDTO>>(HttpStatusCode.OK, "Request Successfull", null, null);
            return new JsonResult(response2);
        }
        [HttpDelete]
        [Route("deleteItem")]
        [Authorize(Roles = "user")]
        public async Task<JsonResult> DeleteItem([FromQuery] int productDetailID)
        {
            bool isSucces2 = await _cartService.deleteProductDetail(productDetailID);

            if (!isSucces2)
            {
                var response = new ResponseDTO<String>(HttpStatusCode.NoContent, "Fail to delete this item", null, null);
                return new JsonResult(response);
            }
            var response2 = new ResponseDTO<List<CartResponseDTO>>(HttpStatusCode.OK, "Request Successfull", null, null);
            return new JsonResult(response2);
        }
        [HttpPut]
        [Route("deleteAll")]
        [Authorize(Roles = "user")]
        public async Task<JsonResult> DeleteAll([FromBody] int[] productIdList)
        {
            bool isSucces2 = await _cartService.deleteAllCart(productIdList.ToList());

            if (!isSucces2)
            {
                var response = new ResponseDTO<String>(HttpStatusCode.NoContent, "Fail to delete this item", null, null);
                return new JsonResult(response);
            }
            var response2 = new ResponseDTO<List<CartResponseDTO>>(HttpStatusCode.OK, "Request Successfull", null, null);
            return new JsonResult(response2);
        }
        [HttpDelete]
        [Route("deleteFolder")]
        [Authorize(Roles ="admin")]
        public async Task<JsonResult> deleteFolder([FromQuery] int purchaseID)
        {
            bool isDelete = await _cartService.deleteFolder(purchaseID);
            if (isDelete)
            {
                return new JsonResult(new ResponseDTO<string>(HttpStatusCode.OK, "Request Successfull", null, null));
            }
            return new JsonResult(new ResponseDTO<string>(HttpStatusCode.BadRequest, "Request Fail", null, null));

        }
    }
}

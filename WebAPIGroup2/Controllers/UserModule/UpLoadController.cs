using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebAPIGroup2.Models.DTO;
using WebAPIGroup2.Models.POJO;
using WebAPIGroup2.Service.Inteface;

namespace WebAPIGroup2.Controllers.UserModule
{
    [Route("api/[controller]")]
    [ApiController]
    public class UpLoadController : ControllerBase
    {
        private readonly IUpLoadService _upLoadService;
        private readonly IUtilService _utilService;
        public UpLoadController(IUpLoadService upLoadService, IUtilService utilService)
        {
            _upLoadService = upLoadService;
            _utilService = utilService;
        }
        [HttpPost]
        [Route("WithTemplate")]
        public async Task<JsonResult> UpLoadWithTemplate([FromForm] UpLoadDTO upLoadDTO)
        {
            var emailName = _upLoadService.ValidateRequestData(upLoadDTO);
            var isValidImages = _upLoadService.ValidateFiles(upLoadDTO.files);
            await Task.WhenAll(isValidImages, emailName);
            if (emailName.Result == null)
            {
                var response = new ResponseDTO<String>(HttpStatusCode.BadRequest, "Request Data is invalid", null, null);
                return new JsonResult(response);
            }
            if (!isValidImages.Result)
            {
                var response = new ResponseDTO<String>(HttpStatusCode.BadRequest, "File is invalid", null, null);
                return new JsonResult(response);
            }
            string folderName = $"{upLoadDTO.userID}_{emailName.Result}";
            var imagesUrls = await _upLoadService.SaveImages(folderName, upLoadDTO.templateID, upLoadDTO.files);
            if (upLoadDTO.templateID == 1)
            {
                List<int> myImage = await _upLoadService.SaveToDBWithNoTemplate(folderName, upLoadDTO, imagesUrls);
                var response = new ResponseDTO<List<int>>(HttpStatusCode.OK, "Save successfull", null, myImage);
                return new JsonResult(response);
            }
            var myImage2 = await _upLoadService.SaveToDBTemporary(folderName, upLoadDTO, imagesUrls);
            var response2 = new ResponseDTO<int>(HttpStatusCode.OK, "Save successfull", null, myImage2.Id);
            return new JsonResult(response2);
        }
        [HttpPost]
        [Route("WithNoTemplate")]
        public async Task<JsonResult> UpLoadWithNoTemplate([FromForm] UpLoadDTO upLoadDTO)
        {
            if (upLoadDTO.templateID == 1 || upLoadDTO.templateID == null)
            {
                upLoadDTO.templateID = 1;
                var emailName = _upLoadService.ValidateRequestData(upLoadDTO);
                var isValidImages = _upLoadService.ValidateFiles(upLoadDTO.files);
                await Task.WhenAll(isValidImages, emailName);
                if (emailName.Result == null)
                {
                    return new JsonResult(new ResponseDTO<String>(HttpStatusCode.BadRequest, "Request Data is invalid", null, null));
                }
                if (!isValidImages.Result)
                {
                    return new JsonResult(new ResponseDTO<String>(HttpStatusCode.BadRequest, "File is invalid", null, null));
                }
                string folderName = $"{upLoadDTO.userID}_{emailName.Result}";
                var imagesUrls = await _upLoadService.SaveImages(folderName, 1, upLoadDTO.files);
                List<int> myImage = await _upLoadService.SaveToDBWithNoTemplate(folderName, upLoadDTO, imagesUrls);
                return new JsonResult(new ResponseDTO<List<int>>(HttpStatusCode.OK, "Save successfull", null, myImage));
             
            }
            return new JsonResult(new ResponseDTO<String>(HttpStatusCode.BadRequest, "Template is invalid", null, null));

        }
        [HttpGet]
        [Route("LoadMyImages")]
        public async Task<JsonResult> LoadMyImage([FromQuery] int userID)
        {
            var myImages = await _upLoadService.LoadMyImages(userID);
            if (myImages == null)
            {
                var response = new ResponseDTO<String>(HttpStatusCode.NoContent, "MyImages is empty", null, null);
                return new JsonResult(response);
            }
            var response2 = new ResponseDTO<List<MyImagesResponseDTO>>(HttpStatusCode.OK, "Request Successfull", null, myImages);
            return new JsonResult(response2);
        }
        [HttpGet]
        [Route("LoadNoTemplate")]
        public async Task<JsonResult> LoadNoTemplate([FromQuery] int userID)
        {
            var myImages = await _upLoadService.LoadNoTemplate(userID);
            if (myImages == null)
            {
                var response = new ResponseDTO<String>(HttpStatusCode.NoContent, "NoTemplate is empty", null, null);
                return new JsonResult(response);
            }
            var response2 = new ResponseDTO<List<MyImagesResponseDTO>>(HttpStatusCode.OK, "Request Successfull", null, myImages);
            return new JsonResult(response2);
        }
        [HttpPost]
        [Route("AddToCart")]
        public async Task<JsonResult> AddToCart([FromForm] OrderDTO orderDTO)
        {
            bool isSucess = await _upLoadService.AddToCart(orderDTO);
            if (isSucess)
            {
                return new JsonResult(new ResponseDTO<List<ProductDetail>>(HttpStatusCode.OK, "Request Successfull", null, null));
            }
            return new JsonResult(new ResponseDTO<List<ProductDetail>>(HttpStatusCode.BadRequest, "Request is invalid", null, null));
        }
        [HttpPut]
        [Route("UpdateCart")]
        public async Task<JsonResult> UpdateCart([FromQuery] int productDetailID, [FromQuery] int quantity)
        {
            bool isSucess = await _upLoadService.UpdateCart(productDetailID, quantity);
            var response = new ResponseDTO<List<ProductDetail>>(HttpStatusCode.OK, "Request Successfull", null, null);
            return new JsonResult(response);
        }
        [HttpGet]
        [Route("LoadCart")]
        public async Task<JsonResult> LoadCart([FromQuery] int userID)
        {
            List<CartResponseDTO> myImages = await _upLoadService.LoadCart(userID);
            if (myImages == null)
            {
                var response = new ResponseDTO<String>(HttpStatusCode.NoContent, "Cart is empty", null, null);
                return new JsonResult(response);
            }
            var response2 = new ResponseDTO<List<CartResponseDTO>>(HttpStatusCode.OK, "Request Successfull", null, myImages);
            return new JsonResult(response2);
        }
    }
}

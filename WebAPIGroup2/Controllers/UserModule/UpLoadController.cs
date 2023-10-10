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
        [Route("SaveImages")]
        public async Task<JsonResult> UpLoadWithTemplate([FromForm] UpLoadDTO upLoadDTO)
        {
            var emailName =  _upLoadService.ValidateRequestData(upLoadDTO);
            var isValidImages =    _upLoadService.ValidateFiles(upLoadDTO.files);
            await Task.WhenAll(isValidImages, emailName);         
            if(emailName.Result == null)
            {   
                var response = new ResponseDTO<String>(HttpStatusCode.BadRequest, "Request Data is invalid", null, null);
                return new JsonResult(response);
            }
            if (!isValidImages.Result)
            {
                var response = new ResponseDTO<String>(HttpStatusCode.BadRequest, "File is invalid", null, null);
                return new JsonResult(response);
            }
            string folderName = $"{emailName.Result}{upLoadDTO.userID}";
            var imagesUrls = await _upLoadService.SaveImages(folderName, upLoadDTO.templateID, upLoadDTO.files);
            var myImage = await _upLoadService.SaveToDBTemporary(folderName,upLoadDTO, imagesUrls);
            var response2 = new ResponseDTO<MyImage>(HttpStatusCode.OK, "Save successfull", null, myImage);
            return new JsonResult(response2);
        }
        [HttpGet]
        [Route("LoadMyImage")]
        public async Task<JsonResult> LoadMyImage([FromQuery] int userID)
        {
            var myImages = await _upLoadService.LoadMyImages(userID);
            if(myImages == null)
            {
                var response= new ResponseDTO<String>(HttpStatusCode.NoContent, "Product is not exist", null, null);
                return new JsonResult(response);
            }
            var response2 = new ResponseDTO<List<MyImage>>(HttpStatusCode.OK, "Request Successfull", null, myImages);
            return new JsonResult(response2);
        }
        [HttpPost]
        [Route("AddToCart")]
        public async Task<JsonResult> AddToCart([FromForm] OrderDTO orderDTO)
        {
            bool check =    await _upLoadService.CreateOrder(orderDTO);
            var response = new ResponseDTO<List<ProductDetail>>(HttpStatusCode.OK, "Request Successfull", null, null);
            return new JsonResult(response);
        }
    }
}

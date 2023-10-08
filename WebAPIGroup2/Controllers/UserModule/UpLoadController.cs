using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebAPIGroup2.Models.DTO;
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
        [Route("defaultTemplate")]
        public async Task<JsonResult> UpLoadWithDefaultTemplate([FromForm] UpLoadDTO upLoadDTO)
        {
            var isValidRequestData =  _upLoadService.ValidateRequestData(upLoadDTO);
            var isValidImages =    _upLoadService.ValidateFiles(upLoadDTO.files);
            Task.WhenAll(isValidImages, isValidRequestData);         
            if(!isValidRequestData.Result)
            {
                var response = new ResponseDTO<String>(HttpStatusCode.BadRequest, "Request Data is invalid", null, null);
                return new JsonResult(response);
            }
            if (!isValidImages.Result)
            {
                var response = new ResponseDTO<String>(HttpStatusCode.BadRequest, "File is invalid", null, null);
                return new JsonResult(response);
            }
            var imagesUrls = await _upLoadService.SaveImages(upLoadDTO.userID,1, upLoadDTO.files);
            bool isSavedDB = await _upLoadService.SaveProductDetails(upLoadDTO,imagesUrls);
            var response2 = new ResponseDTO<String>(HttpStatusCode.OK, "Save successfull", null, null);
            return new JsonResult(response2);

        }
    }
}

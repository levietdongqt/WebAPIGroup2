using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles = "user")]
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
                var response = new ResponseDTO<String>(HttpStatusCode.BadRequest, "File is invalid! Don't upload the same images", null, null);
                return new JsonResult(response);
            }
            string folderName = $"{upLoadDTO.userID}_{emailName.Result}";
            var imagesUrls = await _upLoadService.SaveImages(folderName, upLoadDTO.templateID, upLoadDTO.files);
            if(imagesUrls == null)
            {
                var response = new ResponseDTO<String>(HttpStatusCode.BadRequest, "Some images already exist in this template!", null, null);
                return new JsonResult(response);
            }
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
                    return new JsonResult(new ResponseDTO<String>(HttpStatusCode.BadRequest, "File is invalid! Don't upload the same images", null, null));
                }
                string folderName = $"{upLoadDTO.userID}_{emailName.Result}";
                var imagesUrls = await _upLoadService.SaveImages(folderName, 1, upLoadDTO.files);
                if (imagesUrls == null)
                {
                    var response = new ResponseDTO<String>(HttpStatusCode.BadRequest, "Some images have been uploaded before!", null, null);
                    return new JsonResult(response);
                }
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
        [Route("LoadMyImagesByTemplateId")]
        public async Task<JsonResult> LoadMyImagesByTemplateId([FromQuery] int templateId, [FromQuery] int userId)
        {
            var myImages = await _upLoadService.LoadMyImages(userId);
            if (myImages == null)
            {
                var response = new ResponseDTO<String>(HttpStatusCode.NoContent, "MyImages is empty", null, null);
                return new JsonResult(response);
            }
            var myImage = myImages.FirstOrDefault(t => t.templateId == templateId);
            if (myImage == null)
            {
                myImage = new MyImagesResponseDTO();
                myImage.templateId = templateId;
            }
            var response2 = new ResponseDTO<MyImagesResponseDTO>(HttpStatusCode.OK, "Request Successfull", null, myImage);
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
        [HttpDelete]
        [Route("deleteMyImage")]
        public async Task<JsonResult> DeleteMyImage([FromQuery] int myImmageId)
        {
            var isSucces= await _upLoadService.deleteMyImage(myImmageId);
            if (!isSucces)
            {
                var response = new ResponseDTO<String>(HttpStatusCode.NoContent, "Delete Fail", null, null);
                return new JsonResult(response);
            }
            var response2 = new ResponseDTO<List<string>>(HttpStatusCode.OK, "Request Successfull", null, null);
            return new JsonResult(response2);
        }
        [HttpPost]
        [Route("deleteImages")]
        public async Task<JsonResult> DeleteImages([FromBody] List<int> imageIdList)
        {
            var isSucces = await _upLoadService.deleteImages(imageIdList.ToList());
            if (!isSucces)
            {
                var response = new ResponseDTO<String>(HttpStatusCode.NoContent, "Delete fail! Some image have product in the cart ", null, null);
                return new JsonResult(response);
            }
            var response2 = new ResponseDTO<List<string>>(HttpStatusCode.OK, "Request Successfull", null, null);
            return new JsonResult(response2);
        }

    }
}

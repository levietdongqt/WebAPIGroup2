using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using WebAPIGroup2.Models;
using WebAPIGroup2.Models.DTO;
using WebAPIGroup2.Service.Inteface;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace WebAPIGroup2.Controllers.TemplateModule
{
    [Route("api/[controller]")]
    [ApiController]
    public class TemplateController : ControllerBase
    {
        private readonly ITemplateService templateService;
        private readonly IUtilService utilService;
        public TemplateController(ITemplateService templateService, IUtilService utilService)
        {
            this.templateService = templateService;
            this.utilService = utilService;
        }
    
        [HttpGet]
        public async Task<JsonResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery, [FromQuery] string? sortBy, [FromQuery] bool? isAscending, [FromQuery] bool? status, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000)
        {
            var templateDTO = await templateService.GetAllAsync(filterOn, filterQuery, sortBy, isAscending ?? true,status ?? true,pageNumber, pageSize);

            var response = new ResponseDTO<List<TemplateDTO>>(HttpStatusCode.OK, "Success", null, templateDTO);
            return new JsonResult(response);
        }

        [HttpGet("bestSeller")]
        public async Task<JsonResult> Get([FromQuery] bool? status)
        {
            var templateDto = await templateService.GetBestSeller(status ?? true);
            var response = new ResponseDTO<List<TemplateDTO>>(HttpStatusCode.OK, "Success", null, templateDto);
            return new JsonResult(response);
        }
        [HttpGet("GetTemplateByName")]
        public async Task<JsonResult> Get([FromQuery] string? name,[FromQuery]int page = 1, [FromQuery]int limit = 1)
        {
            var templateDto = await templateService.GetByNameAsync(name,page,limit);
            var response = new ResponseDTO<PaginationDTO<TemplateDTO>>(HttpStatusCode.OK, "Success", null, templateDto);
            return new JsonResult(response);
        }   
        [HttpGet]
        [Route("{id:int}")]
        public async Task<JsonResult> GetById(int id)
        {
            var templateDto = await templateService.GetByIDAsync(id);
            var response = new ResponseDTO<TemplateDTO>(HttpStatusCode.OK, "Success", null, templateDto);
            return new JsonResult(response);
        }

        [HttpPost]
        public async Task<JsonResult> Create([FromForm] AddTemplateDTO addTemplateDTO)          
        {
            var response = new ResponseDTO<TemplateDTO>();
            try
            {
                utilService.ValiadateAllFileUpload(addTemplateDTO.formFileList);
                var templateDTO = await templateService.CreateAsync(addTemplateDTO);
                response = new ResponseDTO<TemplateDTO>(HttpStatusCode.Created, "Add success", null, templateDTO);
            }
            catch (Exception ex)
            {
                response = new ResponseDTO<TemplateDTO>(HttpStatusCode.BadRequest,ex.Message, null,null);
            }          
            return new JsonResult(response);
        }
        [HttpPost]
        [Route("{templateId:int}/description")]
        public async Task<JsonResult> CreateDescription([FromRoute] int templateId, [FromBody] List<DescriptionTemplateDTO> descriptionTemplateDTOs)
        {
            var templateDTO = await templateService.AddDescriptionByTemplateIdAsync(templateId, descriptionTemplateDTOs);
            if (templateDTO == null)
            {
                return new JsonResult(new ResponseDTO<TemplateDTO>(HttpStatusCode.BadRequest, "Add Fail", null, null));
            }
            var response = new ResponseDTO<TemplateDTO>(HttpStatusCode.Created, "Add Desciption successful", null, templateDTO);
            return new JsonResult(response);
        }

        [HttpPost]
        [Route("{templateId:int}/image")]
        public async Task<JsonResult> CreateImage([FromRoute] int templateId, [FromForm] IFormFile[] formFiles)
        {
            var response = new ResponseDTO<TemplateDTO>();
            try
            {
                utilService.ValiadateAllFileUpload(formFiles);
                var templateDTO = await templateService.AddImageByTemplateIdAsync(templateId, formFiles);
                if (templateDTO == null)
                {
                    return new JsonResult(new ResponseDTO<TemplateDTO>(HttpStatusCode.BadRequest, "Add Fail", null, null));
                }
                response = new ResponseDTO<TemplateDTO>(HttpStatusCode.Created, "Add Image successful", null, templateDTO);
            }
            catch(Exception ex)
            {
                response = new ResponseDTO<TemplateDTO>(HttpStatusCode.BadRequest, ex.Message, null, null);
            }           
            return new JsonResult(response);
        }


        [HttpPut]
        [Route("{id:int}")]
        public async Task<JsonResult> Update([FromRoute] int id, [FromBody] AddTemplateDTO updateTemplateDTO)
        {
            var templateDTO = await templateService.UpdateAsync(id, updateTemplateDTO);
            if (templateDTO == null)
            {
                return new JsonResult(new ResponseDTO<TemplateDTO>(HttpStatusCode.NotFound, "Template is null", null, null));
            }
            var response = new ResponseDTO<TemplateDTO>(HttpStatusCode.OK, "Update success", null, templateDTO);
            return new JsonResult(response);
        }
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<JsonResult> Delete([FromRoute] int id)
        {
            var templateDTO = await templateService.UpdateStatusAsync(id);
            if (templateDTO == null)
            {
                return new JsonResult(new ResponseDTO<TemplateDTO>(HttpStatusCode.NotFound, "Template is null", null, null));
            }
            var response = new ResponseDTO<TemplateDTO>(HttpStatusCode.OK, "Delete success", null, templateDTO);
            return new JsonResult(response);
        }

        [HttpPut]
        public async Task<JsonResult> DeleteAll([FromBody] int[] id)
        {
            var templateDTOs = await templateService.UpdateAllStatusAsync(id);
            if (templateDTOs.IsNullOrEmpty())
            {
                return new JsonResult(new ResponseDTO<TemplateDTO>(HttpStatusCode.NotFound, "Template is null", null, null));
            }
            var response = new ResponseDTO<List<TemplateDTO>>(HttpStatusCode.OK, "Delete success", null, templateDTOs);
            return new JsonResult(response);
        }

        [HttpPut]
        [Route("{templateId:int}/description")]
        public async Task<JsonResult> UpdateDescriptionTemplateByTemplateIdAsync([FromRoute] int templateId, [FromBody] List<DescriptionTemplateDTO> descriptionTemplateDTOs)
        {
            var templateDTO = await templateService.UpdateDescriptionByTemplateIdAsync(templateId, descriptionTemplateDTOs);
            if (templateDTO == null)
            {
                return new JsonResult(new ResponseDTO<TemplateDTO>(HttpStatusCode.NotFound, "Template is null", null, null));
            }
            var response = new ResponseDTO<TemplateDTO>(HttpStatusCode.Created, "Update Description successful", null, templateDTO);
            return new JsonResult(response);
        }
    }
}

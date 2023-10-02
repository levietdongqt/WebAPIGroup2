using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<JsonResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery, [FromQuery] string? sortBy, [FromQuery] bool? isAscending, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000)
        {
            var templateDTO = await templateService.GetAllAsync(filterOn, filterQuery, sortBy, isAscending ?? true, pageNumber, pageSize);

            var response = new ResponseDTO<List<TemplateDTO>>(HttpStatusCode.OK, "Success", null, templateDTO);
            return new JsonResult(response);
        }


        [HttpGet]
        [Route("{id:int}")]
        public async Task<JsonResult> GetById(int id)
        {
            var templateDTO = await templateService.GetByIDAsync(id);
            var response = new ResponseDTO<TemplateDTO>(HttpStatusCode.OK, "Success", null, templateDTO);
            return new JsonResult(response);
        }

        [HttpPost]
        public async Task<JsonResult> Create([FromForm] AddTemplateDTO addTemplateDTO)
        {
            var templateDTO = await templateService.CreateAsync(addTemplateDTO);
            var response = new ResponseDTO<TemplateDTO>(HttpStatusCode.Created,"Add success",null, templateDTO);
            return new JsonResult(response);
        }

        [HttpPut]
        public async Task<JsonResult> Update(int id, [FromBody] AddTemplateDTO updateTemplateDTO)
        {
            var templateDTO = await templateService.UpdateAsync(id, updateTemplateDTO);
            if(templateDTO == null)
            {
                return new JsonResult(new ResponseDTO<TemplateDTO>(HttpStatusCode.NotFound, "Template is null", null, null));
            }
            var response = new ResponseDTO<TemplateDTO>(HttpStatusCode.Created, "Update success", null, templateDTO);
            return new JsonResult(response);
        }
        [HttpPost]
        [Route("Test")]
        public async Task<IActionResult> Test([FromForm] Test test)
        {
            List<string> a = await utilService.UploadMany(test.formFile);
            return Ok(a);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPIGroup2.Models.DTO;
using WebAPIGroup2.Service.Inteface;

namespace WebAPIGroup2.Controllers.UserModule
{
    [Route("api/[controller]")]
    [ApiController]
    public class TemplateController : ControllerBase
    {
        private readonly ITemplateService _templateService;

        public TemplateController(ITemplateService templateService)
        {
            _templateService = templateService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllTemplate()
        {
            var template = await _templateService.GetAllAsync();
            if(template != null)
            {
                var response = new ResponseDTO<IEnumerable<TemplateDTO>>(
                    HttpStatusCode.OK,
                    "Get successfully",
                    null,
                    template
                ); 
                return Ok(response);
            }
            else
            {
                return new NotFoundObjectResult(new ResponseDTO<IEnumerable<CategoryDTO>?>(
                    HttpStatusCode.NotFound,
                    "Failed",
                    null,
                    null
                ));
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTemplateById(int id)
        {
            var template = await _templateService.GetTemplateById(id);
            if (template != null)
            {
                var response = new ResponseDTO<TemplateWithCategoryDTO>(
                    HttpStatusCode.OK,
                    "Get successfully",
                    null,
                    template
                );
                return Ok(response);
            }
            else
            {
                return new NotFoundObjectResult(new ResponseDTO<TemplateWithCategoryDTO?>(
                    HttpStatusCode.NotFound,
                    "Failed",
                    null,   
                    null));
            }
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateTemplate(TemplateDTO templateDto)
        {
            var templates = await _templateService.CreateTemplate(templateDto);
            if (templates != null)
            {
                return Ok(new ResponseDTO<TemplateDTO>(HttpStatusCode.Created, "Create successfully", null, templates));
            }
            else
            {
                return BadRequest(new ResponseDTO<TemplateDTO?>(HttpStatusCode.BadRequest, "Create failed", null, null));
            }
        }
    }
}

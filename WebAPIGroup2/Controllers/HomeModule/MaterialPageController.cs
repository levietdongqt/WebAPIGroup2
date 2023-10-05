using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebAPIGroup2.Models.DTO;
using WebAPIGroup2.Service.Inteface;

namespace WebAPIGroup2.Controllers.HomeModule
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialPageController : ControllerBase
    {
        public readonly IMaterialPageService _materialPageService;

        public MaterialPageController(IMaterialPageService materialPageService)
        {
            _materialPageService = materialPageService;
        }

        [HttpGet]
        public async Task<JsonResult> GetAll()
        {
            var materialPageDTOs = await _materialPageService.GetAllAsync();
            var response = new ResponseDTO<List<MaterialPageDTO>>(HttpStatusCode.OK, "Get All Successfully", null,materialPageDTOs);
            return new JsonResult(response);
        }
        [HttpGet]
        [Route("{id:int}")]
        public async Task<JsonResult> GetById([FromRoute] int id)
        {
            var materialPageDTO = await _materialPageService.GetByIdAsync(id);
            if(materialPageDTO == null)
            {
                return new JsonResult(new ResponseDTO<MaterialPageDTO>(HttpStatusCode.NotFound, "Material Page dont exist", null, null));
            }
            var response = new ResponseDTO<MaterialPageDTO>(HttpStatusCode.OK, "Get Successfully", null, materialPageDTO);
            return new JsonResult(response);
        }

        [HttpPost]
        public async Task<JsonResult> Create([FromBody] MaterialPageDTO materialPageDTO)
        {
            var createdDTO = await _materialPageService.CreateAsync(materialPageDTO);
            if(createdDTO == null)
            {
                return new JsonResult(new ResponseDTO<MaterialPageDTO>(HttpStatusCode.BadRequest, "Created fail", null, null));
            }
            return new JsonResult(new ResponseDTO<MaterialPageDTO>(HttpStatusCode.Created, "Created successfully", null, createdDTO));
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<JsonResult> Update([FromRoute] int id, [FromBody] MaterialPageDTO materialPageDTO)
        {
            var updatedDTO = await _materialPageService.UpdateAsync(id,materialPageDTO);
            if (updatedDTO == null)
            {
                return new JsonResult(new ResponseDTO<MaterialPageDTO>(HttpStatusCode.NotFound, "Update is fail", null, null));
            }
            return new JsonResult(new ResponseDTO<MaterialPageDTO>(HttpStatusCode.OK, "Updated successfully", null, updatedDTO));
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebAPIGroup2.Models.DTO;
using WebAPIGroup2.Service.Inteface;

namespace WebAPIGroup2.Controllers.AdminModule
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<JsonResult> GetAll()
        {
            var categoriesDTO = await _categoryService.GetAll();
            var response = new ResponseDTO<List<CategoryDTO>>(HttpStatusCode.OK,"Get all OK",null,categoriesDTO);
            return new JsonResult(response);
        }
    }
}

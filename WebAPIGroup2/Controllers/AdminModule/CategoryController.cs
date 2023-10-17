
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        // GET: api/Category
        [HttpGet]
        public async Task<JsonResult>  Get()
        {
            var categories = await _categoryService.GetAllCategories();
            if (categories != null)
            {
                var response = new ResponseDTO<List<CategoryDTO>>(HttpStatusCode.OK, "Success", null, categories);
                return new JsonResult(response);
            }
            else
            {
                var response = new ResponseDTO<List<CategoryDTO>?>(HttpStatusCode.BadRequest, "Failure", null, null);
                return new JsonResult(response);
            }
        }

        // GET: api/Category/5
        [HttpGet("{id}")]
        public  async Task<JsonResult>  Get(int id)
        {
            var category = await _categoryService.GetCategoryById(id);
            if (category != null)
            {
                var response = new ResponseDTO<CategoryDTO>(HttpStatusCode.OK, "Success", null, category);
                return new JsonResult(response);
            }
            else
            {
                var response = new ResponseDTO<CategoryDTO>(HttpStatusCode.BadRequest, "Failure", null, null);
                return new JsonResult(response);
                
            }
        }

        // POST: api/Category
        [HttpPost]
        public async Task<JsonResult> Post([FromForm] CategoryDTO categoryDto)
        {
            var response = new ResponseDTO<CategoryDTO>();
            try
            {
                var category = await _categoryService.CreateCategory(categoryDto);
                 response = new ResponseDTO<CategoryDTO>(HttpStatusCode.Created, "Success", null, category);
            }
            catch(Exception)
            {
                response = new ResponseDTO<CategoryDTO>(HttpStatusCode.BadRequest, "Failure", null, null);
            }
            return new JsonResult(response);
        }

        // PUT: api/Category/5
        [HttpPut("{id}")]
        public async Task<JsonResult> Put(int id, [FromBody] CategoryDTO categoryDto)
        {
            var category = await _categoryService.UpdateCategory(id,categoryDto);
            if (category == null)
            {
                var response = new ResponseDTO<CategoryDTO>(HttpStatusCode.BadRequest, "Failure", null, null);
                return new JsonResult(response);    
            }
            else
            {
                var response = new ResponseDTO<CategoryDTO>(HttpStatusCode.OK, "Success", null, category);
                return new JsonResult(response);
            }
        }

        // DELETE: api/Category/5
        [HttpDelete("{id}")]
        public async Task<JsonResult> Delete(int id)
        {
            var deleted = await _categoryService.DeleteCategory(id);
            if (deleted)
            {
                var response = new ResponseDTO<CategoryDTO>(HttpStatusCode.OK, "Success", null, null);
                return new JsonResult(response);
            }
            else
            {
                var response = new ResponseDTO<CategoryDTO>(HttpStatusCode.BadRequest, "Failure", null, null);
                return new JsonResult(response);
            }
        }
    }
}

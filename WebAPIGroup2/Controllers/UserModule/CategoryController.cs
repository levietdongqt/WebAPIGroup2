using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPIGroup2.Models;
using WebAPIGroup2.Models.DTO;
using WebAPIGroup2.Models.POJO;
using WebAPIGroup2.Service.Inteface;

namespace WebAPIGroup2.Controllers.UserModule
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly MyImageContext _context;
        private readonly ICategoryService _categoryService;
        public CategoryController(MyImageContext context, ICategoryService categoryService)
        {
            _context = context;
            _categoryService = categoryService;
        }

        // GET: api/Category
        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _categoryService.GetAllasync();
            if(categories != null)
            {
                var response = new ResponseDTO<IEnumerable<CategoryDTO>>(
                        HttpStatusCode.OK,
                        "Get successfully",
                        null,
                        categories
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

        [HttpGet("Template/{id}")]
        public async Task<IActionResult> GetTemplate(int id)
        {
            var categoryTemplate = await _categoryService.GetCategoryWithTemplate(id);
            if (categoryTemplate != null)
            {
                var response = new ResponseDTO<CategoryWithTemplateDTO>(HttpStatusCode.OK,"Get successfully",null,categoryTemplate);
                return Ok(response);  
            }
            else
            {
                var response = new ResponseDTO<CategoryWithTemplateDTO?>(HttpStatusCode.BadRequest,"Get successfully",null,null);
                return BadRequest(response); 
            }
        }
        // GET: api/Category/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetCategory(int id)
        {
            var category = await _categoryService.GetCategoryById(id);
            if (category != null)
            {
                var response = new ResponseDTO<CategoryDTO?>(HttpStatusCode.OK,"Get successfully",null,category);
                return Ok(response);  
            }
            else
            {
                var response = new ResponseDTO<CategoryDTO?>(HttpStatusCode.BadRequest,"Get successfully",null,null);
                return BadRequest(response); 
            }
            
        }
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateCategory(CategoryDTO category)
        {
            bool updateCategory = await _categoryService.UpdateCategory(category);
            if (updateCategory)
            {
                return Ok(new ResponseDTO<CategoryDTO>(HttpStatusCode.Accepted, "Update successfully", null, category));
            }
            else
            {
                return BadRequest(new ResponseDTO<CategoryDTO?>(HttpStatusCode.BadRequest, "Update failed", null, null)); 
            }
        }
        [HttpPost("Create")]
        public async Task<IActionResult> CreateCategory(CategoryDTO category)
        {
          var createCategory = await _categoryService.CreateCategory(category);
          if (createCategory != null)
          {
              return Ok(new ResponseDTO<CategoryDTO>(HttpStatusCode.Created, "Create successfully", null, category));
          }
          else
          {
              return BadRequest(new ResponseDTO<CategoryDTO?>(HttpStatusCode.BadRequest, "Create failed", null, null));
          }
        }

        // DELETE: api/Category/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var deleteCategory = await _categoryService.DeleteCategory(id);
            if (deleteCategory)
            {
                return Ok(new ResponseDTO<CategoryDTO?>(HttpStatusCode.Accepted, "Delete successfully", null, null));
            }
            else
            {
                return BadRequest(new ResponseDTO<CategoryDTO?>(HttpStatusCode.BadRequest, "Delete failed", null, null));
            }
        }
        
    }
}

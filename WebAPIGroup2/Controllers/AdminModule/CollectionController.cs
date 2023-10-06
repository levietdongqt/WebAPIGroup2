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

namespace WebAPIGroup2.Controllers.HomeModule
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollectionController : ControllerBase
    {
        private readonly MyImageContext _context;
        private readonly ICollectionService _collectionService;
        public CollectionController(MyImageContext context, ICollectionService collectionService)
        {
            _context = context;
            _collectionService = collectionService;
        }

        // GET: api/Category
        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _collectionService.GetAllasync();
            if (categories != null)
            {
                var response = new ResponseDTO<IEnumerable<CollectionDTO>>(
                        HttpStatusCode.OK,
                        "Get successfully",
                        null,
                        categories
                );
                return Ok(response);
            }
            else
            {
                return new NotFoundObjectResult(new ResponseDTO<IEnumerable<CollectionDTO>?>(
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
            var collectionTemplate = await _collectionService.GetCollectionWithTemplate(id);
            if (collectionTemplate != null)
            {
                var response = new ResponseDTO<CollectionWithTemplateDTO>(HttpStatusCode.OK, "Get successfully", null, collectionTemplate);
                return Ok(response);
            }
            else
            {
                var response = new ResponseDTO<CollectionWithTemplateDTO?>(HttpStatusCode.BadRequest, "Get successfully", null, null);
                return BadRequest(response);
            }
        }
        // GET: api/Category/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetCollection(int id)
        {
            var collection = await _collectionService.GetCollectionById(id);
            if (collection != null)
            {
                var response = new ResponseDTO<CollectionDTO?>(HttpStatusCode.OK, "Get successfully", null, collection);
                return Ok(response);
            }
            else
            {
                var response = new ResponseDTO<CollectionDTO?>(HttpStatusCode.BadRequest, "Get successfully", null, null);
                return BadRequest(response);
            }

        }
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateCollection(CollectionDTO collection)
        {
            bool updateCollection = await _collectionService.UpdateCollection(collection);
            if (updateCollection)
            {
                return Ok(new ResponseDTO<CollectionDTO>(HttpStatusCode.Accepted, "Update successfully", null, collection));
            }
            else
            {
                return BadRequest(new ResponseDTO<CollectionDTO?>(HttpStatusCode.BadRequest, "Update failed", null, null));
            }
        }
        [HttpPost("Create")]
        public async Task<IActionResult> CreateCollection(CollectionDTO collection)
        {
            var createCollection = await _collectionService.CreateCollection(collection);
            if (createCollection != null)
            {
                return Ok(new ResponseDTO<CollectionDTO>(HttpStatusCode.Created, "Create successfully", null, collection));
            }
            else
            {
                return BadRequest(new ResponseDTO<CollectionDTO?>(HttpStatusCode.BadRequest, "Create failed", null, null));
            }
        }

        // DELETE: api/Category/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCollection(int id)
        {
            var deleteCollection = await _collectionService.DeleteCollection(id);
            if (deleteCollection)
            {
                return Ok(new ResponseDTO<CollectionDTO?>(HttpStatusCode.Accepted, "Delete successfully", null, null));
            }
            else
            {
                return BadRequest(new ResponseDTO<CollectionDTO?>(HttpStatusCode.BadRequest, "Delete failed", null, null));
            }
        }

    }
}

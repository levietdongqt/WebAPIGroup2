using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebAPIGroup2.Models.DTO;
using WebAPIGroup2.Service.Inteface;

namespace WebAPIGroup2.Controllers.HomeModule
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;
        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpPost]
        public async Task<JsonResult> Create([FromBody] AddReviewDTO addReviewDTO)
        {
            var createdReviewDTO = await _reviewService.CreateAsync(addReviewDTO);
            var response = new ResponseDTO<ReviewDTO>(HttpStatusCode.Created, "Created successfully", null, createdReviewDTO);
            return new JsonResult(response);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<JsonResult> Delete([FromRoute] int id)
        {
            var deletedDTO = await _reviewService.DeleteAsync(id);
            if(deletedDTO == null)
            {
                return new JsonResult(new ResponseDTO<ReviewDTO>(HttpStatusCode.NotFound, "Not Found", null, null));
            }
            var response = new ResponseDTO<ReviewDTO>(HttpStatusCode.OK, "Delete successfully", null, deletedDTO);
            return new JsonResult(response);
        }

        [HttpPut]
        [Route("UpdateAll")]
        public async Task<JsonResult> Update([FromBody] List<ReviewDTO> updatedDTO)
        {
            var updateDTO = await _reviewService.UpdateAllAsync(updatedDTO);
            if (updateDTO == null)
            {
                return new JsonResult(new ResponseDTO<ReviewDTO>(HttpStatusCode.NotFound, "Not Found", null, null));
            }
            var response = new ResponseDTO<List<ReviewDTO>>(HttpStatusCode.OK, "Update successfully", null, updateDTO);
            return new JsonResult(response);
        }

        [HttpGet]
        [Route("GetByIsImportant")]
        public async Task<JsonResult> GetReviewsByStatus(int userId,bool isImportant)
        {
            var reviewsDTO = await _reviewService.GetReviewsByStatus(userId, isImportant);
            if(reviewsDTO == null)
            {
                return new JsonResult(new ResponseDTO<ReviewDTO>(HttpStatusCode.NotFound, "Not Found", null, null));
            }
            var response = new ResponseDTO<List<ReviewDTO>>(HttpStatusCode.OK, "Get successfully", null, reviewsDTO);
            return new JsonResult(response);
        }
    }
}

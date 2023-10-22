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

        [HttpGet("{id:int}")]
        public async Task<JsonResult> Get(int id)
        {
            var review = await _reviewService.GetByIDAsync(id);
            var response = new ResponseDTO<ReviewDTO>(HttpStatusCode.OK, "Success", null, review);
            return new JsonResult(response);
        }
        [HttpPost]
        public async Task<JsonResult> Create([FromBody] AddReviewDTO addReviewDTO)
        {
            var createdReviewDTO = await _reviewService.CreateAsync(addReviewDTO);
            if (createdReviewDTO != null)
            {
                var response = new ResponseDTO<ReviewDTO>(HttpStatusCode.Created, "Created successfully", null, createdReviewDTO);
                return new JsonResult(response);
            }
            else
            {
                var response = new ResponseDTO<ReviewDTO>(HttpStatusCode.BadRequest, "Failed to create", null, null);
                return new JsonResult(response);
            }
        }
    }
}

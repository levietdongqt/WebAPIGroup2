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
    }
}

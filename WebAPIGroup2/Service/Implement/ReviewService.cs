using AutoMapper;
using WebAPIGroup2.Models.DTO;
using WebAPIGroup2.Models.POJO;
using WebAPIGroup2.Respository.Inteface;
using WebAPIGroup2.Service.Inteface;

namespace WebAPIGroup2.Service.Implement
{
    public class ReviewService : IReviewService
    {
        public readonly IReviewRepo _reviewRepo;
        public readonly IMapper _mapper;

        public ReviewService(IReviewRepo reviewRepo,IMapper mapper)
        {
            _reviewRepo = reviewRepo;
            _mapper = mapper;
        }

        public async Task<ReviewDTO> CreateAsync(AddReviewDTO addReviewDTO)
        {
            var review = _mapper.Map<Review>(addReviewDTO);
            var result = await _reviewRepo.InsertAsync(review);
            if (!result)
            {
                return null;
            }
            return _mapper.Map<ReviewDTO>(review);
        }
    }
}

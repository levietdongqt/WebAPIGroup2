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

        public async Task<ReviewDTO> DeleteAsync(int id)
        {
            var review = await _reviewRepo.GetByIDAsync(id);
            if (review == null)
            {
                return null;
            }
            var result = await _reviewRepo.DeleteAsync(review);
            if (!result)
            {
                return null;
            }
            return _mapper.Map<ReviewDTO>(review);
        }

        public async Task<List<ReviewDTO>> UpdateAllAsync(List<ReviewDTO> updateReviewsDTO)
        {
            List<Review> reviews = new List<Review>();
            foreach (var updateReview in updateReviewsDTO)
            {
                var review = await _reviewRepo.GetByIDAsync(updateReview.Id);
                if (review == null)
                {
                    return null;     
                }
                review.isImportant = updateReview.isImportant;
                review.ReviewDate = updateReview.ReviewDate;
                review.Rating = updateReview.Rating;
                review.TemplateId = updateReview.TemplateId;
                review.UserId = updateReview.UserId;
                review.Content = updateReview.Content;

                reviews.Add(review);
            }
            var result = await _reviewRepo.UpdateAllAsync(reviews);
            if (!result)
            {
                return null;
            }
            return _mapper.Map<List<ReviewDTO>>(reviews);
        }
    }
}

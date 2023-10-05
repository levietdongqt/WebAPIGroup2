using WebAPIGroup2.Models.DTO;

namespace WebAPIGroup2.Service.Inteface
{
    public interface IReviewService
    {
        Task<ReviewDTO> CreateAsync(AddReviewDTO addReviewDTO);
    }
}

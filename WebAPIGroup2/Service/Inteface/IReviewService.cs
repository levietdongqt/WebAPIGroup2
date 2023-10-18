using WebAPIGroup2.Models.DTO;

namespace WebAPIGroup2.Service.Inteface
{
    public interface IReviewService
    {
        Task<ReviewDTO> CreateAsync(AddReviewDTO addReviewDTO);

        Task<ReviewDTO> DeleteAsync(int id);

        Task<List<ReviewDTO>> UpdateAllAsync(List<ReviewDTO> updateReviewsDTO);
    }
}

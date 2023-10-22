using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using WebAPIGroup2.Models.DTO;
using WebAPIGroup2.Models.POJO;
using WebAPIGroup2.Respository.Implement;
using WebAPIGroup2.Respository.Inteface;
using WebAPIGroup2.Service.Inteface;

namespace WebAPIGroup2.Service.Implement
{
    public class FeedBackService : IFeedBackService
    {
        private readonly IFeedBackRepo _feedBackRepo;
        public readonly IUserRepo _userRepo;
        private readonly IMapper _mapper;

        public FeedBackService(IFeedBackRepo feedBackRepo,IMapper mapper, IUserRepo userRepo)
        {
            _feedBackRepo = feedBackRepo;
            _mapper = mapper;
            _userRepo = userRepo;
        }

        public Task<List<FeedBackDTO>> GetAllAsync(string? filterOn = null, string? filterQuery = null, string? sortBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 1000)
        {
            throw new NotImplementedException();
        }

        public async Task<List<FeedBackDTO>> GetFeedBackByStatus(int userId, bool isImportant)
        {
            var user = await _userRepo.GetByIDAsync(userId);
            if (user == null && user.FeedBacks.IsNullOrEmpty())
            {
                return null;
            }
            var isImportantFeedBacks = user.FeedBacks.Where(x => x.isImportant == isImportant).ToList();
            var feedbacksDTO = _mapper.Map<List<FeedBackDTO>>(isImportantFeedBacks);
            return feedbacksDTO;
        }

        public async Task<List<FeedBackDTO>> UpdateAllAsync(List<FeedBackDTO> updateFeedBackDTO)
        {
            List<FeedBack> feedbacks = new List<FeedBack>();
            foreach (var update in updateFeedBackDTO)
            {
                var feedback = await _feedBackRepo.GetByIDAsync(update.Id);
                if (feedback == null)
                {
                    return null;
                }
                feedback.isImportant = update.isImportant;
                feedback.Content = update.Content;
                feedback.FeedBackDate = update.FeedBackDate;
                feedback.Email = update.Email;
                feedback.UserId = update.UserId;
               

                feedbacks.Add(feedback);
            }
            var result = await _feedBackRepo.UpdateAllAsync(feedbacks);
            if (!result)
            {
                return null;
            }
            return _mapper.Map<List<FeedBackDTO>>(feedbacks);
        }

        public async Task<dynamic> GetFeedBackTake5News()
        {
            var feebbacks = await _feedBackRepo.GetFeedBackTake5News();
            if(feebbacks == null)
            {
                return null;
            }
            return feebbacks;
            
        }
    }
}

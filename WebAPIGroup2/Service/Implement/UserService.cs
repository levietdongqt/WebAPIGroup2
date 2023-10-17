using Microsoft.EntityFrameworkCore;
using WebAPIGroup2.Models;
using WebAPIGroup2.Models.DTO;
using WebAPIGroup2.Models.POJO;
using WebAPIGroup2.Respository.Implement;
using WebAPIGroup2.Respository.Inteface;
using WebAPIGroup2.Service.Inteface;
using System.Text.Json;
using AutoMapper;

namespace WebAPIGroup2.Service.Implement
{
    public class UserService : IUserService
    {
        private readonly IUserRepo _useRepo;
        private readonly IDeliveryInfoRepo _deliveryInfoRepo;
        private readonly IReviewRepo _reviewRepo;
        private readonly MyImageContext _context;
        private readonly IMapper _mapper;
        private readonly IFeedBackRepo _feedBackRepo;
        private readonly IUtilService _utilService;
        private readonly IWebHostEnvironment _webHostEnvironment;




        public UserService(MyImageContext context, IUserRepo userRepo, IDeliveryInfoRepo deliveryInfoRepo, IMapper mapper, IUtilService utilService, IWebHostEnvironment webHostEnvironment, IReviewRepo reviewRepo, IFeedBackRepo feedBackRepo)
        {
            _context = context;
            _deliveryInfoRepo = deliveryInfoRepo;
            _useRepo = userRepo;
            _mapper = mapper;
            _reviewRepo = reviewRepo;
            _feedBackRepo = feedBackRepo;
            _utilService = utilService;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<UserDTO> ChangePassword(AddUserDTO addUserDTO)
        {
            //var passwordHash = BCrypt.Net.BCrypt.HashPassword(userDTO.Password);
            //oldPassword = passwordHash;
            var existingUser = await _context.Users.SingleOrDefaultAsync(u => u.Id == addUserDTO.Id && u.Password == addUserDTO.oldPassword);
            if (existingUser == null)
            {
                return null;
            }

            existingUser.Password = addUserDTO.newPassword;
            var update = await _useRepo.UpdateAsync(existingUser);
            var userDTO = _mapper.Map<UserDTO>(existingUser);

            return userDTO;
        }

        public async Task<DeliveryInfoDTO> CreateDeliveryInfoOfUser(int userId, DeliveryInfoDTO deliveryInfoDTO)
        {
            var user = await _useRepo.GetByIDAsync(userId);
            if (user == null)
            {
                return null;
            }
            var deliveryInfo = _mapper.Map<DeliveryInfo>(deliveryInfoDTO);
            deliveryInfo.UserId = userId;
            var result = await _deliveryInfoRepo.InsertAsync(deliveryInfo);
            if (!result)
            {
                return null;
            }
            return _mapper.Map<DeliveryInfoDTO>(deliveryInfo);
        }
        

        public async Task<UserDTO> CreateUser(UserDTO userDTO)
        {
            var avatar = "Avatar/avatardf.jpg";
            var user = _mapper.Map<User>(userDTO);
            user.Status = UserStatus.Pending;
            user.Avatar = avatar;
            user.Role = UserRole.user;
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(userDTO.Password);
            user.Password = passwordHash;
            var success = await _useRepo.InsertAsync(user);
            if (success)
            {
                var createdUserDTO = _mapper.Map<UserDTO>(user);
                return createdUserDTO;
            }
            return null;

        }

        public async Task<IEnumerable<UserDTO>?> GetAllAsync(string? search, string? st, int page, int pageSize)
        {
            List<UserDTO> list = new List<UserDTO>();
            var users = await _useRepo.GetAllAsync();

            if (users != null)
            {
                if (!string.IsNullOrEmpty(st) && !string.IsNullOrEmpty(search))
                {
                    search = search.ToLower();

                    users = users.Where(sd =>
                        (sd.Status == st) &&
                        ((sd.Email != null && sd.Email.ToLower().Contains(search)) ||
                        (sd.Phone != null && sd.Phone.Contains(search))));
                }
                else if (!string.IsNullOrEmpty(st))
                {
                    users = users.Where(sd => sd.Status == st);
                }
                else if (!string.IsNullOrEmpty(search))
                {
                    search = search.ToLower();

                    users = users.Where(sd =>
                        (sd.Email != null && sd.Email.ToLower().Contains(search)) ||
                        (sd.Phone != null && sd.Phone.Contains(search)));
                }

                // Phân trang
                users = users.Skip((page - 1) * pageSize).Take(pageSize);

                list = _mapper.Map<List<UserDTO>>(users);
            }

            return list;
        }

        public async Task<List<DeliveryInfoDTO>> GetDeliveryInfoByUserIDAsync(int userId)
        {
            var user = await _useRepo.GetByIDAsync(userId);
            if (user == null)
            {
                return null;
            }
            var deliveryInfos = user.DeliveryInfos.ToList();
            return _mapper.Map<List<DeliveryInfoDTO>>(deliveryInfos);
        }

        public async Task<UserDTO> GetUserByIDAsync(int id)
        {
            var user = await _useRepo.GetByIDAsync(id);
            if (user == null) return null;

            var UserFullDto = _mapper.Map<UserDTO>(user);
            var temporaryDeliveryInfos = new List<DeliveryInfoDTO>();
            var temporaryReviews = new List<ReviewTempDTO>();
            var temporaryFeedBacks = new List<FeedBackDTO>();

            foreach (var item in UserFullDto.DeliveryInfos) 
            {
                var deliveryInfo = await _deliveryInfoRepo.GetByIDAsync(item.Id);
                var deliveryInfoDTO = _mapper.Map<DeliveryInfoDTO>(deliveryInfo);
                temporaryDeliveryInfos.Add(deliveryInfoDTO);
            }
            foreach (var item in UserFullDto.Reviews)
            {
                var review = await _reviewRepo.GetByIDAsync(item.Id);
                var reviewDTO = _mapper.Map<ReviewTempDTO>(review);
                temporaryReviews.Add(reviewDTO);
            }
            foreach (var item in UserFullDto.FeedBacks)
            {
                var feedback = await _feedBackRepo.GetByIDAsync(item.Id);
                var feedbackDTO = _mapper.Map<FeedBackDTO>(feedback);
                temporaryFeedBacks.Add(feedbackDTO);
            }
            
            UserFullDto.DeliveryInfos = temporaryDeliveryInfos;
            UserFullDto.Reviews = temporaryReviews;
            UserFullDto.FeedBacks = temporaryFeedBacks;

            return UserFullDto;

        }

        public async Task<UserDTO> UpdateConfirmEmailAsync(UserDTO userDTO)
        {
            var existingUser = await _context.Users.SingleOrDefaultAsync(u => u.Id == userDTO.Id);
            if (existingUser == null)
            {
                return null;
            }
            existingUser.Status = UserStatus.Enabled;
            existingUser.EmailConfirmed = true;
            var updated = await _useRepo.UpdateAsync(existingUser);
            if (!updated)
            {
                return null;
            }
            return _mapper.Map<UserDTO>(existingUser);


        }

        public async Task<string> SaveUploadedFile(IFormFile formFile)
        {
         
            var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetExtension(formFile.FileName)}";

            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Avatar", uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await formFile.CopyToAsync(stream);
            }


            return $"/Avatar/{uniqueFileName}";
        }

        public async Task<UserDTO> UpdateUser(AddUserDTO addUserDTO)
        {
            var existingUser = await _context.Users.SingleOrDefaultAsync(u => u.Id == addUserDTO.Id);

            if (existingUser != null)
            {
                existingUser.FullName = addUserDTO.FullName;
                existingUser.Address = addUserDTO.Address;
                existingUser.Phone = addUserDTO.Phone;
                existingUser.Role = addUserDTO.Role;
                existingUser.DateOfBirth = addUserDTO.DateOfBirth;
                existingUser.Status = addUserDTO.Status;
                existingUser.Gender = addUserDTO.Gender;

                if (addUserDTO.formFile != null)
                {
                    var avatar = await SaveUploadedFile(addUserDTO.formFile);
                    existingUser.Avatar =  avatar;
                }

                var update = await _useRepo.UpdateAsync(existingUser);
                if (!update)
                {
                    return null;
                }
            }

            var userDTO = _mapper.Map<UserDTO>(existingUser);
            return userDTO;
        }

        public async Task<UserDTO> PasswordRecovery(AddUserDTO addUserDTO)
        {
            var existingUser = await _context.Users.SingleOrDefaultAsync(u => u.Email == addUserDTO.Email);

            if (existingUser != null)
            {
                existingUser.Password = addUserDTO.Password;
             
                if (addUserDTO.formFile != null)
                {
                    var avatar = await SaveUploadedFile(addUserDTO.formFile);
                    existingUser.Avatar = avatar;
                }

                var update = await _useRepo.UpdateAsync(existingUser);
                if (!update)
                {
                    return null;
                }
            }

            var userDTO = _mapper.Map<UserDTO>(existingUser);
            return userDTO;
        }


        public async Task<UserDTO> GetUserByEmailAsync(string email)
        {
            var user = await _useRepo.GetUserByEmail(email);
            if (user == null)
            {
                return null;
            }
            var userDTO = _mapper.Map<UserDTO>(user);
            return userDTO;
        }
    }







}

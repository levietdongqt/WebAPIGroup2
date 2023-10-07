using AutoMapper;
using WebAPIGroup2.Models.DTO;
using WebAPIGroup2.Respository.Inteface;
using WebAPIGroup2.Service.Inteface;

namespace WebAPIGroup2.Service.Implement
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepo _categoryRepo;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepo categoryRepo,IMapper mapper)
        {
            _categoryRepo = categoryRepo;
            _mapper = mapper;
        }
        public async Task<List<CategoryDTO>> GetAll()
        {
            var categories = await _categoryRepo.GetAll();
            return _mapper.Map<List<CategoryDTO>>(categories);
        }
    }
}

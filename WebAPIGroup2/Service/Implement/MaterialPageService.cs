using AutoMapper;
using WebAPIGroup2.Models.DTO;
using WebAPIGroup2.Models.POJO;
using WebAPIGroup2.Respository.Inteface;
using WebAPIGroup2.Service.Inteface;

namespace WebAPIGroup2.Service.Implement
{
    public class MaterialPageService : IMaterialPageService
    {
        public readonly IMaterialPageRepo _materialPageRepo;
        public readonly IMapper _mapper;

        public MaterialPageService(IMaterialPageRepo materialPageRepo,IMapper mapper)
        {
            _materialPageRepo = materialPageRepo;
            _mapper = mapper;
        }
        public async Task<MaterialPageDTO> CreateAsync(MaterialPageDTO materialPageDTO)
        {
           var materialPage = _mapper.Map<MaterialPage>(materialPageDTO);
           var result = await _materialPageRepo.InsertAsync(materialPage);
            if (!result)
            {
                return null;
            }
            return _mapper.Map<MaterialPageDTO>(materialPage);
        }

        public async Task<List<MaterialPageDTO>> GetAllAsync()
        {
            var materialPages = await _materialPageRepo.GetAllAsync();
            return _mapper.Map<List<MaterialPageDTO>>(materialPages);
        }

        public async Task<MaterialPageDTO> GetByIdAsync(int id)
        {
            var materialPage = await _materialPageRepo.GetByIDAsync(id);
            if(materialPage == null)
            {
                return null;
            }
            return _mapper.Map<MaterialPageDTO>(materialPage);
        }

        public async Task<MaterialPageDTO> UpdateAsync(int id, MaterialPageDTO materialPageDTO)
        {
            var materialPage = await _materialPageRepo.GetByIDAsync(id);
            if(materialPage == null)
            {
                return null;
            }
            materialPage.PricePerInch = materialPageDTO.PricePerInch;
            materialPage.InchSold = materialPageDTO.InchSold;
            materialPage.Status = materialPageDTO.Status;
            materialPage.Name = materialPageDTO.Name;
            materialPage.Description = materialPageDTO.Description;
            var result = await _materialPageRepo.UpdateAsync(materialPage);
            if (!result)
            {
                return null;
            }
            return _mapper.Map<MaterialPageDTO>(materialPage);
        }
    }
}

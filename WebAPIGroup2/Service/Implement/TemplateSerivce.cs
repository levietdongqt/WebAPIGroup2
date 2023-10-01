using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebAPIGroup2.Models;
using WebAPIGroup2.Models.DTO;
using WebAPIGroup2.Models.POJO;
using WebAPIGroup2.Respository.Inteface;
using WebAPIGroup2.Service.Inteface;

namespace WebAPIGroup2.Service.Implement;

public class TemplateSerivce : ITemplateService
{
    private readonly ITemplateRepo _templateRepo;
    private readonly IMapper _mapper;
    private readonly MyImageContext _context;
    private readonly ICategoryTemplateRepo _categoryTemplateRepo;
    public TemplateSerivce(ITemplateRepo templateRepo,ICategoryTemplateRepo categoryTemplateRepo ,IMapper mapper,MyImageContext context)
    {
        _templateRepo = templateRepo;
        _mapper = mapper;
        _context = context;
        _categoryTemplateRepo = categoryTemplateRepo;
    }
    public async Task<IEnumerable<TemplateDTO>?> GetAllAsync()
    {
        var getAll = await _templateRepo.GetAllAsync();
        return _mapper.Map<IEnumerable<TemplateDTO>>(getAll);
    }

    public async Task<TemplateWithCategoryDTO?> GetTemplateById(int id)
    {
        var getById = await _templateRepo.GetTemplateByID(id);
        return _mapper.Map<TemplateWithCategoryDTO>(getById);
    }
    
    public async Task<TemplateDTO> CreateTemplate(TemplateDTO template)
    {
        var mapTemplate = _mapper.Map<Template>(template);
        var insertTemplate = await _templateRepo.InsertAsync(mapTemplate);
        if (insertTemplate)
        {
            foreach (var id in template.CategoryIds)
            {
                var categoryTemplate = new CategoryTemplate()
                {
                    CategoryId = id,
                    TemplateId = mapTemplate.Id
                };
                await _categoryTemplateRepo.InsertAsync(categoryTemplate);
            }
            return _mapper.Map<TemplateDTO>(mapTemplate);
        }

        return null;
    }

    public async Task<bool> updateTemplate(TemplateDTO template)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> deleteTemplate(int id)
    {
        var find = await _context.Templates.SingleOrDefaultAsync(s=> s.Id==id);
        if (find == null)
        {
            return false;
        }
        var delete = await _templateRepo.DeleteAsync(find);
        return delete;
    }
}
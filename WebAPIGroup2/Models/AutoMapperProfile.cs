using AutoMapper;
using WebAPIGroup2.Models.DTO;
using WebAPIGroup2.Models.POJO;

namespace WebAPIGroup2.Models
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            //User
            CreateMap<User, UserDTO>()
                .ForMember(t => t.Password, option => option.Ignore()).ReverseMap();

            //Template
            CreateMap<Template,TemplateDTO>().ReverseMap();
            CreateMap<Template, AddTemplateDTO>().ReverseMap();


            //DescriptionTemplate
            CreateMap<DescriptionTemplate,DescriptionTemplateDTO>().ReverseMap();

            //Template Image
            CreateMap<TemplateImage,TemplateImageDTO>().ReverseMap();
        }
    }
}

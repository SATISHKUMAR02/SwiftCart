using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using eCommerce.Core.DTO;
using eCommerce.Core.Entities;

namespace eCommerce.Core.Mapping
{
    public class ApplicationUserToUserDtoMappingProfile : Profile
    {
        public ApplicationUserToUserDtoMappingProfile()
        {
            CreateMap<ApplicationUser, UserDTO>().
                ForMember(dest => dest.UserID, opt => opt.MapFrom(src => src.UserID))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.PersonName, opt => opt.MapFrom(src => src.PersonName))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender));
            
        }
    }
}

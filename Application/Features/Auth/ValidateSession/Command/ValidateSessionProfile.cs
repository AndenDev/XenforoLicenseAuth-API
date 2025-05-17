using AutoMapper;
using Domain.Entities;

namespace Application.Features.Auth.ValidateSession.Command
{
    public class ValidateSessionProfile : Profile
    {
        public ValidateSessionProfile()
        {
            CreateMap<User, ValidateSessionViewModel.SessionUserViewModel>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.UserGroupId, opt => opt.MapFrom(src => src.UserGroupId))
                .ForMember(dest => dest.UserGroupName, opt => opt.MapFrom(src => src.UserGroup.Title));
        }
    }
}

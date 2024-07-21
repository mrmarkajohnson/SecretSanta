using Application.Santa.Areas.Account.BaseModels;
using AutoMapper;
using Data.Entities.Shared;

namespace Application.Santa.Areas.Account.Mapping;

public class UserMappingProfile : Profile
{
	public UserMappingProfile()
	{
		CreateMap<Global_User, SantaUser>()
			.ForMember(dest => dest.IdentificationHashed, opt => opt.MapFrom(src => true));
	}
}

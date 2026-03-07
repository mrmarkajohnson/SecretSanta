using Application.Areas.Home.ViewModels;
using AutoMapper;

namespace Application.Areas.Home.Mapping;

public sealed class HomeMappingProfile : Profile
{
	public HomeMappingProfile()
	{
		CreateMap<HomeVm, HomePageVm>();
	}
}

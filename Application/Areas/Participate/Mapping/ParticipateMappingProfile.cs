using Application.Areas.Participate.BaseModels;
using AutoMapper;
using Global.Abstractions.Areas.Participate;
using static Global.Settings.GiftingGroupSettings;

namespace Application.Areas.Participate.Mapping;

public class ParticipateMappingProfile : Profile
{
	public ParticipateMappingProfile()
	{
        CreateMap<Santa_GiftingGroupUser, UserGiftingGroupYear>()
            .ForMember(dest => dest.GiftingGroupKey, opt => opt.MapFrom(src => src.GiftingGroupKey))
            .ForMember(dest => dest.GiftingGroupName, opt => opt.MapFrom(src => src.GiftingGroup.Name))
            .ForMember(dest => dest.MemberStatus, opt => opt.MapFrom(src => src.GroupAdmin ? GroupMemberStatus.Admin : GroupMemberStatus.Joined))
            .ForMember(dest => dest.Included, opt => opt.Ignore())
            .ForMember(dest => dest.Recipient, opt => opt.Ignore())
            .ForMember(dest => dest.CalendarYear, opt => opt.MapFrom(src => DateTime.Today.Year))
            .ForMember(dest => dest.Limit, opt => opt.MapFrom(src => src.GiftingGroup.Years
                .Where(x => x.CalendarYear == DateTime.Today.Year).Select(y => y.Limit).FirstOrDefault()))
            .ForMember(dest => dest.CurrencyCode, opt => opt.MapFrom(src => src.GiftingGroup.Years
                .Where(x => x.CalendarYear == DateTime.Today.Year).Select(y => y.CurrencyCode).FirstOrDefault()
                    ?? src.GiftingGroup.CurrencyCodeOverride))
            .ForMember(dest => dest.CurrencySymbol, opt => opt.MapFrom(src => src.GiftingGroup.Years
                .Where(x => x.CalendarYear == DateTime.Today.Year).Select(y => y.CurrencySymbol).FirstOrDefault()
                    ?? src.GiftingGroup.CurrencySymbolOverride));
        CreateMap<Santa_GiftingGroupUser, IUserGiftingGroupYear>().As<UserGiftingGroupYear>();

        CreateMap<Santa_GiftingGroupApplication, UserGiftingGroupYear>()
            .ForMember(dest => dest.GiftingGroupKey, opt => opt.MapFrom(src => src.GiftingGroupKey))
            .ForMember(dest => dest.GiftingGroupName, opt => opt.MapFrom(src => src.GiftingGroup.Name))
            .ForMember(dest => dest.MemberStatus, opt => opt.MapFrom(src => GroupMemberStatus.Applied))
            .ForMember(dest => dest.Included, opt => opt.MapFrom(src => false))
            .ForMember(dest => dest.Recipient, opt => opt.Ignore())
            .ForMember(dest => dest.CalendarYear, opt => opt.MapFrom(src => DateTime.Today.Year))
            .ForMember(dest => dest.Limit, opt => opt.MapFrom(src => (decimal?)null))
            .ForMember(dest => dest.CurrencyCode, opt => opt.MapFrom(src => "N/A"))
            .ForMember(dest => dest.CurrencySymbol, opt => opt.MapFrom(src => "N/A"));
        CreateMap<Santa_GiftingGroupApplication, IUserGiftingGroupYear>().As<UserGiftingGroupYear>(); 
        
        CreateMap<IUserGiftingGroupYear, ManageUserGiftingGroupYear>();
        CreateMap<IUserGiftingGroupYear, IManageUserGiftingGroupYear>().As<ManageUserGiftingGroupYear>();
    }
}

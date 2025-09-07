using Application.Areas.Account.ViewModels;
using AutoMapper;
using Global.Abstractions.Areas.Account;

namespace Application.Areas.Account.Mapping;

public sealed class AccountVmMappingProfile : Profile
{
    public AccountVmMappingProfile()
    {
        CreateMap<ISecurityQuestions, SetSecurityQuestionsVm>()
            .ForMember(dest => dest.Greeting, opt =>
            {
                opt.Condition(src => !string.IsNullOrEmpty(src.Greeting));
                opt.MapFrom(src => src.Greeting);
            })
            .ForMember(dest => dest.SecurityAnswer1, opt => opt.Ignore())
            .ForMember(dest => dest.SecurityAnswer2, opt => opt.Ignore())
            .ForMember(dest => dest.Update, opt => opt.MapFrom(src => src.SecurityQuestionsSet));

        CreateMap<ISantaUser, UpdateDetailsVm>()
            .ForMember(dest => dest.CurrentPassword, opt => opt.Ignore())
            .ForMember(dest => dest.Greeting, opt => opt.Ignore());

        CreateMap<IUserEmailDetails, EmailPreferencesVm>();
    }
}

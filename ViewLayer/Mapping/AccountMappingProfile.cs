using AutoMapper;
using Global.Abstractions.Global.Account;
using Global.Abstractions.Santa.Areas.Account;
using ViewLayer.Models.Account;

namespace ViewLayer.Mapping;

public class AccountMappingProfile : Profile
{
    public AccountMappingProfile()
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
    }
}

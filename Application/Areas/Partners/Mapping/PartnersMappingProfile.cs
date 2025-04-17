using Application.Areas.Partners.BaseModels;
using Application.Areas.Partners.Predicates;
using AutoMapper;
using static Global.Settings.PartnerSettings;

namespace Application.Areas.Partners.Mapping;

public sealed class PartnersMappingProfile : Profile
{
    public PartnersMappingProfile()
    {
        CreateMap<Santa_PartnerLink, SuggestedRelationship>()
            .ForMember(dest => dest.PartnerLinkKey, opt => opt.MapFrom(src => src.PartnerLinkKey))
            .ForMember(dest => dest.Partner, opt => opt.MapFrom(src => src.ConfirmingSantaUser.GlobalUser))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src =>
                src.SuggestedByIgnoreOld ? RelationshipStatus.IgnoreOld // show as ignored even if the other person hasn't confirmed
                : src.RelationshipEnded != null ? (src.Confirmed == true ? RelationshipStatus.Ended : RelationshipStatus.EndedBeforeConfirmation)
                : src.Confirmed == true ? RelationshipStatus.Active
                : src.Confirmed == null ? RelationshipStatus.ToBeConfirmed
                : RelationshipStatus.Avoid))
            .ForMember(dest => dest.SharedGroupNames, opt =>
                opt.MapFrom(RelationshipPredicates.RelationshipSharedGroupNames()));
        //.ForMember(dest => dest.SuggestedByCurrentUser, opt => opt.MapFrom(src => true)); // set automatically on the class

        CreateMap<Santa_PartnerLink, ConfirmingRelationship>()
            .ForMember(dest => dest.PartnerLinkKey, opt => opt.MapFrom(src => src.PartnerLinkKey))
            .ForMember(dest => dest.Partner, opt => opt.MapFrom(src => src.SuggestedBySantaUser.GlobalUser))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src =>
                src.ConfirmingUserIgnore ? RelationshipStatus.IgnoreOld // show as ignored even if the other person hasn't confirmed
                : src.RelationshipEnded != null ? (src.Confirmed == true ? RelationshipStatus.Ended : RelationshipStatus.EndedBeforeConfirmation)
                : src.Confirmed == true ? RelationshipStatus.Active
                : src.Confirmed == null ? RelationshipStatus.ToConfirm
                : RelationshipStatus.Avoid))
            .ForMember(dest => dest.SharedGroupNames, opt =>
                opt.MapFrom(RelationshipPredicates.RelationshipSharedGroupNames()));
            //.ForMember(dest => dest.SuggestedByCurrentUser, opt => opt.MapFrom(src => false)); // set automatically on the class

    }
}

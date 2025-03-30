using Application.Areas.Partners.BaseModels;
using Application.Areas.Partners.Predicates;
using AutoMapper;
using static Global.Settings.PartnerSettings;

namespace Application.Areas.Partners.Mapping;

public class PartnersMappingProfile : Profile
{
    public PartnersMappingProfile()
    {
        CreateMap<Santa_PartnerLink, SuggestedRelationship>()
            .ForMember(dest => dest.PartnerLinkKey, opt => opt.MapFrom(src => src.PartnerLinkKey))
            .ForMember(dest => dest.Partner, opt => opt.MapFrom(src => src.ConfirmingSantaUser.GlobalUser))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src =>
                src.SuggestedByIgnoreOld ? RelationshipStatus.IgnoreOld // show as ignored even if the other person hasn't confirmed
                : src.RelationshipEnded != null ? (src.Confirmed ? RelationshipStatus.Ended : RelationshipStatus.EndedBeforeConfirmation)
                : src.Confirmed ? RelationshipStatus.Active
                : RelationshipStatus.ToBeConfirmed))
            .ForMember(dest => dest.SharedGroupNames, opt =>
                opt.MapFrom(RelationshipPredicates.RelationshipSharedGroupNames()));

        CreateMap<Santa_PartnerLink, ConfirmingRelationship>()
            .ForMember(dest => dest.PartnerLinkKey, opt => opt.MapFrom(src => src.PartnerLinkKey))
            .ForMember(dest => dest.Partner, opt => opt.MapFrom(src => src.SuggestedBySantaUser.GlobalUser))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src =>
                src.ConfirmedByIgnoreOld ? RelationshipStatus.IgnoreOld // show as ignored even if the other person hasn't confirmed
                : src.RelationshipEnded != null ? (src.Confirmed ? RelationshipStatus.Ended : RelationshipStatus.EndedBeforeConfirmation)
                : src.Confirmed ? RelationshipStatus.Active
                : RelationshipStatus.ToConfirm))
            .ForMember(dest => dest.SharedGroupNames, opt =>
                opt.MapFrom(RelationshipPredicates.RelationshipSharedGroupNames()));

    }
}

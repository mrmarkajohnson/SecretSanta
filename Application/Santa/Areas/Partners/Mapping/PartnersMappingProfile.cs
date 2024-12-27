using Application.Santa.Areas.Partners.BaseModels;
using Application.Santa.Areas.Partners.Predicates;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Global.Settings.PartnerSettings;

namespace Application.Santa.Areas.Partners.Mapping;

public class PartnersMappingProfile : Profile
{
	public PartnersMappingProfile()
    {
        CreateMap<Santa_PartnerLink, SuggestedRelationship>()
            .ForMember(dest => dest.PartnerLinkId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Partner, opt => opt.MapFrom(src => src.ConfirmingSantaUser.GlobalUser))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src =>
                src.SuggestedByIgnoreOld && src.ConfirmedByIgnoreOld ? RelationshipStatus.IgnoreOld
                : src.RelationshipEnded != null ? RelationshipStatus.Ended
                : src.Confirmed ? RelationshipStatus.Active
                : RelationshipStatus.ToBeConfirmed))
            .ForMember(dest => dest.SharedGroupNames, opt => 
                opt.MapFrom(RelationshipPredicates.RelationshipSharedGroupNames()));

        CreateMap<Santa_PartnerLink, ConfirmingRelationship>()
            .ForMember(dest => dest.PartnerLinkId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Partner, opt => opt.MapFrom(src => src.SuggestedBySantaUser.GlobalUser))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src =>
                src.SuggestedByIgnoreOld && src.ConfirmedByIgnoreOld ? RelationshipStatus.IgnoreOld
                : src.RelationshipEnded != null ? RelationshipStatus.Ended
                : src.Confirmed ? RelationshipStatus.Active
                : RelationshipStatus.ToConfirm))
            .ForMember(dest => dest.SharedGroupNames, opt =>
                opt.MapFrom(RelationshipPredicates.RelationshipSharedGroupNames()));

    }
}

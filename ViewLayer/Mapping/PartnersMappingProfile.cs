using AutoMapper;
using Global.Abstractions.Areas.Partners;
using ViewLayer.Models.Partners;

namespace ViewLayer.Mapping;

public sealed class PartnersMappingProfile : Profile
{
    public PartnersMappingProfile()
    {
        CreateMap<IRelationships, RelationshipsVm>();

        CreateMap<IRelationship, ManageRelationshipVm>()
            .ForMember(dest => dest.Partner, opt => opt.MapFrom(src => src.Partner)) // just in case
            .ForMember(dest => dest.OriginalStatus, opt => opt.MapFrom(src => src.Status));

        CreateMap<IRelationship, RelationshipVm>()
            .ForMember(dest => dest.Partner, opt => opt.MapFrom(src => src.Partner)); // just in case
    }
}

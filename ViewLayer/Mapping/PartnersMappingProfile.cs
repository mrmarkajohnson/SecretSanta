using AutoMapper;
using Global.Abstractions.Areas.Partners;
using ViewLayer.Models.Partners;

namespace ViewLayer.Mapping;

public class PartnersMappingProfile : Profile
{
	public PartnersMappingProfile()
	{
        CreateMap<IRelationships, RelationshipsVm>();
        CreateMap<IRelationship, RelationshipVm>()
            .ForMember(dest => dest.Partner, opt => opt.MapFrom(src => src.Partner)); // just in case
    }
}

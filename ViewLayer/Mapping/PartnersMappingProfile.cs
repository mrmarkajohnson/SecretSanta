using AutoMapper;
using Global.Abstractions.Global.Partners;
using ViewLayer.Models.Partners;

namespace ViewLayer.Mapping;

public class PartnersMappingProfile : Profile
{
	public PartnersMappingProfile()
	{
        CreateMap<IRelationships, RelationshipsVm>();
        CreateMap<IRelationship, RelationshipVm>();
    }
}

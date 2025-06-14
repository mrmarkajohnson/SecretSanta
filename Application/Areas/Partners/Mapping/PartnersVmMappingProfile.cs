using Application.Areas.Partners.ViewModels;
using AutoMapper;
using Global.Abstractions.Areas.Partners;

namespace Application.Areas.Partners.Mapping;

public sealed class PartnersVmMappingProfile : Profile
{
    public PartnersVmMappingProfile()
    {
        CreateMap<IRelationships, RelationshipsVm>();

        CreateMap<IRelationship, ManageRelationshipVm>()
            .ForMember(dest => dest.Partner, opt => opt.MapFrom(src => src.Partner)) // just in case
            .ForMember(dest => dest.OriginalStatus, opt => opt.MapFrom(src => src.Status));

        CreateMap<IRelationship, RelationshipVm>()
            .ForMember(dest => dest.Partner, opt => opt.MapFrom(src => src.Partner)); // just in case
    }
}

using Application.Shared.BaseModels;
using AutoMapper;

namespace Application.Areas.Shared.Mapping;

public sealed class SharedMappingProfile : Profile
{
    public SharedMappingProfile()
    {
        IList<string> GroupNames = new List<string>();

        CreateMap<Global_User, VisibleUser>()
            .IncludeBase<Global_User, UserNamesBase>()
            .ForMember(dest => dest.SharedGroupNames, opt => opt.MapFrom(src =>
                src.SantaUser.GiftingGroupLinks
                    .Where(x => x.DateArchived == null && x.DateDeleted == null)
                    .Where(x => x.GiftingGroup.DateArchived == null && x.GiftingGroup.DateDeleted == null)
                    .Where(x => GroupNames.Contains(x.GiftingGroup.Name))
                    .Select(x => x.GiftingGroup.Name)));
        CreateMap<Global_User, IVisibleUser>().As<VisibleUser>();
    }
}

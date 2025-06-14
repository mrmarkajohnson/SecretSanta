using AutoMapper;
using Global.Abstractions.Areas.Messages;
using ViewModels.Models.Messages;

namespace ViewModels.Mapping;

public sealed class MessageMappingProfile : Profile
{
    public MessageMappingProfile()
    {
        CreateMap<IReadMessage, ReadMessageVm>();
    }
}

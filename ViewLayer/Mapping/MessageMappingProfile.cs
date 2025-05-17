using AutoMapper;
using Global.Abstractions.Areas.Messages;
using ViewLayer.Models.Messages;

namespace ViewLayer.Mapping;

public sealed class MessageMappingProfile : Profile
{
    public MessageMappingProfile()
    {
        CreateMap<IReadMessage, ReadMessageVm>();
    }
}

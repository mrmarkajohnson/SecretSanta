using Application.Areas.Messages.ViewModels;
using AutoMapper;
using Global.Abstractions.Areas.Messages;

namespace Application.Areas.Messages.Mapping;

public sealed class MessageVmMappingProfile : Profile
{
    public MessageVmMappingProfile()
    {
        CreateMap<IReadMessage, ReadMessageVm>();
    }
}

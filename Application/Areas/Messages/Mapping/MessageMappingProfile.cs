using Application.Areas.Messages.BaseModels;
using Application.Shared.BaseModels;
using AutoMapper;
using Global.Abstractions.Global.Messages;

namespace Application.Areas.Messages.Mapping;

public sealed class MessageMappingProfile : Profile
{
	public MessageMappingProfile()
	{
        CreateMap<Santa_MessageRecipient, ReadMessage>()
            .IncludeMembers(src => src.Message)
            .ForMember(dest => dest.MessageRecipientKey, opt => opt.MapFrom(src => src.MessageRecipientKey))
            .ForMember(dest => dest.Read, opt => opt.MapFrom(src => src.Read));
        CreateMap<Santa_MessageRecipient, IReadMessage>().As<ReadMessage>();

        CreateMap<Santa_Message, ReadMessage>()
            .ForMember(dest => dest.MessageKey, opt => opt.MapFrom(src => src.MessageKey))
            .ForMember(dest => dest.Sent, opt => opt.MapFrom(src => src.DateCreated))
            .ForMember(dest => dest.Sender, opt =>
            {
                opt.Condition(src => (src.ShowAsFromSanta == false));
                opt.MapFrom(src => src.Sender.GlobalUser);
            })
            .ForMember(dest => dest.ShowAsFromSanta, opt => opt.MapFrom(src => src.ShowAsFromSanta))
            .ForMember(dest => dest.RecipientTypes, opt => opt.MapFrom(src => src.RecipientTypes))
            .ForMember(dest => dest.HeaderText, opt => opt.MapFrom(src => src.HeaderText))
            .ForMember(dest => dest.MessageText, opt => opt.MapFrom(src => src.MessageText))
            .ForMember(dest => dest.Important, opt => opt.MapFrom(src => src.Important));
        CreateMap<Santa_Message, IReadMessage>().As<ReadMessage>();
    }
}

using Application.Areas.Messages.BaseModels;
using AutoMapper;
using Global.Abstractions.Areas.Messages;

namespace Application.Areas.Messages.Mapping;

public sealed class MessageMappingProfile : Profile
{
    public MessageMappingProfile()
    {
        int? CurrentSantaUserKey = null;
        
        CreateMap<Santa_MessageRecipient, ReadMessage>()
            .IncludeMembers(src => src.Message)
            .ForMember(dest => dest.MessageRecipientKey, opt => opt.MapFrom(src => src.MessageRecipientKey))
            .ForMember(dest => dest.Read, opt => opt.MapFrom(src => src.Read));
        CreateMap<Santa_MessageRecipient, IReadMessage>().As<ReadMessage>();

        CreateMap<Santa_MessageRecipient, SantaMessage>()
            .IncludeMembers(src => src.Message)
            .ForMember(dest => dest.MessageRecipientKey, opt => opt.MapFrom(src => src.MessageRecipientKey))
            .ForMember(dest => dest.Read, opt => opt.MapFrom(src => src.Read));
        CreateMap<Santa_MessageRecipient, ISantaMessage>().As<SantaMessage>();

        CreateMap<Santa_Message, ReadMessage>()
            .IncludeBase<Santa_Message, SantaMessage>()            
            .ForMember(dest => dest.GiftingGroupKey, opt => opt.MapFrom(src => src.GiftingGroupYear == null 
                ? (int?)null 
                : src.GiftingGroupYear.GiftingGroup.GiftingGroupKey));
        CreateMap<Santa_Message, IReadMessage>().As<ReadMessage>();

        CreateMap<Santa_Message, SantaMessage>()
            .IncludeBase<Santa_Message, SantaMessageBase>()
            .ForMember(dest => dest.Sender, opt =>
            {
                opt.Condition(src => (src.ShowAsFromSanta == false));
                opt.MapFrom(src => src.Sender.GlobalUser);
            })
            .ForMember(dest => dest.IsSentMessage, opt => opt.MapFrom(src => src.SenderKey == CurrentSantaUserKey));
        CreateMap<Santa_Message, ISantaMessage>().As<SantaMessage>();

        CreateMap<Santa_Message, SantaMessageBase>()
            .ForMember(dest => dest.MessageKey, opt => opt.MapFrom(src => src.MessageKey))
            .ForMember(dest => dest.Sent, opt => opt.MapFrom(src => src.DateCreated))
            .ForMember(dest => dest.ShowAsFromSanta, opt => opt.MapFrom(src => src.ShowAsFromSanta))
            .ForMember(dest => dest.RecipientType, opt => opt.MapFrom(src => src.RecipientType))
            .ForMember(dest => dest.HeaderText, opt => opt.MapFrom(src => src.HeaderText))
            .ForMember(dest => dest.MessageText, opt => opt.MapFrom(src => src.MessageText))
            .ForMember(dest => dest.GroupName, opt => opt.MapFrom(src => src.GiftingGroupYear == null
                ? (string?)null
                : src.GiftingGroupYear.GiftingGroup.Name))
            .ForMember(dest => dest.Important, opt => opt.MapFrom(src => src.Important))
            .ForMember(dest => dest.CanReply, opt => opt.MapFrom(src => src.CanReply));
        CreateMap<Santa_Message, ISantaMessageBase>().As<SantaMessageBase>();
    }
}

using Application.Areas.Messages.BaseModels;
using AutoMapper;
using Global.Abstractions.Areas.Messages;
using System.Linq.Expressions;
using static Global.Settings.MessageSettings;

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
            .ForMember(dest => dest.Sender, opt => opt.MapFrom(src => src.ShowAsFromSanta ? null : src.Sender));
        CreateMap<Santa_Message, ISantaMessage>().As<SantaMessage>();

        CreateMap<Santa_Message, SantaMessageBase>()
            .ForMember(dest => dest.MessageKey, opt => opt.MapFrom(src => src.MessageKey))
            .ForMember(dest => dest.Sent, opt => opt.MapFrom(src => src.DateCreated))
            .ForMember(dest => dest.ShowAsFromSanta, opt => opt.MapFrom(src => src.ShowAsFromSanta
                || src.RecipientType == MessageRecipientType.GiftRecipient))
            .ForMember(dest => dest.RecipientType, opt => opt.MapFrom(src => src.RecipientType))
            .ForMember(dest => dest.UseSpecificRecipient, opt => opt.MapFrom(src => src.Recipients.Count() == 1
                && SpecificRecipientTypes.Contains(src.RecipientType)))
            .ForMember(dest => dest.SpecificRecipient, opt => opt.MapFrom(src => src.Recipients.Count() == 1
                ? src.Recipients.First().RecipientSantaUser
                : src.Sender)) // we have to map from something, so this is handled in the DTOs when required
            .ForMember(dest => dest.HeaderText, opt => opt.MapFrom(src => src.HeaderText))
            .ForMember(dest => dest.MessageText, opt => opt.MapFrom(src => src.MessageText))
            .ForMember(dest => dest.GroupName, opt => opt.MapFrom(src => src.GiftingGroupYear == null
                ? (string?)null
                : src.GiftingGroupYear.GiftingGroup.Name))
            .ForMember(dest => dest.ReplyTo, opt => opt.MapFrom(src => src.ReplyToMessage == null
                ? src // we have to map from something, so this is handled in the DTOs when required
                : (src.ReplyToMessage.SenderKey != CurrentSantaUserKey || src.OriginalMessage == null
                    ? src.ReplyToMessage
                    : src.OriginalMessage)))
            .ForMember(dest => dest.ReplyToName, opt => opt.Ignore())
            .ForMember(dest => dest.Important, opt => opt.MapFrom(src => src.Important))
            .ForMember(dest => dest.CanReply, opt => opt.MapFrom(src => src.CanReply))
            .ForMember(dest => dest.IsSentMessage, opt => opt.MapFrom(src => src.SenderKey == CurrentSantaUserKey));
        CreateMap<Santa_Message, ISantaMessageBase>().As<SantaMessageBase>();

        CreateMap<Santa_Message, SentMessage>()
            .IncludeBase<Santa_Message, SantaMessageBase>()            
            .ForMember(dest => dest.CanReply, opt => opt.MapFrom(src => true));
        CreateMap<Santa_Message, ISentMessage>().As<SentMessage>();
    }
}

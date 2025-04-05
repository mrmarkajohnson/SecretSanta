using AutoMapper;
using Global.Abstractions.Global.Messages;
using ViewLayer.Models.Messages;

namespace ViewLayer.Mapping;

public class MessageMappingProfile : Profile
{
	public MessageMappingProfile()
	{
		CreateMap<IReadMessage, ReadMessageVm>();
	}
}

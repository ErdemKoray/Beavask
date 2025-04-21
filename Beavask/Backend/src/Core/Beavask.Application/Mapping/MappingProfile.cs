using AutoMapper;
using Beavask.Application.DTOs.Event;
using Beavask.Application.DTOs.UserContact;
using Beavask.Domain.Entities.Base;

namespace Beavask.Application.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        //UserContact Entity
        CreateMap<UserContact, UserContactDto>().ReverseMap();
        CreateMap<UserContactCreateDto, UserContact>();

        //Event Entity
        CreateMap<EventCreateDto, Event>();
        CreateMap<EventUpdateDto, Event>();
        CreateMap<Event, EventDto>().ReverseMap();
    }
}

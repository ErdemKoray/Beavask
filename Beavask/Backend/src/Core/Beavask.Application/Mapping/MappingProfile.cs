using AutoMapper;
using Beavask.Application.DTOs.UserContact;
using Beavask.Domain.Entities.Base;

namespace Beavask.Application.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<UserContact, UserContactDto>().ReverseMap();
        CreateMap<UserContactCreateDto, UserContact>();
    }
}

using AutoMapper;
using Beavask.Application.DTOs.Company;
using Beavask.Application.DTOs.Event;
using Beavask.Application.DTOs.LogDtos;
using Beavask.Application.DTOs.Message;
using Beavask.Application.DTOs.NotificationDtos;
using Beavask.Application.DTOs.Permission;
using Beavask.Application.DTOs.Problem;
using Beavask.Application.DTOs.Role;
using Beavask.Application.DTOs.RolePermission;
using Beavask.Application.DTOs.Team;
using Beavask.Application.DTOs.UserContact;
using Beavask.Application.DTOs.UserRole;
using Beavask.Domain.Entities.Base;
using Beavask.Domain.Entities.Join;

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
        
        // Team Entity
        CreateMap<Team, TeamDto>().ReverseMap();
        CreateMap<TeamCreateDto, Team>();
        CreateMap<TeamUpdateDto, Team>();

        //Problem Entity
        CreateMap<Problem, ProblemDto>().ReverseMap();
        CreateMap<ProblemCreateDto, Problem>();
        CreateMap<ProblemUpdateDto, Problem>();

        //Company Entity
        CreateMap<Company, CompanyDto>();
        CreateMap<CompanyCreateDto, Company>();
        CreateMap<CompanyUpdateDto, Company>();

        //Log Entity
        CreateMap<Log, LogDto>().ReverseMap();
        CreateMap<LogCreateDto, Log>();      

        //Notification Entity
        CreateMap<NotificationCreateDto, Notification>();
        CreateMap<Notification, NotificationDto>();

        //Message Entity
        CreateMap<MessageCreateDto, Message>();
        CreateMap<Message, MessageDto>();

        //Role Entity
        CreateMap<RoleCreateDto, Role>().ReverseMap();
        CreateMap<RoleUpdateDto, Role>().ReverseMap();
        CreateMap<RoleDto, Role>().ReverseMap();

        //Permission Entity
        CreateMap<PermissionCreateDto, Permission>().ReverseMap();
        CreateMap<PermissionUpdateDto, Permission>().ReverseMap();
        CreateMap<PermissionDto, Permission>().ReverseMap();

        //RolePermission Entity
        CreateMap<RolePermissionCreateDto, RolePermission>();
        CreateMap<RolePermission, RolePermissionDto>();

        //UserRole Entity
        CreateMap<UserRoleCreateDto, UserRole>();
        CreateMap<UserRole, UserRoleDto>();
    }
}

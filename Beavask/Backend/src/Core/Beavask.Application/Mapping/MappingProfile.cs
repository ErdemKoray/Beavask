using AutoMapper;
using Beavask.Application.DTOs.Auth;
using Beavask.Application.DTOs.Comment;
using Beavask.Application.DTOs.Company;
using Beavask.Application.DTOs.Customer;
using Beavask.Application.DTOs.Event;
using Beavask.Application.DTOs.LogDtos;
using Beavask.Application.DTOs.Message;
using Beavask.Application.DTOs.Milestone;
using Beavask.Application.DTOs.NotificationDtos;
using Beavask.Application.DTOs.Permission;
using Beavask.Application.DTOs.Problem;
using Beavask.Application.DTOs.Project;
using Beavask.Application.DTOs.Role;
using Beavask.Application.DTOs.RolePermission;
using Beavask.Application.DTOs.Task;
using Beavask.Application.DTOs.Team;
using Beavask.Application.DTOs.User;
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
        CreateMap<TeamCreateDto, Team>()
        .ForMember(dest => dest.CompanyId, opt => opt.Ignore())
        .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
        .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
        .ForMember(dest => dest.TeamMembers, opt => opt.Ignore())
        .ForMember(dest => dest.Events, opt => opt.Ignore());
        CreateMap<TeamUpdateDto, Team>();
        CreateMap<Team, TeamWithMembersDto>();

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

        //User Entity
        CreateMap<UserCreateDto, User>();
        CreateMap<UserUpdateDto, User>();
        CreateMap<User, UserDto>();
        CreateMap<UserWithTeamAndCompanyDto,UserDto>();
        CreateMap<User, UserBirefForCompany>()
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.UserName ?? string.Empty))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email ?? string.Empty))
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.IsRegistered, opt => opt.MapFrom(src => true))
            .ForMember(dest => dest.IsAssignedToCompany, opt => opt.MapFrom(src => src.CompanyId.HasValue));
        

        //Milestone Entity
        CreateMap<Milestone, MilestoneDto>().ReverseMap();
        CreateMap<MilestoneCreateDto, Milestone>().ReverseMap();
        CreateMap<MilestoneUpdateDto, Milestone>().ReverseMap();

        //Customer Entity
        CreateMap<CustomerCreateDto, Customer>();
        CreateMap<CustomerUpdateDto, Customer>();
        CreateMap<Customer, CustomerDto>();

        //Project Entity
        CreateMap<Project, ProjectDto>().ReverseMap();
        CreateMap<ProjectCreateDto, Project>();
        CreateMap<ProjectUpdateDto, Project>();   

        //Task Entity
        CreateMap<Domain.Entities.Base.Task, TaskDto>().ReverseMap();
        CreateMap<TaskCreateDto, Domain.Entities.Base.Task>();
        CreateMap<TaskUpdateDto, Domain.Entities.Base.Task>();     
        CreateMap<TaskDto,Domain.Entities.Base.Task>();

        //InvitationToken Entity
        CreateMap<ProjectInvitationRequest, InvitationToken>()
        .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.ToEmail))
        .ForMember(dest => dest.Token, opt => opt.Ignore())
        .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
        .ForMember(dest => dest.ExpiresAt, opt => opt.Ignore())
        .ForMember(dest => dest.IsUsed, opt => opt.Ignore())
        .ForMember(dest => dest.Id, opt => opt.Ignore());

        //Comment Entity
        CreateMap<CommentCreateDto, Comment>();
        CreateMap<Comment, CommentDto>();
        CreateMap<CommentDto, Comment>();
        CreateMap<Comment, CommentCreateDto>();
    }
}
using Beavask.Application.Interface.Repository;

namespace Beavask.Application.Interface;

public interface IUnitOfWork
{
    ICompanyRepository CompanyRepository { get; }
    IUserRepository UserRepository { get; }
    IRoleRepository RoleRepository { get; }
    IProblemRepository ProblemRepository { get; }
    ICustomerRepository CustomerRepository { get; }
    IDependencyRepository DependencyRepository { get; }
    IProjectRepository ProjectRepository { get; }
    IFileRepository FileRepository { get; }
    IEventRepository EventRepository { get; }
    IPermissionRepository PermissionRepository { get; }
    ITaskRepository TaskRepository { get; }
    ILogRepository LogRepository { get; }
    ITeamRepository TeamRepository { get; }
    IMilestoneRepository MilestoneRepository { get; }
    IRolePermissionRepository RolePermissionRepository { get; }
    ITimeTrackingRepository TimeTrackingRepository { get; }
    IMessageRepository MessageRepository { get; }
    IUserRoleRepository UserRoleRepository { get; }
    ITeamEventRepository TeamEventRepository { get; }
    INotificationRepository NotificationRepository { get; }
    IUserContactRepository UserContactRepository { get; }
    IProjectMemberRepository ProjectMemberRepository { get; }
    ICommentRepository CommentRepository { get; }
    IVerificationCodeRepository VerificationCodeRepository { get; }
    
    Task<int> SaveChangesAsync();
}

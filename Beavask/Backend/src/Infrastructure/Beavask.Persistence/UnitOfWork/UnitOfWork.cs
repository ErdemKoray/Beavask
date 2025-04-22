using Beavask.Application.Interface.Repository;
using Beavask.Application.Interface;
using Beavask.Infrastructure.Persistence;
using Beavask.Persistence.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly BeavaskDbContext _context;

    public UnitOfWork(BeavaskDbContext context)
    {
        _context = context;
    }

    private ICompanyRepository? _companyRepository;
    public ICompanyRepository CompanyRepository => _companyRepository ??= new CompanyRepository(_context);

    private IUserRepository? _userRepository;
    public IUserRepository UserRepository => _userRepository ??= new UserRepository(_context);

    private IRoleRepository? _roleRepository;
    public IRoleRepository RoleRepository => _roleRepository ??= new RoleRepository(_context);

    private IProblemRepository? _problemRepository;
    public IProblemRepository ProblemRepository => _problemRepository ??= new ProblemRepository(_context);

    private ICustomerRepository? _customerRepository;
    public ICustomerRepository CustomerRepository => _customerRepository ??= new CustomerRepository(_context);

    private IDependencyRepository? _dependencyRepository;
    public IDependencyRepository DependencyRepository => _dependencyRepository ??= new DependencyRepository(_context);

    private IProjectRepository? _projectRepository;
    public IProjectRepository ProjectRepository => _projectRepository ??= new ProjectRepository(_context);

    private IFileRepository? _fileRepository;
    public IFileRepository FileRepository => _fileRepository ??= new FileRepository(_context);

    private IEventRepository? _eventRepository;
    public IEventRepository EventRepository => _eventRepository ??= new EventRepository(_context);

    private IPermissionRepository? _permissionRepository;
    public IPermissionRepository PermissionRepository => _permissionRepository ??= new PermissionRepository(_context);

    private ITaskRepository? _taskRepository;
    public ITaskRepository TaskRepository => _taskRepository ??= new TaskRepository(_context);

    private ILogRepository? _logRepository;
    public ILogRepository LogRepository => _logRepository ??= new LogRepository(_context);

    private ITeamRepository? _teamRepository;
    public ITeamRepository TeamRepository => _teamRepository ??= new TeamRepository(_context);

    private IMilestoneRepository? _milestoneRepository;
    public IMilestoneRepository MilestoneRepository => _milestoneRepository ??= new MilestoneRepository(_context);

    private IRolePermissionRepository? _rolePermissionRepository;
    public IRolePermissionRepository RolePermissionRepository => _rolePermissionRepository ??= new RolePermissionRepository(_context);

    private ITimeTrackingRepository? _timeTrackingRepository;
    public ITimeTrackingRepository TimeTrackingRepository => _timeTrackingRepository ??= new TimeTrackingRepository(_context);

    private IMessageRepository? _messageRepository;
    public IMessageRepository MessageRepository => _messageRepository ??= new MessageRepository(_context);

    private IUserRoleRepository? _userRoleRepository;
    public IUserRoleRepository UserRoleRepository => _userRoleRepository ??= new UserRoleRepository(_context);

    private ITeamEventRepository? _teamEventRepository;
    public ITeamEventRepository TeamEventRepository => _teamEventRepository ??= new TeamEventRepository(_context);

    private INotificationRepository? _notificationRepository;
    public INotificationRepository NotificationRepository => _notificationRepository ??= new NotificationRepository(_context);

    private IUserContactRepository? _userContactRepository;
    public IUserContactRepository UserContactRepository => _userContactRepository ??= new UserContactRepository(_context);

    private IProjectMemberRepository? _projectMemberRepository;
    public IProjectMemberRepository ProjectMemberRepository => _projectMemberRepository ??= new ProjectMemberRepository(_context);

    private ICommentRepository? _commentRepository;
    public ICommentRepository CommentRepository => _commentRepository ??= new CommentRepository(_context);

    public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();
}

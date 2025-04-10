using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.Entities.Core;
using TaskManagement.Domain.Entities.Relationships;

namespace TaskManagement.Infrastructure.Persistence
{
    public class TaskManagementDbContext : DbContext
    {
        public TaskManagementDbContext(DbContextOptions<TaskManagementDbContext> options)
            : base(options)
        {
        }

        // Core klasöründeki entity'ler
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Dependency> Dependencies { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Domain.Entities.Core.File> Files { get; set; }         // Çakışma olursa alias veya isim değişikliği yapın
        public DbSet<Log> Logs { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Milestone> Milestones { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Domain.Entities.Core.Permission> Permissions { get; set; }
        public DbSet<Problem> Problems { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Domain.Entities.Core.Task> Tasks { get; set; } // 'Task' ismi System.Threading.Tasks ile çakışabilir
        public DbSet<TimeTracking> TimeTrackings { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserContact> UserContacts { get; set; }

        // Relationships klasöründeki entity'ler
        public DbSet<ProjectMember> ProjectMembers { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<TeamEvent> TeamEvents { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Burada Fluent API yapılandırmaları, tablo isimleri, ilişkiler vb. tanımlanabilir.
        }
    }
}

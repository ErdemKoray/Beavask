using Microsoft.EntityFrameworkCore;
using Beavask.Domain.Entities.Base;
using Beavask.Domain.Entities.Join;

namespace Beavask.Infrastructure.Persistence
{
    public class BeavaskDbContext : DbContext
    {
        public BeavaskDbContext(DbContextOptions<BeavaskDbContext> options)
            : base(options)
        {
        }

        public DbSet<Comment> Comments { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Dependency> Dependencies { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Domain.Entities.Base.File> Files { get; set; }         
        public DbSet<Log> Logs { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Milestone> Milestones { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Domain.Entities.Base.Permission> Permissions { get; set; }
        public DbSet<Problem> Problems { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Domain.Entities.Base.Task> Tasks { get; set; }
        public DbSet<TimeTracking> TimeTrackings { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserContact> UserContacts { get; set; }
        public DbSet<VerificationCode> VerificationCodes { get; set; }
        public DbSet<InvitationToken> InvitationTokens { get; set; }

        // Relationships klasöründeki entity'ler
        public DbSet<ProjectMember> ProjectMembers { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<TeamEvent> TeamEvents { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

                modelBuilder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany(u => u.SentMessages)
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

                modelBuilder.Entity<Message>()
                .HasOne(m => m.Receiver)
                .WithMany(u => u.ReceivedMessages)
                .HasForeignKey(m => m.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

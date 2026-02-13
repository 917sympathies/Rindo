using Microsoft.EntityFrameworkCore;
using Rindo.Domain.DataObjects;

namespace Rindo.Infrastructure;

public class PostgresDbContext(DbContextOptions<PostgresDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Chat>()
            .HasMany(c => c.Messages)
            .WithOne()
            .HasForeignKey(b => b.ChatId);
        
        modelBuilder.Entity<ProjectTask>()
            .HasMany(c => c.Comments)
            .WithOne()
            .HasForeignKey(c => c.TaskId);
        
        modelBuilder.Entity<Stage>()
            .HasMany(p => p.Tasks)
            .WithOne()
            .HasForeignKey(p => p.StageId);
        
        modelBuilder.Entity<Project>()
            .HasMany(p => p.Stages)
            .WithOne()
            .HasForeignKey(p => p.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Project>()
            .HasMany<ProjectTask>()
            .WithOne()
            .HasForeignKey(p => p.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Chat>()
            .HasOne<Project>()
            .WithOne();

        modelBuilder.Entity<Project>()
            .HasMany(p => p.Roles)
            .WithOne()
            .HasForeignKey(r => r.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Role>()
            .HasMany(r => r.Users)
            .WithMany()
            .UsingEntity("roles_to_users");
        
        modelBuilder.Entity<User>()
            .HasMany(u => u.Invitations)
            .WithOne()
            .HasForeignKey(inv => inv.RecipientId);

        modelBuilder.Entity<Project>()
            .HasMany(p => p.Invitations)
            .WithOne()
            .HasForeignKey(inv => inv.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);;
        
        modelBuilder.Entity<Project>()
            .HasMany(p => p.Users)
            .WithMany(p => p.Projects)
            .UsingEntity("projects_to_users");
    }
    
    public DbSet<Chat> Chats { get; init; }
    public DbSet<ChatMessage> ChatMessages { get; init; }
    public DbSet<User> Users { get; init; }
    public DbSet<Stage> Stages { get; init; }
    public DbSet<Project> Projects { get; init; }
    public DbSet<ProjectTask> Tasks { get; init; }
    public DbSet<TaskComment> TaskComments { get; init; }
    public DbSet<Role> Roles { get; init; }
    public DbSet<Invitation> Invitations { get; init; }
}
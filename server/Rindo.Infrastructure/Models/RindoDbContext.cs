using Microsoft.EntityFrameworkCore;
using Rindo.Domain.Entities;
using Task = Rindo.Domain.Entities.Task;

namespace Rindo.Infrastructure.Models;

public class RindoDbContext : DbContext
{
    public RindoDbContext(DbContextOptions<RindoDbContext> options)
        :base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Chat>()
            .HasMany(c => c.Messages)
            .WithOne(c => c.Chat)
            .HasForeignKey(b => b.ChatId);
        
        modelBuilder.Entity<Task>()
            .HasMany(c => c.Comments)
            .WithOne(c => c.Task)
            .HasForeignKey(c => c.TaskId);
        
        modelBuilder.Entity<Stage>()
            .HasMany(p => p.Tasks)
            .WithOne()
            .HasForeignKey(p => p.StageId);
        
        modelBuilder.Entity<Project>()
            .HasMany(p => p.Stages)
            .WithOne()
            .HasForeignKey(p => p.ProjectId);

        modelBuilder.Entity<Chat>()
            .HasOne<Project>()
            .WithOne();

        modelBuilder.Entity<Project>()
            .HasMany(p => p.Roles)
            .WithOne()
            .HasForeignKey(r => r.ProjectId);

        modelBuilder.Entity<Role>()
            .HasMany(r => r.Users)
            .WithMany();
        
        modelBuilder.Entity<User>()
            .HasMany(u => u.Invitations)
            .WithOne()
            .HasForeignKey(inv => inv.UserId);

        modelBuilder.Entity<Project>()
            .HasMany(p => p.Invitations)
            .WithOne()
            .HasForeignKey(inv => inv.ProjectId);

        modelBuilder.Entity<Project>()
            .HasMany(p => p.Tags)
            .WithOne()
            .HasForeignKey(t => t.ProjectId);
    }
    
    public DbSet<Chat> Chats { get; init; }
    
    public DbSet<ChatMessage> ChatMessages { get; init; }
    
    public DbSet<User> Users { get; init; }
    
    public DbSet<Stage> Stages { get; init; }
    
    public DbSet<Project> Projects { get; init; }
    
    public DbSet<Task> Tasks { get; init; }
    
    public DbSet<TaskComment> TaskComments { get; init; }
    
    public DbSet<Role> Roles { get; init; }
    
    public DbSet<Tag> Tags { get; init; }
    
    public DbSet<Invitation> Invitations { get; init; }
}
namespace Rindo.Domain.Repositories;

public interface IUnitOfWork
{
    // IChatMessageRepository ChatMessages { get; }
    // IChatRepository Chats { get; }
    // IStageRepository Stages { get; }
    // IProjectRepository Projects { get; }
    // ITaskCommentRepository TaskComments { get; }
    // ITaskRepository Tasks { get; }
    // IRoleRepository Roles { get; }
    // IUserRepository Users { get; }
    Task SaveAsync();
}
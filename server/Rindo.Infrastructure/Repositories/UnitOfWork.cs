using Rindo.Domain.Repositories;
using Rindo.Infrastructure.Models;


namespace Rindo.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly RindoDbContext _context;

    // private IChatMessageRepository _chatMessageRepository = default!;
    // private IChatRepository _chatRepository = default!;
    // private IStageRepository _stagesRepository = default!;
    // private IProjectRepository _projectRepository = default!;
    // private ITaskCommentRepository _taskCommentRepository = default!;
    // private ITaskRepository _taskRepository = default!;
    // private IRoleRepository _roleRepository = default!;
    // private IUserRepository _userRepository = default!; 
    
    public UnitOfWork(RindoDbContext context)
    {
        _context = context;
    }
    //
    // public IChatMessageRepository ChatMessages
    // {
    //     get
    //     {
    //         if (_chatMessageRepository == null) _chatMessageRepository = new ChatMessageRepository(_context);
    //         return _chatMessageRepository;
    //     }
    // }
    //
    // public IChatRepository Chats
    // {
    //     get
    //     {
    //         if (_chatRepository == null) _chatRepository = new ChatRepository(_context);
    //         return _chatRepository;
    //     }
    // }
    //
    // public IStageRepository Stages
    // {
    //     get
    //     {
    //         if (_stagesRepository == null) _stagesRepository = new StageRepository(_context);
    //         return _stagesRepository;
    //     }
    // }
    //
    // public IProjectRepository Projects
    // {
    //     get
    //     {
    //         if (_projectRepository == null) _projectRepository = new ProjectRepository(_context);
    //         return _projectRepository;
    //     }
    // }
    //
    // public ITaskCommentRepository TaskComments
    // {
    //     get
    //     {
    //         if (_taskCommentRepository == null) _taskCommentRepository = new TaskCommentRepository(_context);
    //         return _taskCommentRepository;
    //     }
    // }
    //
    // public ITaskRepository Tasks
    // {
    //     get
    //     {
    //         if (_taskRepository == null) _taskRepository = new TaskRepository(_context);
    //         return _taskRepository;
    //     }
    // }
    //
    // public IRoleRepository Roles
    // {
    //     get
    //     {
    //         if (_roleRepository == null) _roleRepository = new RoleRepository(_context);
    //         return _roleRepository;
    //     }
    // }
    //
    // public IUserRepository Users
    // {
    //     get
    //     {
    //         if (_userRepository == null) _userRepository = new UserRepository(_context);
    //         return _userRepository;
    //     }
    // }

    public Task SaveAsync() => _context.SaveChangesAsync();
}
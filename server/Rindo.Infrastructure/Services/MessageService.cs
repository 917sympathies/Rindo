using Application.Services.UserService;
using Rindo.Domain.Entities;
using Rindo.Domain.Repositories;
using Task = System.Threading.Tasks.Task;

namespace Application.Services.IChatMessageService;

public class MessageService : IMessageService
{
    private readonly IChatMessageRepository _messageRepository;
    private readonly IUserService _userService;
    private readonly IUnitOfWork _unitOfWork;
    
    public MessageService(IChatMessageRepository messageRepository, IUserService userService, IUnitOfWork unitOfWork)
    {
        _messageRepository = messageRepository;
        _unitOfWork = unitOfWork;
        _userService = userService;
    }
    public async Task<Tuple<string, string>> AddMessage(Guid userId, Guid chatId, string content)
    {
        var user = (await _userService.GetUserById(userId)).Value;
        var msg = new ChatMessage() { ChatId = chatId, SenderId = userId, Content = content, Username = user!.Username};
        await _messageRepository.AddMessage(msg);
        await _unitOfWork.SaveAsync();
        return new Tuple<string, string>(user.Username, msg.Id.ToString());
    }
}
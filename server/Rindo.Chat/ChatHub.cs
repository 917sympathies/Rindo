using Application.Interfaces.Services;
using Application.Services;
using Application.Services.ChatService;
using Application.Services.CommentsService;
using Application.Services.IChatMessageService;
using Application.Services.UserService;
using Microsoft.AspNetCore.SignalR;

namespace Rindo.Chat;

public class ChatHub : Hub
{
    private readonly IMessageService _messageService;
    private readonly ICommentService _commentService;
    private readonly IUserService _userService;
    private readonly IProjectService _projectService;
    private readonly IInvitationService _invitationService;

    public ChatHub(IMessageService messageService, ICommentService commentService, IUserService userService, IProjectService projectService, IInvitationService invitationService)
    {
        _messageService = messageService;
        _commentService = commentService;
        _userService = userService;
        _projectService = projectService;
        _invitationService = invitationService;
    }
    
    public async Task SendProjectChat(string user, string message, string chat)
    {
        var userId = Guid.Parse(user);
        var chatId = Guid.Parse(chat);
        var result = await _messageService.AddMessage(userId, chatId, message);
        await this.Clients.All.SendAsync($"ReceiveProjectChat{chat}", result.Item2,result.Item1, message);
    }
    
    public async Task SendTaskComment(string user, string message, string task)
    {
        var userId = Guid.Parse(user);
        var taskId = Guid.Parse(task);
        var comment = await _commentService.AddComment(userId, taskId, message);
        await this.Clients.All.SendAsync($"ReceiveTaskComment{task}", comment);
    }

    public async Task FetchDeleteProject(string project)
    {
        var projectId = Guid.Parse(project);
        await this.Clients.All.SendAsync($"ReceiveDeleteProject", projectId);
    }

    public async Task FetchChangeProjectName(string project)
    {
        var projectId = Guid.Parse(project);
        var proj = await _projectService.GetProjectById(projectId);
        await this.Clients.All.SendAsync("ReceiveChangeProjectName", projectId, proj.Name);
    }

    public async Task SendAcceptInvite(string invite, string project, string user)
    {
        var inviteId = Guid.Parse(invite);
        var projectId = Guid.Parse(project);
        var userId = Guid.Parse(user);
        var projectEntity = await _projectService.GetProjectById(projectId);
        var result = await _projectService.AddUserToProject(projectId, userId);
        if (result.IsSuccess && projectEntity != null)
        {
            await _invitationService.DeleteInvitation(inviteId);
            await this.Clients.All.SendAsync($"ReceiveAcceptInvite{user}", projectEntity.Id, projectEntity.Name);
        }
    }

    public async Task SendTaskAdd(string projectId)
    {
        await this.Clients.All.SendAsync($"ReceiveTaskAdd{projectId}", true);
    }
}
using Application.Interfaces.Services;
using Microsoft.AspNetCore.SignalR;

namespace Rindo.Chat;

public class ChatHub : Hub
{
    private readonly IMessageService _messageService;
    private readonly ICommentService _commentService;
    private readonly IProjectService _projectService;
    private readonly IInvitationService _invitationService;

    public ChatHub(IMessageService messageService, ICommentService commentService, IProjectService projectService, IInvitationService invitationService)
    {
        _messageService = messageService;
        _commentService = commentService;
        _projectService = projectService;
        _invitationService = invitationService;
    }
    
    public async Task SendProjectChat(string _userId, string message, string _chatId)
    {
        var userId = Guid.Parse(_userId);
        var chatId = Guid.Parse(_chatId);
        var result = await _messageService.AddMessage(userId, chatId, message);
        await this.Clients.All.SendAsync($"ReceiveProjectChat{_chatId}", result.Item2,result.Item1, message);
    }
    
    public async Task SendTaskComment(string _userId, string message, string _taskId)
    {
        var userId = Guid.Parse(_userId);
        var taskId = Guid.Parse(_taskId);
        var comment = await _commentService.AddComment(userId, taskId, message);
        await this.Clients.All.SendAsync($"ReceiveTaskComment{_taskId}", comment);
    }

    public async Task FetchDeleteProject(string _projectId)
    {
        await this.Clients.All.SendAsync($"ReceiveDeleteProject", _projectId);
    }

    public async Task FetchChangeProjectName(string _projectId)
    {
        var projectId = Guid.Parse(_projectId);
        var project = await _projectService.GetProjectById(projectId);
        if (project is null) return;
        await this.Clients.All.SendAsync("ReceiveChangeProjectName", projectId, project.Name);
    }

    public async Task SendAcceptInvite(string _inviteId, string _projectId, string _userId)
    {
        var inviteId = Guid.Parse(_inviteId);
        var projectId = Guid.Parse(_projectId);
        var userId = Guid.Parse(_userId);
        var project = await _projectService.GetProjectById(projectId);
        var result = await _projectService.AddUserToProject(projectId, userId);
        if (project is null || !result.IsSuccess) return; 
        await _invitationService.DeleteInvitation(inviteId); 
        await this.Clients.All.SendAsync($"ReceiveAcceptInvite{_userId}", project.Id, project.Name);
    }

    public async Task SendTaskAdd(string _projectId)
    {
        await this.Clients.All.SendAsync($"ReceiveTaskAdd{_projectId}", true);
    }
}
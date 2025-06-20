﻿using Application.Common.Mapping;
using Application.Interfaces.Services;
using Rindo.Domain.Common;
using Rindo.Domain.DTO;
using Rindo.Domain.Models;
using Rindo.Domain.Repositories;

namespace Application.Services;

public class UserService(IUserRepository userRepository, IProjectRepository projectRepository) : IUserService
{
    public async Task<Result<User?>> GetUserById(Guid id)
    {
        var user = await userRepository.GetUserById(id);
        if (user is null) return Error.NotFound("User with this id doesn't exists");
        return user;
    }

    //TODO: what's the point of this method? if it's necessary then remove nullable
    public async Task<UserDto?> GetUserInfo(Guid id)
    {
        var user = await userRepository.GetUserById(id);
        return user?.MapToDto();
    }

    public async Task<Result> ChangeUserLastName(Guid id, string lastName)
    {
        var user = await userRepository.GetUserById(id);
        if (user is null) return Error.NotFound("User with this id doesn't exists");
        user.LastName = lastName;
        userRepository.UpdateUser(user);
        
        return Result.Success();
    }

    public async Task<Result> ChangeUserFirstName(Guid id, string firstName)
    {
        var user = await userRepository.GetUserById(id);
        if (user is null) return Error.NotFound("User with this id doesn't exists");
        
        user.FirstName = firstName;
        userRepository.UpdateUser(user);
        return Result.Success();
    }

    public async Task<Result<IEnumerable<UserDto>>> GetUsersByProjectId(Guid projectId)
    {
        var project = await projectRepository.GetProjectById(projectId);
        if (project is null) return Error.NotFound("Project with this id doesn't exists");
        
        var users = project.Users.ToList();
        var owner = await userRepository.GetUserById(project.OwnerId);
        users.Add(owner);
        return users.Select(x => x.MapToDto()).ToArray();
    }
}
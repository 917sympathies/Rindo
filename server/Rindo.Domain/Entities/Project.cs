﻿using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Rindo.Domain.Entities;

public class Project
{
    public Guid Id { get; init;}
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public Guid OwnerId { get; init; }
    public Guid ChatId { get; set; } 
    public ICollection<Stage> Stages { get; set; } = default!;
    public ICollection<User> Users { get; set; } = default!;
    public ICollection<Role> Roles { get; set; } = default!;
    public ICollection<Invitation> Invitations { get; set; } = default!;
    public ICollection<Tag> Tags { get; set; } = default!;
    public DateOnly StartDate { get; set; } 
    public DateOnly FinishDate { get; set; } 
}
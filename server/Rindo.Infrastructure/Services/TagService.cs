using Application.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using Rindo.Domain.Common;
using Rindo.Domain.Entities;
using Rindo.Infrastructure.Models;

namespace Rindo.Infrastructure.Services;

public class TagService : ITagService
{
    private readonly RindoDbContext _context;

    public TagService(RindoDbContext context) => _context = context;

    public async Task<Tag> CreateTag(string name, Guid projectId)
    {
        var tag = new Tag { Name = name, ProjectId = projectId };
        _context.Tags.Add(tag);
        await _context.SaveChangesAsync();
        return tag;
    }

    public async Task<Result> DeleteTag(Guid id)
    {
        var tag = await _context.Tags.FirstOrDefaultAsync(t => t.Id == id);
        if (tag is null) return Error.NotFound("Такого тэга не существует");
        _context.Tags.Remove(tag);
        await _context.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<IEnumerable<Tag>> GetTagsByProjectId(Guid projectId)
    {
        var tags = await _context.Tags.Where(t => t.ProjectId == projectId).ToListAsync();
        return tags;
    }
}
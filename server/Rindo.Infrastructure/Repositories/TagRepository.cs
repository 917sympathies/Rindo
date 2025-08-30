using Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Rindo.Domain.Models;


namespace Rindo.Infrastructure.Repositories;

public class TagRepository(PostgresDbContext context) : RepositoryBase<Tag>(context), ITagRepository
{
    public async Task AddTagAsync(Tag tag)
    {
        await CreateAsync(tag);
    }

    public Task DeleteTag(Tag tag)
    {
        Delete(tag);
        return Task.CompletedTask;
    }

    public Task DeleteManyTags(IEnumerable<Tag> tags)
    {
        DeleteMany(tags);
        return Task.CompletedTask;
    }

    public async Task<Tag?> GetTagByIdAsync(Guid tagId)
    {
        return await FindByCondition(x => x.Id == tagId).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Tag>> GetTagsByProjectIdAsync(Guid projectId)
    {
        return await FindByCondition(x => x.ProjectId == projectId).ToListAsync();
    }
}
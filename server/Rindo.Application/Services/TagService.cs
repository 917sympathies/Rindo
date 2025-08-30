using Application.Common.Exceptions;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Rindo.Domain.Models;

namespace Application.Services;

public class TagService(ITagRepository tagRepository) : ITagService
{
    public async Task<Tag> CreateTag(string name, Guid projectId)
    {
        var tag = new Tag { Name = name, ProjectId = projectId };
        await tagRepository.AddTagAsync(tag);
        return tag;
    }

    public async Task DeleteTag(Guid tagId)
    {
        var tag = await tagRepository.GetTagByIdAsync(tagId);
        if (tag is null) throw new NotFoundException(nameof(Tag), tagId);
        await tagRepository.DeleteTag(tag);
    }

    public async Task DeleteTagsByProjectId(Guid projectId)
    {
        var tags = await tagRepository.GetTagsByProjectIdAsync(projectId);
        await tagRepository.DeleteManyTags(tags);
    }

    public async Task<IEnumerable<Tag>> GetTagsByProjectId(Guid projectId)
    {
        return await tagRepository.GetTagsByProjectIdAsync(projectId);
    }
}
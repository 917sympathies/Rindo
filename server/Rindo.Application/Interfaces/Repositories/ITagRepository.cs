using Rindo.Domain.Models;

namespace Application.Interfaces.Repositories;

public interface ITagRepository
{
    Task AddTagAsync(Tag tag);
    Task DeleteTag(Tag tag);
    Task DeleteManyTags(IEnumerable<Tag> tags);
    Task<Tag?> GetTagByIdAsync(Guid tagId);
    Task<IEnumerable<Tag>> GetTagsByProjectIdAsync(Guid projectId);
}
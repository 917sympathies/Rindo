using Rindo.Domain.Common;
using Rindo.Domain.Entities;

namespace Application.Interfaces.Services;

public interface ITagService
{
    Task<Tag> CreateTag(string name, Guid projectId);
    Task<Result> DeleteTag(Guid id);
    Task<IEnumerable<Tag>> GetTagsByProjectId(Guid projectId);
}
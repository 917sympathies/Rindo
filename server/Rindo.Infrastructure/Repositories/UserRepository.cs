using Microsoft.EntityFrameworkCore;
using Rindo.Domain.DTO;
using Rindo.Domain.Entities;
using Rindo.Domain.Repositories;
using Rindo.Infrastructure.Models;
using Task = System.Threading.Tasks.Task;

namespace Rindo.Infrastructure.Repositories;

public class UserRepository : RepositoryBase<User>, IUserRepository
{
    public UserRepository(RindoDbContext context) : base(context)
    {
    }

    public Task CreateUser(User user) => CreateAsync(user);

    public Task DeleteUser(User user) => DeleteAsync(user);

    public Task UpdateUser(User user) => UpdateAsync(user);

    public async Task<User?> GetUserById(Guid id) =>
        await FindByCondition(u => u.Id == id).Include(u => u.Projects).FirstOrDefaultAsync();

    public async Task<User?> GetUserByUsername(string username) =>
        await FindByCondition(u => u.Username == username)
            .Include(u => u.Projects)
            .FirstOrDefaultAsync();
        
}
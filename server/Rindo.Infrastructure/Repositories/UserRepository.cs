using Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Rindo.Domain.Models;

using Task = System.Threading.Tasks.Task;

namespace Rindo.Infrastructure.Repositories;

public class UserRepository : RepositoryBase<User>, IUserRepository
{
    private readonly PostgresDbContext _context;
    public UserRepository(PostgresDbContext context) : base(context)
    {
        _context = context;
    }

    public Task<User> CreateUser(User user) => CreateAsync(user);

    public void DeleteUser(User user) => Delete(user);

    public void UpdateUser(User user) => Update(user);

    public async Task<User?> GetUserById(Guid id) =>
        await FindByCondition(u => u.Id == id).Include(u => u.Projects).FirstOrDefaultAsync();

    public async Task<User?> GetUserByUsername(string username) =>
        await FindByCondition(u => u.Username == username)
            .Include(u => u.Projects)
            .FirstOrDefaultAsync();

    public async Task<User[]> GetUsersByIds(Guid[] ids)
    {
        return await _context.Users.Where(x => ids.Contains(x.Id)).ToArrayAsync();
    }
        
}
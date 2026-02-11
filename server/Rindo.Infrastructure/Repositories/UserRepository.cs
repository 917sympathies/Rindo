using Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Rindo.Domain.DataObjects;
using Task = System.Threading.Tasks.Task;

namespace Rindo.Infrastructure.Repositories;

public class UserRepository(PostgresDbContext context) : RepositoryBase<User>(context), IUserRepository
{
    private readonly PostgresDbContext _context = context;

    public Task<User> CreateUser(User user) => CreateAsync(user);
    
    public async Task DeleteUser(User user) => await Delete(user);
    
    public async Task UpdateUser(User user)
    {
        await _context.Users
            .Where(x => x.UserId == user.UserId)
            .ExecuteUpdateAsync(updates => updates
                .SetProperty(x => x.FirstName, user.FirstName)
                .SetProperty(x => x.LastName, user.LastName)
                .SetProperty(x => x.Email, user.Email)
            );
    }

    public async Task<User?> GetUserById(Guid id) =>
        await FindByCondition(u => u.UserId == id).Include(u => u.Projects).FirstOrDefaultAsync();

    public async Task<User?> GetUserByUsername(string username) =>
        await FindByCondition(u => u.Username == username)
            .Include(u => u.Projects)
            .FirstOrDefaultAsync();

    public async Task<User[]> GetUsersByIds(Guid[] ids)
    {
        return await _context.Users.Where(x => ids.Contains(x.UserId)).ToArrayAsync();
    }
        
}
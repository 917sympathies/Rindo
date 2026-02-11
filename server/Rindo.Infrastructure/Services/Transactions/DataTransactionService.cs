using Application.Interfaces.Transactions;

namespace Rindo.Infrastructure.Services.Transactions;

public class DataTransactionService(PostgresDbContext context) : IDataTransactionService
{
    public async Task BeginTransactionAsync()
    {
        await context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        await context.Database.CommitTransactionAsync();
    }
}
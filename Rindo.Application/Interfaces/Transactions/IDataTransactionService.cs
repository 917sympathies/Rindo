namespace Application.Interfaces.Transactions;

public interface IDataTransactionService
{
    public Task BeginTransactionAsync();
    public Task CommitTransactionAsync();
}
namespace Carsharing.Core.Abstractions
{
    public interface IUnitOfWork
    {
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        void Dispose();
        Task RollbackTransactionAsync();
        Task SaveChangesAsync();
    }
}
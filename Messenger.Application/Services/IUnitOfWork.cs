namespace Messenger.Application.Services;

public interface IUnitOfWork {

    Task BeginTransaction(CancellationToken cancel = default);

    Task CommitTransaction(CancellationToken cancel = default);

    Task RollbackTransaction(CancellationToken cancel = default);
}
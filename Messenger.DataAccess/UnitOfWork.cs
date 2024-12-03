using Messenger.Application.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Messenger.DataAccess;

public class UnitOfWork(MessengerDbContext db) : IUnitOfWork {
    public async Task BeginTransaction(CancellationToken cancel = default) {
        await db.Database.BeginTransactionAsync(cancel).ConfigureAwait(false);
    }

    public async Task CommitTransaction(CancellationToken cancel = default) {
        await db.Database.CommitTransactionAsync(cancel).ConfigureAwait(false);
    }

    public async Task RollbackTransaction(CancellationToken cancel = default) {
        await db.Database.RollbackTransactionAsync(cancel).ConfigureAwait(false);
    }
}
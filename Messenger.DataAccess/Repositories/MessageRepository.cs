using Messenger.Application.Repositories;
using Messenger.Domain;
using Microsoft.EntityFrameworkCore;

namespace Messenger.DataAccess.Repositories;

public class MessageRepository(MessengerDbContext context) : IMessageRepository {
    public async Task CreateAsync(Message message, CancellationToken cancellationToken) {
        await context.AddAsync(message, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

    }

    public async Task DeleteAsync(Message message, CancellationToken cancellationToken) {
        message.Delete();
        context.Update(message);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Message?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) {
        return await context.Set<Message>().SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task UpdateAsync(Message message, CancellationToken cancellationToken = default) {
        context.Update(message);
        await context.SaveChangesAsync(cancellationToken);
    }
}
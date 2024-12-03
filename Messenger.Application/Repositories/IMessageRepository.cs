using Messenger.Domain;

namespace Messenger.Application.Repositories;

public interface IMessageRepository {
    public Task<Message?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    public Task CreateAsync(Message message, CancellationToken cancellationToken);
    public Task DeleteAsync(Message message, CancellationToken cancellationToken);
    public Task UpdateAsync(Message message, CancellationToken cancellationToken = default);
}
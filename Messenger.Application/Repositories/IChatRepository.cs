using Messenger.Application.Models;
using Messenger.Domain;

namespace Messenger.Application.Repositories;

public interface IChatRepository {
    public Task<Chat?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    public Task<IList<ChatDto>> GetAllByUserId(Guid userId, CancellationToken cancellationToken);
}
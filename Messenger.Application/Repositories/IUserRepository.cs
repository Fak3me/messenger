using Messenger.Domain;

namespace Messenger.Application.Repositories;

public interface IUserRepository {
    public Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    public Task<IList<Guid>> GetChatParticipantsIdsByChatId(Guid chatId, CancellationToken cancellationToken);

    public IAsyncEnumerable<(Guid userId,string email, List<Guid> messageIds)> GetUserMailsForUnreadMessagesNotification(
        Guid[] offlineUserIds, CancellationToken cancellationToken = default);

    public Task<IList<Guid>> GetUserIds(CancellationToken cancellationToken = default);
}
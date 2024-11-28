using Messanager.Domain;

namespace Messenger.Application.Repositories {
    public interface IUserRepository {
        public Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        public Task<IList<Guid>> GetChatParticipantsIdsByChatId(Guid chatId, CancellationToken cancellationToken);
    }

    public interface IMessageRepository {
        public Task CreateAsync(Message message, CancellationToken cancellationToken);
        public Task DeleteAsync(Message message, CancellationToken cancellationToken);
        public Task UpdateAsync(Message message, CancellationToken cancellationToken);
    }
}
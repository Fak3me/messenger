using Messenger.Application.Models;

namespace Messenger.Application.Services
{
    public interface IMessagesService {
        Task CreateMessage(MessageDto message, Guid creatorId, CancellationToken ct = default);
        Task EditMessage(MessageDto message, CancellationToken ct = default);
        Task DeleteMessage(MessageDto message, CancellationToken ct = default);
        Task<IList<MessageDto>> GetMessages(Guid chatId, CancellationToken ct);
    }
}

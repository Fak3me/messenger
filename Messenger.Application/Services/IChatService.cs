using Messenger.Application.Models;

namespace Messenger.Application.Services
{
    public interface IChatService {
        Task<IList<ChatDto>> GetChats(Guid userId, CancellationToken ct);
    }
}

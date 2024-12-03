using Messenger.Application.Repositories;
using Messenger.Domain;
using Microsoft.EntityFrameworkCore;

namespace Messenger.DataAccess.Repositories {
    public class UserRepository(MessengerDbContext context) : IUserRepository {
        public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) {
            return await context.Set<User>().Where(x => x.Id == id).SingleOrDefaultAsync(cancellationToken);
        }

        public async Task<IList<Guid>>
            GetChatParticipantsIdsByChatId(Guid chatId, CancellationToken cancellationToken) {
            var chat = await context.Set<Chat>().Include(chat => chat.Participants)
                .SingleOrDefaultAsync(x => x.Id == chatId, cancellationToken);
            if (chat == null) {
                throw new ArgumentNullException("chat");
            }

            return chat.Participants.Select(x => x.Id).ToList() ?? new List<Guid>();
        }

        public async IAsyncEnumerable<(Guid userId, string email, List<Guid> messageIds)>
            GetUserMailsForUnreadMessagesNotification(
                Guid[] offlineUserIds, CancellationToken cancellationToken = default) {
            foreach (var userId in offlineUserIds) {
                var date = DateTime.Now.AddMinutes(-5);
                var user = await context.Set<User>().SingleOrDefaultAsync(x => x.Id == userId, cancellationToken);
                var messageIds = await context.Set<Message>().Where(message =>
                        message.Created < date &&
                        message.ReadBy.All(readBy => readBy.Id != userId) &&
                        message.NotificationReceivedBy.All(receivedBy => receivedBy.Id != userId) &&
                        message.Chat.Participants.Any(x => x.Id == userId)).Select(x => x.Id)
                    .ToListAsync(cancellationToken);
                yield return (user.Id, user.Email, messageIds);
            }
        }

        public async Task<IList<Guid>> GetUserIds(CancellationToken cancellationToken = default) {
            return await context.Set<User>().Select(x => x.Id).ToListAsync(cancellationToken);
        }
    }
}
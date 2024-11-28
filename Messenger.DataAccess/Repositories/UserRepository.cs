using Messanager.Domain;
using Messenger.Application.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Messenger.DataAccess.Repositories {
    public class UserRepository : IUserRepository {
        private readonly MessengerDbContext _context;

        public UserRepository(MessengerDbContext context) {
            _context = context;
        }

        public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken) {
            return await _context.Set<User>().Where(x => x.Id == id).SingleOrDefaultAsync(cancellationToken);
        }

        public async Task<IList<Guid>> GetChatParticipantsIdsByChatId(Guid chatId, CancellationToken cancellationToken) {
            var chat = await _context.Set<Chat>().SingleOrDefaultAsync(x => x.Id == chatId, cancellationToken);
            return chat?.Participants.Select(x => x.Id).ToList() ?? new List<Guid>();
        }
    }
    public class MessageRepository : IMessageRepository {
        private readonly MessengerDbContext _context;

        public MessageRepository(MessengerDbContext context) {
            _context = context;
        }

        public async Task CreateAsync(Message message, CancellationToken cancellationToken) {
            await _context.AddAsync(message, cancellationToken);
        }

        public async Task DeleteAsync(Message message, CancellationToken cancellationToken) {
            message.Delete();
            _context.Update(message);

        }

        public async Task UpdateAsync(Message message, CancellationToken cancellationToken) {
            _context.Update(message);
        }
    }
}
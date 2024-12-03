using Messenger.Application.Models;
using Messenger.Application.Repositories;
using Messenger.Domain;
using Microsoft.EntityFrameworkCore;

namespace Messenger.DataAccess.Repositories;

public class ChatRepository(MessengerDbContext context) : IChatRepository {
    public async Task<Chat?> GetByIdAsync(Guid id, CancellationToken cancellationToken) {
        return await context.Set<Chat>().Include(x=>x.Messages).SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<IList<ChatDto>> GetAllByUserId(Guid userId, CancellationToken cancellationToken) {
        return await context.Set<Chat>().Where((x => x.Participants.Any(y => y.Id == userId))).Select(x=> new ChatDto {
            ChatId = x.Id,
            Participants = x.Participants.Select(y=> new UserDto {
                UserId = y.Id,
                UserName =y.Login
            }).ToList()
        }).ToListAsync(cancellationToken);
    }
}
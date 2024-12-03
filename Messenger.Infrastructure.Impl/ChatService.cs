using Messenger.Application.Models;
using Messenger.Application.Repositories;
using Messenger.Application.Services;

namespace Messenger.Infrastructure.Impl;

public class ChatService : IChatService {
    private readonly IChatRepository _chatRepository;

    public ChatService(IChatRepository chatRepository) {
        _chatRepository = chatRepository;
    }

    public async Task<IList<ChatDto>> GetChats(Guid userId, CancellationToken ct) {
        return await _chatRepository.GetAllByUserId(userId, ct);
    }
}
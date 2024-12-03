using Messenger.Application;
using Messenger.Application.Models;
using Messenger.Application.Repositories;
using Messenger.Application.Services;
using Microsoft.AspNetCore.SignalR;

namespace Messenger.Service.SignalR
{
    public class MessengerClientService : IMessengerClientService {
        private readonly IHubContext<MessengerHub> _hubContext;
        private readonly IHubConnectionsAccessorService _hubConnectionsAccessorService;
        private readonly IUserRepository _userRepository;
        public MessengerClientService(IHubConnectionsAccessorService hubConnectionsAccessorService, IHubContext<MessengerHub> hubContext, IUserRepository userRepository) {
            _hubConnectionsAccessorService = hubConnectionsAccessorService;
            _hubContext = hubContext;
            _userRepository = userRepository;
        }

        public async Task NewMessageNotification(MessageDto message, CancellationToken ct) {
            var chatParticipantIds = await _userRepository.GetChatParticipantsIdsByChatId(message.ChatId, ct);
            foreach (var chatParticipantId in chatParticipantIds) {
                var onlineConnections = await _hubConnectionsAccessorService.FindConnectionsByEmployeeId(chatParticipantId);
                if (onlineConnections == null || !onlineConnections.Any()) {
                    return;
                }
                var onlineConnectionIds = onlineConnections.Select(x => x.ConnectionId).ToList();
                await _hubContext.Clients.Clients(onlineConnectionIds).SendAsync("NewMessage", message);
            }

        }
        public async Task MessageEditedNotification(MessageDto message, CancellationToken ct) {
            var chatParticipantIds = await _userRepository.GetChatParticipantsIdsByChatId(message.ChatId, ct);
            foreach (var chatParticipantId in chatParticipantIds) {
                var onlineConnections = await _hubConnectionsAccessorService.FindConnectionsByEmployeeId(chatParticipantId);
                if (onlineConnections == null || !onlineConnections.Any()) {
                    return;
                }
                var onlineConnectionIds = onlineConnections.Select(x => x.ConnectionId).ToList();
                await _hubContext.Clients.Clients(onlineConnectionIds).SendAsync("MessageEdited", message);
            }

        }

        public async Task MessageDeletedNotification(MessageDto message, CancellationToken ct) {
            var chatParticipantIds = await _userRepository.GetChatParticipantsIdsByChatId(message.ChatId, ct);
            foreach (var chatParticipantId in chatParticipantIds) {
                var onlineConnections = await _hubConnectionsAccessorService.FindConnectionsByEmployeeId(chatParticipantId);
                if (onlineConnections == null || !onlineConnections.Any()) {
                    return;
                }
                var onlineConnectionIds = onlineConnections.Select(x => x.ConnectionId).ToList();
                await _hubContext.Clients.Clients(onlineConnectionIds).SendAsync("MessageDeleted", message);
            }
        }
    }
}

using System.Collections.Concurrent;
using Messenger.Application.Repositories;
using Microsoft.AspNetCore.SignalR;

namespace Messenger.Service.SignalR {
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
                var onlineConnections = _hubConnectionsAccessorService.FindConnectionsByEmployeeId(chatParticipantId);
                var onlineConnectionIds = onlineConnections.Select(x => x.ConnectionId).ToList();
                await _hubContext.Clients.Clients(onlineConnectionIds).SendAsync("NewMessage", message);
            }

        }
        public async Task MessageEditedNotification(MessageDto message, CancellationToken ct) {
            var chatParticipantIds = await _userRepository.GetChatParticipantsIdsByChatId(message.ChatId, ct);
            foreach (var chatParticipantId in chatParticipantIds) {
                var onlineConnections = _hubConnectionsAccessorService.FindConnectionsByEmployeeId(chatParticipantId);
                var onlineConnectionIds = onlineConnections.Select(x => x.ConnectionId).ToList();
                await _hubContext.Clients.Clients(onlineConnectionIds).SendAsync("MessageEdited", message);
            }

        }

        public async Task MessageDeletedNotification(MessageDto message, CancellationToken ct) {
            var chatParticipantIds = await _userRepository.GetChatParticipantsIdsByChatId(message.ChatId, ct);
            foreach (var chatParticipantId in chatParticipantIds) {
                var onlineConnections = _hubConnectionsAccessorService.FindConnectionsByEmployeeId(chatParticipantId);
                var onlineConnectionIds = onlineConnections.Select(x => x.ConnectionId).ToList();
                await _hubContext.Clients.Clients(onlineConnectionIds).SendAsync("MessageDeleted", message);
            }
        }
    }

    public class Connection {
        public string ConnectionId { get; protected set; }
        public Guid EmployeeId { get; protected set; }
        public static Connection InitFromContext(HubCallerContext context) {
            return new Connection {
                ConnectionId = context.ConnectionId,
                //EmployeeId =  (context.User.Identity as ClaimsIdentity)?.Claims. получение из Identity Id юзера
            };
        }

    }
    public class MessageDto {
        public Guid? MessageId { get; set; }
        public string Message { get; set; }
        public Guid ChatId { get; set; }
    }
    public class MessengerHub : Hub {
        public async Task SendMessage(MessageDto message) {
            //todo логика сохранения сообщения + отправка уведомления/сообщения в открытые подключения с этим чатом
        }
        public async Task EditMessage(MessageDto message) {
            //todo логика изменения  сообщения + отправка уведомления/сообщения в открытые подключения с этим чатом
        }
        public async Task DeleteMessage(MessageDto message) {
            //todo логика удаления  сообщения + отправка уведомления/сообщения в открытые подключения с этим чатом
        }

    }
    public interface IHubConnectionsAccessorService {
        List<Connection> FindConnectionsByEmployeeId(Guid employeeId);
        bool TryAddConnection(HubCallerContext context);
        bool TryRemoveConnection(HubCallerContext context);
    }

    public class HubConnectionsAccessorService : IHubConnectionsAccessorService {
        private ConcurrentDictionary<Guid, Connection> _connections { get; set; } = new ConcurrentDictionary<Guid, Connection>();
        public bool TryRemoveConnection(HubCallerContext context) {
            var connection = Connection.InitFromContext(context);
            return this._connections.TryRemove(connection.EmployeeId, out connection);
        }
        public bool TryAddConnection(HubCallerContext context) {
            var connection = Connection.InitFromContext(context);
            return this._connections.TryAdd(connection.EmployeeId, connection);
        }
        public List<Connection> FindConnectionsByEmployeeId(Guid employeeId) {
            var connections = _connections.Values.Where(x => x.EmployeeId == employeeId).ToList();
            return connections;
        }
    }


    public class ConnectionsFilter : IHubFilter {
        private readonly IHubConnectionsAccessorService _hubConnectionsAccessorService;

        public ConnectionsFilter(IHubConnectionsAccessorService hubConnectionsAccessorService) {
            _hubConnectionsAccessorService = hubConnectionsAccessorService;
        }

        public async ValueTask<object> InvokeMethodAsync(HubInvocationContext invocationContext,
            Func<HubInvocationContext, ValueTask<object>> next) {
            return await next(invocationContext);
        }

        public async Task OnConnectedAsync(HubLifetimeContext context, Func<HubLifetimeContext, Task> next) {
            var isAdded = _hubConnectionsAccessorService.TryAddConnection(context.Context);
            if (!isAdded) {
                await context.Hub.Clients.Client(context.Context.ConnectionId).SendAsync("TryReconnect");
                //status update
                await next(context);
            }
            await next(context);
        }

        public async Task OnDisconnectedAsync(HubLifetimeContext context, Exception exception, Func<HubLifetimeContext, Exception, Task> next) {
            _hubConnectionsAccessorService.TryRemoveConnection(context.Context);
            // status update
            await next(context, exception);
        }
    }
}

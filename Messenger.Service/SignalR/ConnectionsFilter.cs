using Messenger.Application.Models;
using Messenger.Application.Services;
using Microsoft.AspNetCore.SignalR;

namespace Messenger.Service.SignalR {
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
            var isAdded = await _hubConnectionsAccessorService.TryAddConnection(GetConnectionInfo(context.Context));
            if (!isAdded) {
                await context.Hub.Clients.Client(context.Context.ConnectionId).SendAsync("TryReconnect");
                await next(context);
            }
            await next(context);
        }

        public async Task OnDisconnectedAsync(HubLifetimeContext context, Exception exception, Func<HubLifetimeContext, Exception, Task> next) {
            await _hubConnectionsAccessorService.TryRemoveConnection(GetConnectionInfo(context.Context));
            await next(context, exception);
        }

        private ConnectionDto GetConnectionInfo(HubCallerContext context) {
            // identity не подкручен, поэтому работник у нас будет захардкожен
                // var employeeId = context.User.Identity.GetUserId();
                var employeeId = IdentityExtensions.EmployeeId;

            return new ConnectionDto {
                ConnectionId = context.ConnectionId,
                EmployeeId = employeeId
            };
        }
    }
}

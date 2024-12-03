using System.Collections.Concurrent;
using Messenger.Application;
using Messenger.Application.Models;
using Messenger.Application.Services;
using Microsoft.AspNetCore.SignalR;

namespace Messenger.Service.SignalR {
    public class HubConnectionsAccessorService : IHubConnectionsAccessorService {
        private ConcurrentDictionary<Guid, ConnectionDto> _connections { get; set; } = new ConcurrentDictionary<Guid, ConnectionDto>();
        public Task<bool> TryRemoveConnection(ConnectionDto connection) {
            return Task.FromResult(this._connections.TryRemove(connection.EmployeeId, out _));
        }
        public Task<bool> TryAddConnection(ConnectionDto connection) {
            return Task.FromResult(this._connections.TryAdd(connection.EmployeeId, connection ));
        }
        public Task<List<ConnectionDto>> FindConnectionsByEmployeeId(Guid employeeId) {
            var connections = _connections.Values.Where(x => x.EmployeeId == employeeId).ToList();
            return Task.FromResult(connections);
        }
    }
}

using Messenger.Application;
using Messenger.Application.Models;
using Messenger.Application.Services;
using Microsoft.AspNetCore.SignalR;

namespace Messenger.Service.SignalR;

public class RedisHubConnectionsAccessorService(IRedisDistributedCacheManager distributedCache)
    : IHubConnectionsAccessorService {
    public async Task<bool> TryRemoveConnection(ConnectionDto connection) {
        await distributedCache.RemoveFromArrayAsync($"con_{connection.EmployeeId}", connection.ConnectionId);
        return true;
    }
    public async Task<bool> TryAddConnection(ConnectionDto connection) {
        await distributedCache.AddToArrayAsync($"con_{connection.EmployeeId}", connection.ConnectionId);
        return true;
    }
    public async Task<List<ConnectionDto>> FindConnectionsByEmployeeId(Guid employeeId) {
        var connections = await distributedCache.GetArrayAsync($"con_{employeeId}");
        if (connections == null) {
            return new List<ConnectionDto>();
        }
        return connections.Select(x=> new ConnectionDto {
            ConnectionId = x, EmployeeId = employeeId
        }).ToList();
    }
}
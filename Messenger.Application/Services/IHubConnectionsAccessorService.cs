using Messenger.Application.Models;

namespace Messenger.Application.Services {
    public interface IHubConnectionsAccessorService {
        Task<List<ConnectionDto>> FindConnectionsByEmployeeId(Guid employeeId);
        Task<bool> TryAddConnection(ConnectionDto connection);
        Task<bool> TryRemoveConnection(ConnectionDto connection);
    }
}

using Messenger.Application;
using Messenger.Application.Enums;
using Messenger.Application.Services;

namespace Messenger.Infrastructure.Impl;

public class EmployeeStatusService(IHubConnectionsAccessorService hubConnectionsAccessorService) : IEmployeeStatusService {
    public async Task<Status> GetStatus(Guid employeeId) {
        var connections = await hubConnectionsAccessorService.FindConnectionsByEmployeeId(employeeId);
        if (connections == null || !connections.Any()) {
            return Status.Offline;
        }
        return Status.Online;
    }
}
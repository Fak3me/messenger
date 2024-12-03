using Messenger.Application.Enums;

namespace Messenger.Application.Services;

public interface IEmployeeStatusService {
    Task<Status> GetStatus(Guid employeeId);
}
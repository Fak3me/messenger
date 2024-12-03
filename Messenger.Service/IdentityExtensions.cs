using System.Security.Principal;

namespace Messenger.Service;

public static class IdentityExtensions {
    public static Guid GetUserId(this IIdentity identity) {
        // return Guid.Parse(identity.Name);
        return EmployeeId;
    }

    public static Guid EmployeeId = Guid.Parse("8f4f4d62-6c2c-4153-bb07-22aefeb41bdc");
    public static Guid SecondEmployee = Guid.Parse("8f4f4d62-6c2c-4153-bb07-22aefeb41bdc");
}
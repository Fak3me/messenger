using Messenger.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.Service.Controllers;

[Route("api/[controller]")]
[ApiController]
// [Authorize]
public class ChatsController(IChatService chatService) : ControllerBase {
    [HttpGet]
    public async Task<IActionResult> GetChats(CancellationToken ct) {
        var result = await chatService.GetChats(GetUserId(), ct);
        return Ok(result);
    }

    private Guid GetUserId() {
        // return this.User.Identity.GetUserId();
        return IdentityExtensions.EmployeeId;
    }
}
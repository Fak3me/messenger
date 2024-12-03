using Messenger.Application.Models;
using Messenger.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.Service.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize]
    public class MessagesController(IMessagesService messagesService) : ControllerBase {
        [HttpGet]
        // Просто, без пагинации итд, но нужно будет добавить и грузить уже не через чат, а через сообщения
        public async Task<IActionResult> GetMessages(Guid chatId, CancellationToken ct) {
            var result = await messagesService.GetMessages(chatId, ct);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessage([FromBody]MessageDto message, CancellationToken ct) {
            await messagesService.CreateMessage(message, GetUserId(), ct);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> EditMessage([FromBody]MessageDto message, CancellationToken ct) {
            await messagesService.EditMessage(message, ct);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteMessage([FromBody]MessageDto message, CancellationToken ct) {
            await messagesService.DeleteMessage(message, ct);
            return Ok();
        }

        private Guid GetUserId() {
            // return this.User.Identity.GetUserId();
            return IdentityExtensions.EmployeeId;
        }
    }
}
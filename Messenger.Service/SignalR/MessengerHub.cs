using Messenger.Application.Models;
using Messenger.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Messenger.Service.SignalR {
    public class MessengerHub(IMessagesService messagesService, IUnitOfWork unitOfWork, ILogger<MessengerHub> logger)
        : Hub {
        public async Task SendMessage(MessageDto dto) {
            try {
                await unitOfWork.BeginTransaction();
                await messagesService.CreateMessage(dto, GetUserId());
                await unitOfWork.CommitTransaction();
            }
            catch (Exception e) {
                logger.LogError(e, "Ошибка сохранения сообщения");
                await unitOfWork.RollbackTransaction();
                throw;
            }
        }

        public async Task EditMessage(MessageDto dto) {
            try {
                // var dto = new MessageDto { ChatId = chatId, MessageId = messageId, Message = message };
                await unitOfWork.BeginTransaction();
                await messagesService.EditMessage(dto);
                await unitOfWork.CommitTransaction();
            }
            catch (Exception e) {
                logger.LogError(e, "Ошибка изменения сообщения");
                await unitOfWork.RollbackTransaction();
                throw;
            }
        }

        public async Task DeleteMessage(MessageDto dto) {
            try {
                await unitOfWork.BeginTransaction();
                await messagesService.DeleteMessage(dto);
                await unitOfWork.CommitTransaction();
            }
            catch (Exception e) {
                logger.LogError(e, "Ошибка удаления сообщения");
                await unitOfWork.RollbackTransaction();
                throw;
            }
        }

        private Guid GetUserId() {
            // return this.User.Identity.GetUserId();
            return IdentityExtensions.EmployeeId;
        }
    }
}
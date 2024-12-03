using Messenger.Application.Services;
using Messenger.Domain;
using Microsoft.Extensions.Logging;

namespace Messenger.DataAccess;

public class DbInit(MessengerDbContext context, IUnitOfWork unitOfWork, ILogger<DbInit> logger) {
    public async Task Init() {
        if (!context.Set<Chat>().Any()) {
            try {
                await unitOfWork.BeginTransaction();
                var user1 = User.Create(Constants.EmployeeId, "login1", "email1@mail.com");
                var user2 = User.Create(Constants.SecondEmployee, "login2", "email2@mail.com");

                var chat1 = Chat.Create();
                chat1.AddParticipant(user1);
                chat1.AddParticipant(user2);
                var chat2 = Chat.Create();
                chat2.AddParticipant(user1);
                chat2.AddParticipant(user2);

                var message1 = Message.Create("Test Message", user1, chat1);
                var message2 = Message.Create("Test Message 2 Chat", user2, chat2);

                await context.AddRangeAsync(chat1, chat2);
                await context.AddRangeAsync(message1, message2);
                await context.AddRangeAsync(user2, user1);
                await context.SaveChangesAsync();
                await unitOfWork.CommitTransaction();
            }
            catch (Exception e) {
                logger.LogError(e, "ошибка сидирования");
                await unitOfWork.RollbackTransaction();
                throw;
            }
        }
    }
}
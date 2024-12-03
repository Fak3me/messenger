using Messenger.Application.Enums;
using Messenger.Application.Repositories;
using Messenger.Application.Services;

namespace Messenger.Service;

public class NotificationHostedService(IServiceProvider serviceProvider) : IHostedService, IDisposable {
    private Timer _timer;

    public Task StartAsync(CancellationToken cancellationToken) {
        _timer = new Timer(SendNotifications, null, TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(150));
        return Task.CompletedTask;
    }

    private async void SendNotifications(object state) {
        using (var scope = serviceProvider.CreateScope()) {
            var userRepository = scope.ServiceProvider.GetService<IUserRepository>();
            var unitOfWork = scope.ServiceProvider.GetService<IUnitOfWork>();
            var messageRepository = scope.ServiceProvider.GetService<IMessageRepository>();
            var employeeStatusService = scope.ServiceProvider.GetService<IEmployeeStatusService>();
            var mailSender = scope.ServiceProvider.GetService<IMailSender>();
            var logger = scope.ServiceProvider.GetService<ILogger<NotificationHostedService>>();
            try {
                await unitOfWork.BeginTransaction();

                var usersIds = await userRepository.GetUserIds();
                var offlineUserIds = new List<Guid>();
                foreach (var usersId in usersIds) {
                    if ((await employeeStatusService.GetStatus(usersId)) == Status.Offline) {
                        offlineUserIds.Add(usersId);
                    }
                }

                await foreach (var result in userRepository.GetUserMailsForUnreadMessagesNotification(
                                   offlineUserIds.ToArray())) {
                    //отправка сообщения можно вынести в отдельный сервис и отправлять сообщения по очереди в реббит и слушать на сервисе уведомления
                    //через консюмер, по задаче это не требуется поэтому просто используем сервис заглушку
                    if (result.messageIds.Any()) {
                        await mailSender.SendMailAsync(result.email,
                            $"У вас новые сообщение {string.Join(", ", result.messageIds)}");
                        foreach (var messageId in result.messageIds) {
                            var message = await messageRepository.GetByIdAsync(messageId);
                            var user = await userRepository.GetByIdAsync(result.userId);
                            //отмечаем, что по сообщению отправлено уведомление
                            message.SetNotificationSentByUser(user);
                            await messageRepository.UpdateAsync(message);
                        }
                    }
                }

                await unitOfWork.CommitTransaction();
            }
            catch (Exception e) {
                logger.LogError(e, "Ошибка отправки писем");
                await unitOfWork.RollbackTransaction();
                throw;
            }
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) {
        return Task.CompletedTask;
    }

    public void Dispose() {
        _timer?.Dispose();
    }
}
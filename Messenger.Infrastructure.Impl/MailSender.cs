using Messenger.Application.Services;
using Microsoft.Extensions.Logging;

namespace Messenger.Infrastructure.Impl;

public class MailSender(ILogger<MailSender> logger) : IMailSender {
    private ILogger<MailSender> _logger = logger;

    public Task SendMailAsync(string receiver, string message) {
        logger.LogInformation($"Отправлено сообщение - {message} на почту - {receiver}");
        return Task.CompletedTask;
    }
}
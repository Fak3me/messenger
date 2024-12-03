namespace Messenger.Application.Services;

public interface IMailSender {
    Task SendMailAsync(string receiver, string message);
}
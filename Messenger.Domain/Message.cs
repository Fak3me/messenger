namespace Messenger.Domain;

public class Message {
    public virtual Guid Id { get; protected set; }
    public virtual string Text { get; protected set; }
    public virtual DateTime Created { get; protected set; }
    public virtual User Creator { get; protected set; }
    public virtual bool IsDeleted { get; protected set; }
    public virtual IList<User> ReadBy { get; protected set; } = new List<User>();
    public virtual IList<User> NotificationReceivedBy { get; protected set; } = new List<User>();
    public virtual Chat Chat { get; protected set; }

    public static Message Create(string text, User creator, Chat chat) {
        var message = new Message
            { Id = Guid.NewGuid(), Text = text, Creator = creator, Chat = chat, Created = DateTime.Now };
        message.ReadBy.Add(creator);
        return message;
    }

    public void Delete() {
        IsDeleted = true;
    }

    public void ChangeMessage(string message) {
        Text = message;
    }

    public void SetNotificationSentByUser(User user) {
        if (NotificationReceivedBy.Any(x => x.Id == user.Id)) {
            return;
        }

        NotificationReceivedBy.Add(user);
    }
}
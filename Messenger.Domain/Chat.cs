namespace Messenger.Domain;

public class Chat {
    public virtual Guid Id { get; protected set; }
    public virtual IList<User> Participants { get; protected set; } = new List<User>();
    public virtual IList<Message> Messages { get; protected set; } = new List<Message>();

    public void AddParticipant(User participant) {
        if (!Participants.Contains(participant)) {
            Participants.Add(participant);
        }
    }

    public static Chat Create() {
        return new Chat { Id = Guid.NewGuid() };
    }
}
namespace Messanager.Domain {
    public class User {
        public virtual Guid Id { get; protected set; }
        public virtual string Login { get; protected set; }
        public virtual Status Status { get; protected set; }
        public virtual string Email { get; protected set; }
        public virtual IList<Chat> Chats { get; protected set; }
    }
    public class Message {
        public virtual Guid Id { get; protected set; }
        public virtual string Text { get; protected set; }
        public virtual User Creator { get; protected set; }
        public virtual bool IsDeleted { get; protected set; }
        public virtual IList<User> ReadBy { get; protected set; } = new List<User>();
        public virtual Chat Chat { get; protected set; }

        public void Delete() {
            IsDeleted = true;
        }
    }

    public class Chat {
        public virtual Guid Id { get; protected set; }
        public virtual IList<User> Participants { get; protected set; } = new List<User>();
        public virtual IList<Message> Messages { get; protected set; } = new List<Message>();
    }
    public enum Status {
        Online = 0,
        Offline = 1
    }
}

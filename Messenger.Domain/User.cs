namespace Messenger.Domain {
    public class User {
        public virtual Guid Id { get; protected set; }
        public virtual string Login { get; protected set; }
        public virtual string Email { get; protected set; }
        public virtual IList<Chat> Chats { get; protected set; }

        public static User Create(Guid id, string login, string email) {
            return new User { Id = id, Login = login, Email = email };
        }
    }
}
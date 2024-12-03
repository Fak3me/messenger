using Messenger.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Messenger.DataAccess.Configurations {
    public class UserConfig : IEntityTypeConfiguration<User> {
        public void Configure(EntityTypeBuilder<User> builder) {
            builder.HasKey(x => x.Id);
            builder.HasMany(x => x.Chats).WithMany(x => x.Participants).UsingEntity(x => x.ToTable("UsersToChats"));
        }
    }

    public class MessageConfig : IEntityTypeConfiguration<Message> {
        public void Configure(EntityTypeBuilder<Message> builder) {
            builder.HasKey(x => x.Id);
            builder.HasMany(x => x.ReadBy).WithMany().UsingEntity(x => x.ToTable("MessagesReadBy"));
            builder.HasMany(x => x.NotificationReceivedBy).WithMany().UsingEntity(x => x.ToTable("NewUnreadMessagesNotifiedBy"));
            builder.HasOne(x => x.Chat).WithMany(y => y.Messages);
        }
    }
    public class ChatConfig : IEntityTypeConfiguration<Chat> {
        public void Configure(EntityTypeBuilder<Chat> builder) {
            builder.HasKey(x => x.Id);
            builder.HasMany(x => x.Participants).WithMany(x=>x.Chats).UsingEntity(x => x.ToTable("UsersToChats"));
            builder.HasMany(x => x.Messages).WithOne(x=>x.Chat);
        }
    }
}

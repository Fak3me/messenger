using Messanager.Domain;
using Microsoft.EntityFrameworkCore;

namespace Messenger.DataAccess {
    public class MessengerDbContext : DbContext {
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MessengerDbContext).Assembly);

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Chat> Chats { get; set; }

    }
}
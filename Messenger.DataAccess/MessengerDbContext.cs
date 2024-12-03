using Messenger.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace Messenger.DataAccess {
    public class MessengerDbContext : DbContext {
        public MessengerDbContext(DbContextOptions<MessengerDbContext> opts) : base(opts) {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MessengerDbContext).Assembly);
            // var user1 = User.Create(Constants.EmployeeId, "login1", "email1@mail.com");
            // var user2 = User.Create(Constants.SecondEmployee, "login2", "email2@mail.com");
            //
            // var chat1 = Chat.Create();
            // chat1.AddParticipant(user1);
            // chat1.AddParticipant(user2);
            // var chat2 = Chat.Create();
            // chat2.AddParticipant(user1);
            // chat2.AddParticipant(user2);
            //
            // var message1 = Message.Create("Test Message", user1, chat1);
            // var message2 = Message.Create("Test Message 2 Chat", user2, chat1);
            //
            // modelBuilder.Entity<User>().HasData(user1, user2);
            // modelBuilder.Entity<Chat>().HasData(chat1, chat2);
            // modelBuilder.Entity<Message>().HasData(message1, message2);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Chat> Chats { get; set; }
    }

    public static class DependencyInjection {
        public static void AddDataAccessPostgresql(this IServiceCollection services, IConfiguration configuration) {
            var dataSourceBuilder = new NpgsqlDataSourceBuilder(configuration["PostgreDb:DefaultConnectionString"]);
            var dataSource = dataSourceBuilder.Build();

            services.AddDbContext<MessengerDbContext>(options =>
                options.UseNpgsql(dataSource), ServiceLifetime.Scoped);
        }
    }
}
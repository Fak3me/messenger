using Messanager.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace Messenger.DataAccess {
    public class MessengerDbContext : DbContext {
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MessengerDbContext).Assembly);

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
             options.UseNpgsql(dataSource), ServiceLifetime.Transient);
        }
    }
}
using Messenger.Application;
using Messenger.Infrastructure.Impl;
using Messenger.Application.Repositories;
using Messenger.Application.Services;
using Messenger.DataAccess;
using Messenger.DataAccess.Repositories;
using Messenger.Service;
using Messenger.Service.Middlewares;
using Messenger.Service.SignalR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddControllers(options => {
        options.Filters.Add<ExceptionFilter>();
    });

builder.Host.UseSerilog((context, provider, config) => {
    config
        .ReadFrom.Configuration(context.Configuration)
        .Enrich.WithProperty("app-name", "messenger-api")
        .Enrich.FromLogContext();
}, writeToProviders: true);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    cfg => {
        cfg.EnableAnnotations();
        cfg.DescribeAllParametersInCamelCase();
        cfg.SwaggerDoc("v1", new() {
            Title = "messenger API",
        });
    });

builder.Services.AddDataAccessPostgresql(builder.Configuration);

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddScoped<IChatRepository, ChatRepository>();

builder.Services.AddScoped<IMessagesService, MessagesService>();
builder.Services.AddScoped<IChatService, ChatService>();
builder.Services.AddScoped<IEmployeeStatusService, EmployeeStatusService>();
builder.Services.AddScoped<IMailSender, MailSender>();

builder.Services.AddScoped<IRedisDistributedCacheManager, RedisDistributedCacheManager>();
builder.Services.AddDistributedMemoryCache();

builder.Services.Configure<RedisOptions>(options => builder.Configuration.GetSection("RedisOptions").Bind(options));
builder.Services.AddSingleton(x => x.GetService<IOptions<RedisOptions>>()!.Value);

builder.Services.AddHostedService<NotificationHostedService>();

builder.Services.AddSignalR(hubOptions => {
    hubOptions.EnableDetailedErrors = true;
    hubOptions.KeepAliveInterval = TimeSpan.FromSeconds(15);
    hubOptions.ClientTimeoutInterval = TimeSpan.FromMinutes(1);
}).AddHubOptions<MessengerHub>(options => { options.AddFilter<ConnectionsFilter>(); });

builder.Services.AddScoped<IHubConnectionsAccessorService, RedisHubConnectionsAccessorService>();
builder.Services.AddScoped<IMessengerClientService, MessengerClientService>();
builder.Services.AddScoped<DbInit>();

var app = builder.Build();
using (var scope = app.Services.CreateScope()) {
    var dbInit = scope.ServiceProvider.GetService<DbInit>();
    await dbInit.Init();

}
app.MapGet("/", () => "Hello World!");
app.UseSwagger();
app.UseSwaggerUI();

app.UseMiddleware<DbTransactionMiddleware>();
app.UseAuthorization();
app.MapHub<MessengerHub>("/messengerHub");
app.MapControllers();
app.Run();

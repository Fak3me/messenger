using Messenger.Application.Repositories;
using Messenger.DataAccess;
using Messenger.DataAccess.Repositories;
using Messenger.Service;
using Messenger.Service.SignalR;
using Microsoft.AspNetCore.SignalR;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddControllers(options => {
        options.Filters.Add<ExceptionFilter>();
    });

builder.Services.AddDataAccessPostgresql(builder.Configuration);
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();


builder.Services.AddSignalR(hubOptions => {
    hubOptions.EnableDetailedErrors = true;
    hubOptions.KeepAliveInterval = TimeSpan.FromSeconds(15);
    hubOptions.ClientTimeoutInterval = TimeSpan.FromMinutes(1);
}).AddHubOptions<MessengerHub>(options => { options.AddFilter<ConnectionsFilter>(); });

builder.Services.AddSingleton<IHubConnectionsAccessorService, HubConnectionsAccessorService>();
builder.Services.AddScoped<IMessengerClientService, MessengerClientService>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();

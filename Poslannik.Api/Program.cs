using Microsoft.EntityFrameworkCore;
using Poslannik.Api.Hubs;
using Poslannik.DataBase;
using Poslannik.DataBase.Repositories;
using Poslannik.DataBase.Repositories.Interfaces;
using Poslannik.Framework.Hubs;

var builder = WebApplication.CreateBuilder(args);

IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

var dbContextOptions = new DbContextOptionsBuilder().UseNpgsql(configuration.GetConnectionString("PostgreSQL")).Options;

// Регистрация зависимостей

builder.Services
    .AddTransient(x => configuration)
    .AddTransient(x => dbContextOptions)
    // Регистрация БД
    .AddDbContext<ApplicationContext>()
    .AddTransient<IUserRepository, UserRepository>()
    .AddTransient<IMessageRepository, MessageRepository>()
    .AddTransient<IChatRepository, ChatRepository>()
    .AddTransient<IChatParticipantRepository, ChatParticipantRepository>();

var app = builder.Build();

app.MapHub<AuthorizationHub>(HubConstants.AuthorizationHubPath);

app.Run();

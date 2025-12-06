using Microsoft.AspNetCore.Authentication.JwtBearer;
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

builder.Services.AddSignalR(options => options.MaximumParallelInvocationsPerClient = 10);
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();

builder.Services.AddTransient(x => configuration);

builder.Services.AddDbContext<ApplicationContext>(option => option.UseNpgsql(configuration.GetConnectionString("PostgreSQL")));
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IMessageRepository, MessageRepository>();
builder.Services.AddTransient<IChatRepository, ChatRepository>();
builder.Services.AddTransient<IChatParticipantRepository, ChatParticipantRepository>();

var app = builder.Build();

//var userRepo = app.Services.CreateScope().ServiceProvider.GetRequiredService<IUserRepository>();
//var userRepoImp = (UserRepository)userRepo;
//await userRepoImp.AddTestUserAsync();

app.UseAuthentication();
app.UseAuthorization();

app.MapHub<AuthorizationHub>(HubConstants.AuthorizationHubPath);

app.Run();

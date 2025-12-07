using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Poslannik.Api.Hubs;
using Poslannik.DataBase;
using Poslannik.DataBase.Repositories;
using Poslannik.DataBase.Repositories.Interfaces;
using Poslannik.Framework.Hubs;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

builder.Services.AddSignalR(options => options.MaximumParallelInvocationsPerClient = 10);
builder.Services.AddAuthorization();

var jwtSecretKey = configuration["Jwt:SecretKey"]!;
var jwtIssuer = configuration["Jwt:Issuer"]!;
var jwtAudience = configuration["Jwt:Audience"]!;

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey))
        };

        // Настройка для SignalR - получение токена из query string
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                var path = context.HttpContext.Request.Path;

                // Если запрос к SignalR хабу и есть токен в query string
                if (!string.IsNullOrEmpty(accessToken) &&
                    (path.StartsWithSegments(HubConstants.ChatHubPath) ||
                     path.StartsWithSegments(HubConstants.AuthorizationHubPath) ||
                     path.StartsWithSegments(HubConstants.UserHubPath)))
                {
                    context.Token = accessToken;
                }

                return Task.CompletedTask;
            }
        };
    });

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
app.MapHub<ChatHub>(HubConstants.ChatHubPath);
app.MapHub<UserHub>(HubConstants.UserHubPath);

app.Run();

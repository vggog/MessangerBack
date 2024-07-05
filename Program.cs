using System.Configuration;
using Microsoft.AspNetCore.Identity;
using MessangerBack.DataBase;
using Microsoft.EntityFrameworkCore;
using MessangerBack.Services;
using MessangerBack.Repositories;
using Microsoft.Extensions.Configuration;
using MessangerBack.Utils;
using MessangerBack.Options;
using MessangerBack.Extentions;
using MessangerBack.Hubs;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JwtOptions"));
builder.Services.Configure<EmailOptions>(builder.Configuration.GetSection("EmailOptions"));

builder.Services.AddDbContext<DataBaseContext>(
    options => 
        options.UseNpgsql(
            builder.Configuration.GetConnectionString("DefaultConnection")
            )
        );

builder.Services.AddIdentity<IdentityUser<Guid>, IdentityRole<Guid>>()
                .AddEntityFrameworkStores<DataBaseContext>();

// Add services to the container.
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost"; // Update this with your Redis server address
    options.InstanceName = "SampleInstance";
});

builder.Services.AddScoped<IPasswordUtils, PasswordUtils>();
builder.Services.AddScoped<IJwtProvider, JwtProvider>();

builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<IChangePasswordService, ChangePasswordService>();

builder.Services.AddScoped<IChatService, ChatService>();
builder.Services.AddScoped<IChatRepository, ChatRepository>();

builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();

builder.Services.AddApiAuthentication(builder.Configuration);

builder.Services.AddCors( options => 
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:3000", "http://192.168.0.109:3000")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

builder.Services.AddStackExchangeRedisCache(options => {
    var connection = builder.Configuration.GetConnectionString("Redis");

    options.Configuration = connection;
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();

app.MapHub<ChatHub>("/api/ChatMessages");

app.Run();

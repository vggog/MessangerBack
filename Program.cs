using System.Configuration;
using MessangerBack.DataBase;
using Microsoft.EntityFrameworkCore;
using MessangerBack.Services;
using MessangerBack.Repositories;
using Microsoft.Extensions.Configuration;
using MessangerBack.Utils;
using MessangerBack.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JwtOptions"));

builder.Services.AddDbContext<DataBaseContext>(
    options => 
        options.UseNpgsql(
            builder.Configuration.GetConnectionString("DefaultConnection")
            )
        );

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPasswordUtils, PasswordUtils>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IJwtProvider, JwtProvider>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();

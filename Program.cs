using Microsoft.AspNetCore.Identity;
using MessangerBack.DataBase;
using Microsoft.EntityFrameworkCore;
using MessangerBack.Services;
using MessangerBack.Repositories;
using MessangerBack.Utils;
using MessangerBack.Options;
using MessangerBack.Extentions;
using MessangerBack.Hubs;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.OpenSearch;
using AutoRegisterTemplateVersion = Serilog.Sinks.OpenSearch.AutoRegisterTemplateVersion;
using CertificateValidations = OpenSearch.Net.CertificateValidations;
using System.Net;
using Serilog.Formatting;
using Newtonsoft.Json;


var builder = WebApplication.CreateBuilder(args);
//builder.Logging.ClearProviders();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });

    // Добавьте этот код для добавления заголовка авторизации
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});
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

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost";
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

builder.Services.AddCors(options => 
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

Serilog.Debugging.SelfLog.Enable(msg => Console.WriteLine(msg));
ServicePointManager.ServerCertificateValidationCallback = (o, certificate, chain, errors) => true;
ServicePointManager.ServerCertificateValidationCallback = CertificateValidations.AllowAll;

var logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.OpenSearch(new OpenSearchSinkOptions(new Uri("https://localhost:9200"))
    {
        AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.OSv1,
        MinimumLogEventLevel = (LogEventLevel)Enum.Parse(typeof(LogEventLevel), builder.Configuration["Serilog:MinimumLevel:Default"]),
        TypeName = "_doc",
        InlineFields = false,
        ModifyConnectionSettings = x =>
            x.BasicAuthentication("admin", "bars123@superMyPassword")
                .ServerCertificateValidationCallback(CertificateValidations.AllowAll)
                .ServerCertificateValidationCallback((o, certificate, chain, errors) => true),
        IndexFormat = "my-index-{0:yyyy.MM.dd}",
    })
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

var app = builder.Build();

app.Logger.LogInformation("starting up");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI( c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API V1");
        c.OAuthClientId("swagger");
        c.OAuthAppName("Swagger");
    });
}

app.UseCors();

app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();

app.MapHub<ChatHub>("/api/ChatMessages");

app.Run();
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MoodProject.Api;
using MoodProject.Api.Configuration;
using MoodProject.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuration
builder.Services.AddSingleton(provider => provider.GetService<IConfiguration>().GetSection("Authentication:Schemes:Bearer").Get<AuthConfiguration>());

// Security
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();

// SignalR
builder.Services.AddSignalR();
builder.Services.AddHostedService<MedicationsNotifier>();

// Notification Service
builder.Services.AddSingleton<NotificationService>();

// CORS Policy
builder.Services.AddCors(c => c.AddPolicy("dev", builder =>
{
    builder
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();
}));

// Database
builder.Services.AddDbContext<MoodProjectContext>();

// Custom injections
builder.Services.AddSingleton(sp => new HttpClient());
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("dev");

app.UseHttpsRedirection();

// SignalR
app.UseAuthorization();

app.MapHub<NotificationsHub>("notifications");

app.MapControllers();

app.Run();
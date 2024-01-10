using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Logging.AzureAppServices;
using Microsoft.IdentityModel.Tokens;
using MoodProject.Api;
using MoodProject.Api.Configuration;
using MoodProject.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// API
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuration
builder.Services.AddSingleton(provider => provider.GetService<IConfiguration>().GetSection("Authentication:Schemes:Bearer").Get<AuthConfiguration>());
builder.Services.AddSingleton(provider => provider.GetService<IConfiguration>().GetSection("Authority").Get<AuthorityConfiguration>());
builder.Services.AddSingleton(provider => provider.GetService<IConfiguration>().GetSection("Notification").Get<NotificationConfiguration>());
builder.Services.AddSingleton(provider => provider.GetService<IConfiguration>().GetSection("FileStorage").Get<FileStorageConfiguration>());


// Security
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["Authentication:Schemes:Bearer:ValidIssuer"],
        ValidAudiences = builder.Configuration.GetSection("Authentication:Schemes:Bearer:ValidAudiences").Get<string[]>(),
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Authentication:Schemes:Bearer:Secret"]))
    };
});

// Logging
builder.Services.AddLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
    logging.AddFile("logs/api-{Date}.txt");
    logging.AddDebug();
    logging.AddAzureWebAppDiagnostics();
}).Configure<AzureFileLoggerOptions>(options =>
{
    options.FileName = "api-log";
    options.FileSizeLimit = 50 * 1024;
    options.RetainedFileCountLimit = 30;
});

// SignalR
builder.Services.AddSignalR();
builder.Services.AddHostedService<MedicationsNotifier>();

// Services
builder.Services.AddScoped<NotificationService>();
builder.Services.AddSingleton<AuthorizationService>();

// CORS Policy
builder.Services.AddCors(c => c.AddDefaultPolicy(corsBuilder =>
{
    corsBuilder
        .WithOrigins(builder.Configuration.GetSection("Authentication:Schemes:Bearer:ValidAudiences").Get<string[]>() ?? Array.Empty<string>())
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

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapHub<NotificationsHub>("notifications");

app.MapControllers();

app.Run();
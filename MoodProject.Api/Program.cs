using Microsoft.EntityFrameworkCore;
using MoodProject.Api;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS Policy
builder.Services.AddCors(c => c.AddPolicy("dev", builder =>
{
    builder
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();
}));

// MySQL
var connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=moodproject;";
var version = new MySqlServerVersion(new Version(10, 4, 11));
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

app.UseAuthorization();

app.MapControllers();

app.Run();
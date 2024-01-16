using Microsoft.EntityFrameworkCore;
using MoodProject.Api.Configuration;
using MoodProject.Core.Models;
using MoodProject.Core.Models.Notifications;

namespace MoodProject.Api;

public class MoodProjectContext : DbContext
{
    private DatabaseConfiguration Config { get; init; }
    public DbSet<SymptomType> SymptomTypes { get; set; }
    public DbSet<Symptom> Symptoms { get; set; }
    public DbSet<FactorValue> FactorValues { get; set; }
    public DbSet<CustomQuizzQuestion> QuizzQuestions { get; set; }
    public DbSet<QuizzAnswer> QuizzAnswers { get; set; }
    public DbSet<ChatRoom> ChatRooms { get; set; }
    public DbSet<ChatRoomPost> ChatRoomPosts { get; set; }
    public DbSet<ChatRoomComment> ChatRoomComments { get; set; }
    public DbSet<Medication> Medications { get; set; }
    public DbSet<MedicationDayUsage> MedicationDayUsages { get; set; }
    public DbSet<NotificationSubscription> NotificationSubscriptions { get; set; }
    public DbSet<Resource> Resources { get; set; }
    public DbSet<ResourceFile> ResourceFiles { get; set; }

    public MoodProjectContext(DatabaseConfiguration configuration)
    {
        Config = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(Config.ConnectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
    }
}
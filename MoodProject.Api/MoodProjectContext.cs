using Microsoft.EntityFrameworkCore;
using MoodProject.Core;
using MoodProject.Core.Models;

namespace MoodProject.Api;

public class MoodProjectContext : DbContext
{
    public DbSet<User> Users { get; set; }
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

    public MoodProjectContext()
    {
        
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        /* old mysql
        var connectionString = "Server=localhost; User ID=root; Password=; Database=moodproject";
        optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        */
        const string connectionString =
            "Host=flora.db.elephantsql.com;Database=einxmlgw;Username=einxmlgw;Password=l-zqSmHY366YOxf_iZTy3SipGL9rmjkp;Include Error Detail=true";
        optionsBuilder.UseNpgsql(connectionString);

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        /*

        // Symptom <-> SymptomType
        modelBuilder.Entity<Symptom>()
            .HasOne(symptom => symptom.Type)
            .WithMany()
            .HasForeignKey(symptom => symptom.TypeId)
            .IsRequired();

        // Symptom <-> FactorValue
        modelBuilder.Entity<Symptom>()
            .HasMany(symptom => symptom.ValuesHistory)
            .WithOne()
            .HasForeignKey(value => value.SymptomId)
            .IsRequired();
        */
        /*
        modelBuilder.Entity<FactorValue>()
            .HasOne(value => value.Symptom)
            .WithMany(symptom => symptom.HarmfulnessFactorValuesHistory)
            .OnDelete(DeleteBehavior.NoAction);
        
        modelBuilder.Entity<FactorValue>()
            .HasOne(value => value.Symptom)
            .WithMany(symptom => symptom.PresenceFactorValuesHistory)
            .OnDelete(DeleteBehavior.NoAction);
        */
        
        //base.OnModelCreating(modelBuilder);
    }
}
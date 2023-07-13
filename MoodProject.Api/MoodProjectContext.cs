using Microsoft.EntityFrameworkCore;
using MoodProject.Core;

namespace MoodProject.Api;

public class MoodProjectContext : DbContext
{
    public DbSet<SymptomType> SymptomTypes { get; set; }
    public DbSet<Symptom> Symptoms { get; set; }
    public DbSet<FactorValue> FactorValues { get; set; }
    public MoodProjectContext()
    {
        
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = "Server=localhost; User ID=root; Password=; Database=moodproject";
        optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Symptom <-> SymptomType
        modelBuilder.Entity<Symptom>()
            .HasOne(symptom => symptom.Type)
            .WithMany()
            .HasForeignKey(symptom => symptom.TypeId)
            .IsRequired();

        // Symptom <-> FactorValue
        modelBuilder.Entity<Symptom>()
            .HasMany(symptom => symptom.ValuesHistory)
            .WithOne(value => value.Symptom)
            .HasForeignKey(value => value.SymptomId)
            .IsRequired();

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
        
        base.OnModelCreating(modelBuilder);
    }
}
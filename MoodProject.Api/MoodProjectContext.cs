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

    public MoodProjectContext()
    {
        
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        const string connectionString =
            "Host=flora.db.elephantsql.com;Database=einxmlgw;Username=einxmlgw;Password=l-zqSmHY366YOxf_iZTy3SipGL9rmjkp;Include Error Detail=true";
        optionsBuilder.UseNpgsql(connectionString);

    }
}
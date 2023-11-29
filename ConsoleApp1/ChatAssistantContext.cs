using ChatAssistant.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ConsoleApp1
{
    public class ChatAssistantContext : DbContext
    {
        public DbSet<Agent> Agents { get; set; }
        public DbSet<ChatSession> ChatSessions { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<Team> Teams { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=ChatAssistant.db");
    }
}

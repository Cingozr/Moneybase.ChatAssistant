using ChatAssistant.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ChatAssistant.Infrastructure.Data
{
    public class ChatAssistantDbContext  : DbContext
    {
        public ChatAssistantDbContext(DbContextOptions<ChatAssistantDbContext> options) : base(options)
        {
            
        } 
        public DbSet<Agent> Agents { get; set; }
        public DbSet<ChatSession> ChatSessions { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<Team> Teams { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        { 
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        } 
    }


}

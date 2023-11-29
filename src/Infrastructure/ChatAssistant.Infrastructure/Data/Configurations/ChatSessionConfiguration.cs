using ChatAssistant.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatAssistant.Infrastructure.Data.Configurations
{
    public class ChatSessionConfiguration : IEntityTypeConfiguration<ChatSession>
    {
        public void Configure(EntityTypeBuilder<ChatSession> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.ConnectionId)
                .IsRequired();

            builder.Property(s => s.AgentId).HasDefaultValue(0);

            builder.Property(s => s.QueueTime)
                .IsRequired();

            builder.Property(s => s.IsAssigned)
                .IsRequired();

            builder.Property(s => s.IsActive)
                .IsRequired();
             

        }
    }
}

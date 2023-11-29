using ChatAssistant.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatAssistant.Infrastructure.Data.Configurations
{
    public class AgentConfiguration : IEntityTypeConfiguration<Agent>
    {
        public void Configure(EntityTypeBuilder<Agent> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.AgentName)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasOne(a => a.Team)
                .WithMany(t => t.Agents)
                .HasForeignKey(a => a.TeamId);
             


            builder.Ignore(a => a.Efficiency);  
        }
    }
}

using ChatAssistant.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatAssistant.Infrastructure.Data.Configurations
{
    public class TeamConfiguration : IEntityTypeConfiguration<Team>
    {
        public void Configure(EntityTypeBuilder<Team> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.TeamName)
                .IsRequired()
                .HasMaxLength(20);

            builder.HasMany(t => t.Agents)
                .WithOne(a => a.Team)
                .HasForeignKey(a => a.TeamId);
             
        }
    }
}

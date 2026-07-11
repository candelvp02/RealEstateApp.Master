using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstateApp.Core.Domain.Entities;

namespace RealEstateApp.Infrastructure.Persistence.EntityConfigurations;

public class MessageEntityConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.ToTable("Messages");
        builder.Property(m => m.Content).IsRequired().HasMaxLength(1000);
        builder.Property(m => m.SenderRole).IsRequired().HasMaxLength(20);
        builder.Property(m => m.ClientId).IsRequired();
        builder.Property(m => m.AgentId).IsRequired();
    }
}
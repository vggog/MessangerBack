using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MessangerBack.Models;

namespace MessangerBack.Configurations;

public class ChatConfiguration : IEntityTypeConfiguration<ChatModel>
{
    public void Configure(EntityTypeBuilder<ChatModel> builder)
    {
        builder
            .HasOne(c => c.LastMessage);

        builder 
            .HasMany(c => c.Messages)
            .WithOne(m => m.Chat);

        builder
            .HasOne(c => c.Admin)
            .WithMany(u => u.ChatWhereIsTheAdmin);
    }
}
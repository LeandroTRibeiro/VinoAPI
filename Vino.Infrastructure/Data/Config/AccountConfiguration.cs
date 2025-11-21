using BetterThanYou.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BetterThanYou.Infrastructure.Data.Config;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.ToTable("Account");
        
        builder.HasKey(x => x.Id);
        
        builder.HasIndex(x => x.Email).IsUnique();
        
        builder.Property(e => e.Email)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.PasswordHash)
            .IsRequired();
        
        builder.Property(e => e.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("NOW()");

        builder.Property(e => e.UpdatedAt)
            .IsRequired(false);

    }
}
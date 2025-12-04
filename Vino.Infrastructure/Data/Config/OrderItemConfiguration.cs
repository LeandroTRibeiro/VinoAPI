using BetterThanYou.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BetterThanYou.Infrastructure.Data.Config;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("OrderItems");
        
        builder.HasKey(oi => oi.Id);
        
        builder.Property(oi => oi.OrderId)
            .IsRequired();
        
        builder.Property(oi => oi.ProdutoId)
            .IsRequired();
        
        builder.Property(oi => oi.Quantidade)
            .IsRequired();
        
        builder.Property(oi => oi.PrecoUnitario)
            .IsRequired()
            .HasPrecision(18, 2);
        
        builder.Property(oi => oi.Subtotal)
            .IsRequired()
            .HasPrecision(18, 2);
        
        // Relacionamentos
        builder.HasOne(oi => oi.Produto)
            .WithMany()
            .HasForeignKey(oi => oi.ProdutoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
using BetterThanYou.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BetterThanYou.Infrastructure.Data.Config;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");
        
        builder.HasKey(o => o.Id);
        
        builder.Property(o => o.NumeroOrder)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.HasIndex(o => o.NumeroOrder)
            .IsUnique();
        
        builder.Property(o => o.ClienteId)
            .IsRequired();
        
        builder.Property(o => o.DataPedido)
            .IsRequired();
        
        builder.Property(o => o.Status)
            .IsRequired();
        
        builder.Property(o => o.ValorTotal)
            .IsRequired()
            .HasPrecision(18, 2);
        
        builder.Property(o => o.Observacoes)
            .HasMaxLength(1000);
        
        builder.Property(o => o.EnderecoEntrega)
            .HasMaxLength(500);
        
        builder.Property(o => o.CriadoPor)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(o => o.DataCriacao)
            .IsRequired()
            .HasDefaultValueSql("NOW()");
        
        builder.Property(o => o.ModificadoPor)
            .HasMaxLength(100);
        
        builder.Property(o => o.Ativo)
            .IsRequired()
            .HasDefaultValue(true);
        
        // Relacionamentos
        builder.HasOne(o => o.Cliente)
            .WithMany()
            .HasForeignKey(o => o.ClienteId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasMany(o => o.Itens)
            .WithOne(i => i.Order)
            .HasForeignKey(i => i.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
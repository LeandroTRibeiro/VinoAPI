using BetterThanYou.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BetterThanYou.Infrastructure.Data.Config;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Product");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(e => e.Nome)
            .IsRequired()
            .HasMaxLength(200);
        
        builder.Property(e => e.Descricao)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(e => e.QuantidadeEstoque)
            .IsRequired();

        builder.Property(e => e.PrecoCusto)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(e => e.PrecoVenda)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(e => e.FotoUrl)
            .IsRequired(false)
            .HasMaxLength(500);

        builder.Property(e => e.CriadoPor)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.DataCriacao)
            .IsRequired()
            .HasDefaultValueSql("NOW()");

        builder.Property(e => e.ModificadoPor)
            .IsRequired(false)
            .HasMaxLength(100);

        builder.Property(e => e.DataModificacao)
            .IsRequired(false);

        builder.Property(e => e.Ativo)
            .IsRequired()
            .HasDefaultValue(true);
    }
}
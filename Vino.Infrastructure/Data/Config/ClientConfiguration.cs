using BetterThanYou.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BetterThanYou.Infrastructure.Data.Config;

public class ClientConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.ToTable("Clients");
        
        builder.HasKey(c => c.Id);
        
        builder.Property(c => c.NomeFantasia)
            .IsRequired()
            .HasMaxLength(200);
        
        builder.Property(c => c.RazaoSocial)
            .HasMaxLength(200);
        
        builder.Property(c => c.Email)
            .HasMaxLength(100);
        
        builder.Property(c => c.CpfCnpj)
            .HasMaxLength(20);
        
        builder.Property(c => c.Endereco)
            .HasMaxLength(300);
        
        builder.Property(c => c.Cidade)
            .HasMaxLength(100);
        
        builder.Property(c => c.Estado)
            .HasMaxLength(2);
        
        builder.Property(c => c.Cep)
            .HasMaxLength(10);
        
        builder.Property(c => c.Telefones)
            .HasConversion(
                v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions)null),
                v => System.Text.Json.JsonSerializer.Deserialize<List<ContactPhone>>(v, (System.Text.Json.JsonSerializerOptions)null) ?? new List<ContactPhone>()
            )
            .HasColumnType("jsonb");
        
        builder.Property(c => c.CriadoPor)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(c => c.DataCriacao)
            .IsRequired()
            .HasDefaultValueSql("NOW()");
        
        builder.Property(c => c.ModificadoPor)
            .HasMaxLength(100);
        
        builder.Property(c => c.Ativo)
            .IsRequired()
            .HasDefaultValue(true);
        
        builder.Property(c => c.FotoUrl)
            .HasMaxLength(500);
    }
}
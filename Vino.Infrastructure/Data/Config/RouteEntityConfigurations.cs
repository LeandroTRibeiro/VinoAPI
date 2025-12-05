using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BetterThanYou.Core.Entities;

namespace BetterThanYou.Infrastructure.Data.Configurations;

public class RouteConfiguration : IEntityTypeConfiguration<Route>
{
    public void Configure(EntityTypeBuilder<Route> builder)
    {
        builder.ToTable("Routes");
        
        builder.HasKey(r => r.Id);
        
        builder.Property(r => r.Nome)
            .IsRequired()
            .HasMaxLength(200);
        
        builder.Property(r => r.Descricao)
            .HasMaxLength(1000);
        
        builder.Property(r => r.DataRota)
            .IsRequired();
        
        builder.Property(r => r.Status)
            .IsRequired()
            .HasConversion<int>();
        
        builder.Property(r => r.EnderecoPartida)
            .HasMaxLength(500);
        
        builder.Property(r => r.CidadePartida)
            .HasMaxLength(100);
        
        builder.Property(r => r.EstadoPartida)
            .HasMaxLength(2);
        
        builder.Property(r => r.CepPartida)
            .HasMaxLength(10);
        
        builder.Property(r => r.LatitudePartida)
            .HasPrecision(10, 8);
        
        builder.Property(r => r.LongitudePartida)
            .HasPrecision(11, 8);
        
        builder.Property(r => r.DistanciaTotal)
            .HasPrecision(10, 2);
        
        builder.Property(r => r.CriadoPor)
            .IsRequired()
            .HasMaxLength(200);
        
        builder.Property(r => r.ModificadoPor)
            .HasMaxLength(200);
        
        builder.Property(r => r.Ativo)
            .IsRequired()
            .HasDefaultValue(true);
        
        builder.HasMany(r => r.Paradas)
            .WithOne(p => p.Route)
            .HasForeignKey(p => p.RouteId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasIndex(r => r.DataRota);
        builder.HasIndex(r => r.Status);
        builder.HasIndex(r => new { r.Ativo, r.DataRota });
    }
}

public class RouteStopConfiguration : IEntityTypeConfiguration<RouteStop>
{
    public void Configure(EntityTypeBuilder<RouteStop> builder)
    {
        builder.ToTable("RouteStops");
        
        builder.HasKey(rs => rs.Id);
        
        builder.Property(rs => rs.Latitude)
            .HasPrecision(10, 8);
        
        builder.Property(rs => rs.Longitude)
            .HasPrecision(11, 8);
        
        builder.Property(rs => rs.DistanciaParadaAnterior)
            .HasPrecision(10, 2);
        
        builder.Property(rs => rs.Observacoes)
            .HasMaxLength(1000);
        
        builder.HasOne(rs => rs.Route)
            .WithMany(r => r.Paradas)
            .HasForeignKey(rs => rs.RouteId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(rs => rs.Cliente)
            .WithMany()
            .HasForeignKey(rs => rs.ClienteId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasIndex(rs => rs.RouteId);
        builder.HasIndex(rs => rs.ClienteId);
        builder.HasIndex(rs => new { rs.RouteId, rs.OrdemOtimizada });
    }
}
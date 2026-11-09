using Inventario.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventario.Api.Data.Configurations;

public class EtiquetaConfiguration : IEntityTypeConfiguration<Etiqueta>
{
    public void Configure(EntityTypeBuilder<Etiqueta> builder)
    {
        builder.ToTable("Etiquetas");

        builder.Property(e => e.Nombre)
               .IsRequired()
               .HasMaxLength(30);

        builder.HasIndex(e => e.Nombre)
               .IsUnique();

        builder.HasData(
            new { Id = 1, Nombre = "Inalámbrico" },
            new { Id = 2, Nombre = "Oferta" },
            new { Id = 3, Nombre = "Gamer" }
        );
    }
}
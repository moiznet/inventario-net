using Inventario.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventario.Api.Data.Configurations;

public class CategoriaConfiguration : IEntityTypeConfiguration<Categoria>
{
    public void Configure(EntityTypeBuilder<Categoria> builder)
    {
        builder.ToTable("Categorias");

        builder.Property(c => c.Nombre)
               .IsRequired()
               .HasMaxLength(50);

        builder.HasIndex(c => c.Nombre)
               .IsUnique();

        builder.HasData(
            new { Id = 1, Nombre = "Periféricos" },
            new { Id = 2, Nombre = "Pantallas" },
            new { Id = 3, Nombre = "Audio" }
        );
    }
}
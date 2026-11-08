using Inventario.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventario.Api.Data.Configurations;

public class ProductoConfiguration : IEntityTypeConfiguration<Producto>
{
    public void Configure(EntityTypeBuilder<Producto> builder)
    {
        builder.ToTable("Productos", t =>
        {
            t.HasCheckConstraint("CK_Productos_Precio", "[Precio] > 0");
            t.HasCheckConstraint("CK_Productos_Stock", "[Stock] >= 0");
        });

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Nombre)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(p => p.Precio)
               .HasPrecision(18, 2);

        builder.HasOne(p => p.Categoria)          // un producto tiene UNA categoría
               .WithMany(c => c.Productos)        // una categoría tiene MUCHOS productos
               .HasForeignKey(p => p.CategoriaId) // unidos por esta columna
               .OnDelete(DeleteBehavior.Restrict);
    }
}
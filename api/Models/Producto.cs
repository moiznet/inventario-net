namespace Inventario.Api.Models;

public class Producto
{
    
  
     public int Id { get; private set; }
    public string Nombre { get; private set; } = string.Empty;
    public decimal Precio { get; private set; }
    public int Stock { get; private set; }
    public int CategoriaId { get; private set; }

    public Categoria? Categoria { get; private set; }     // ← navegación: su categoría
    public List<Etiqueta> Etiquetas { get; private set; } = [];   // ← sus muchas etiquetas

     public Producto(string nombre, decimal precio, int stock, int categoriaId)   // ← sin id
    {
        if (stock < 0)
            throw new ArgumentException("El stock no puede ser negativo", nameof(stock));
        if (precio < 0)
            throw new ArgumentException("El precio no puede ser negativo", nameof(precio));

        Nombre = nombre;
        Precio = precio;
        Stock = stock;
        CategoriaId = categoriaId;
    }

    public void DescontarStock(int cantidad){
            if (cantidad <= 0)
            {
                throw new ArgumentException("La cantidad debe ser mayor que cero", nameof(cantidad));
            }

            if (cantidad > Stock)
            {
               
                throw new StockInsuficienteException(Nombre,cantidad,Stock);
            }

            Stock -= cantidad;
    }
    public void AgregarEtiqueta(Etiqueta etiqueta)
    {
        if (!Etiquetas.Contains(etiqueta))
            Etiquetas.Add(etiqueta);
    }
  
}






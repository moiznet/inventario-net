namespace Inventario.Api.Models;

public class Producto
{
    public int Id { get; init; }
    public string Nombre { get; } = string.Empty;
    public decimal Precio { get; }
    public int Stock { get; private set; }
 

    public Producto(int id, string nombre, decimal precio, int stock)
    {
        Id = id;
        Nombre = nombre;
        Precio = precio;
        Stock = stock;
    

        if (stock < 0){
            throw new ArgumentException("El stock no puede ser negativo", nameof(stock));
        };

        if (precio < 0){
            throw new ArgumentException("El precio no puede ser negativo", nameof(precio));
        };

   
        
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
  
}






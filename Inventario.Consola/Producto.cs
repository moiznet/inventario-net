

public class Producto
{
    public int Id { get; init; }
    public string Nombre { get; set; } = string.Empty;
    public decimal Precio { get; set; }
    public int stock { get; private set; }
 

    public Producto(int id, string nombre, decimal precio, int stock)
    {
        Id = id;
        Nombre = nombre;
        Precio = precio;
        this.stock = stock;
    

        if (stock < 0){
            throw new ArgumentException("El stock no puede ser negativo", nameof(stock));
        };

        if (precio < 0){
            throw new ArgumentException("El precio no puede ser negativo", nameof(precio));
        };

   
        
    }

    public void DescontarStock(int cantidad){
            if (cantidad < 0)
            {
                throw new ArgumentException("La cantidad a descontar no puede ser negativa", nameof(cantidad));
            }

            if (cantidad > stock)
            {
               
                throw new StockInsuficienteException(Nombre,cantidad,stock);
            }

            stock -= cantidad;
    }
 
    public async Task<ProductoDto> createDto(int Id)
    {
        await Task.Delay(3000);
        return new ProductoDto(Id,"teclado",300000);
    }
}






   
public class Pedido
{
    
    
    
    public int Id { get; }
    public DateTime Fecha  { get;  }
    private readonly List<LineaPedido> _lineas = [];
    public IReadOnlyList<LineaPedido> Lineas => _lineas;
    public decimal Total => _lineas.Sum(l => l.Subtotal);



    public Pedido(int id ){

        Id = id;
        Fecha = DateTime.Now;

    }

    public void AgregarLinea(Producto producto, int cantidad)
    {
        producto.DescontarStock(cantidad);
        _lineas.Add(new LineaPedido(producto.Id, producto.Nombre, cantidad, producto.Precio));
    }
 






}

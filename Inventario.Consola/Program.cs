 
using System.Globalization;
using System.Diagnostics;
using System.Runtime.CompilerServices;
 
  
Dictionary<int, Producto> inventario = new Dictionary<int, Producto>
{
    [0] = new Producto(0, "Mouse", 5000m, 5),
    [1] = new Producto(1, "Teclado", 50000m, 5),
    [2] = new Producto(2, "Monitor", 50000m, 5),
    [3] = new Producto(3, "Cable", 5000m, 5),
    [4] = new Producto(4, "Combo", 5000m, 2),
    [5] = new Producto(5, "Silla", 5000m, 5)
};
 
List<(int ProductoId, int Cantidad)> carrito =
[
    (1, 2),   // funciona
    (6, 8),   // donde peroducto no exista
    (4, 3)    // stock insuficiente

];

async Task<Pedido> CrearPedidoAsync(int idPedido,List<(int ProductoId, int Cantidad)> carrito)
{   

    var pedido = new Pedido(idPedido); 
    
 

    foreach (var item in carrito)
    {
        var producto = await ObtenerProductoAsync(item.ProductoId);

        if (producto is null)
        {
            Console.WriteLine($"Producto {item.ProductoId} no existe.");
            continue;
        }

        try
        {
            pedido.AgregarLinea(producto, item.Cantidad);
        }
        catch (StockInsuficienteException ex)
        {
            
            Console.WriteLine($"No se pudo agregar {ex.Nombre}: pediste {ex.Cantidad}, solo hay {ex.Stock}.");

        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"No se pudo agregar {producto.Nombre}: {ex.Message}");
        }
    }

    return pedido;
}

async Task<Producto?> ObtenerProductoAsync(int id)
{
    await Task.Delay(1000);
    
    if(inventario.TryGetValue(id,out var producto))
    {
        return inventario[id];
    }
    else
    {
        return null;
    }

}

void ImprimirLineas(IEnumerable<Pedido.LineaPedido> lineas)
{
    foreach (var linea in lineas)
    {
        Console.WriteLine($"{linea.Cantidad} x {linea.NombreProducto}    ${linea.ValorUnitario:N0}    ${linea.Subtotal:N0}");
    }
}


void ImprimirInventarioActulizado(List<Producto> productos)
{
    Console.WriteLine("\nInventario actualizado:\n");
    foreach (var producto in productos)
    {
        Console.WriteLine($"Producto: {producto.Nombre} - Stock: {producto.stock}");
    }
}


var objectopedido = await CrearPedidoAsync(1, carrito);
Console.WriteLine($"\nPedido #{objectopedido.Id} - {objectopedido.Fecha} \n");
ImprimirLineas(objectopedido.Lineas);

Console.WriteLine($"\nTotal: ${objectopedido.Total:F0} \n");

ImprimirInventarioActulizado(inventario.Values.ToList());







 




 
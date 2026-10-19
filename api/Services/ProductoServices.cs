
using Inventario.Api.Models;
using Inventario.Api.Options;
using Microsoft.Extensions.Options;
namespace Inventario.Api.Services;

public class ProductoServices(IOptions<InventarioOptions> options, ILogger<ProductoServices> logger): IProductoService
{   

    private readonly InventarioOptions _opciones = options.Value;
    private readonly List<Producto> _productos =
    [
        new Producto(1, "Mouse", 50_000m, 10),
        new Producto(2, "Teclado", 120_000m, 5),
        new Producto(3, "Monitor", 850_000m, 3)
    ];
    public IEnumerable<Producto> ObtenerTodos()
    {
       return _productos;
    }

    public Producto? ObtenerPorId(int id)
    {
        return _productos.FirstOrDefault(p => p.Id == id);
    }

    public Producto Crear(string nombre, decimal precio, int stock)
    {
        var nuevoId = _productos.Count == 0 ? 1 : _productos.Max(p => p.Id) + 1;
        var producto = new Producto(nuevoId, nombre, precio, stock);
        _productos.Add(producto);
        logger.LogInformation("Producto creado: {Id} {Nombre}", producto.Id, producto.Nombre);
        return producto;
    }

    public IEnumerable<Producto> ObtenerPorNombre(string nombre)
    {
       List<Producto> productos = _productos.Where(p => p.Nombre.Contains(nombre, StringComparison.OrdinalIgnoreCase)).ToList(); 
       return productos;
    }

    public bool Eliminar(int id)
    {
        var producto = _productos.FirstOrDefault(p => p.Id == id);
        if (producto == null)
        {
            return false;
        }
        _productos.Remove(producto);
        logger.LogInformation("Producto eliminado: {Id} {Nombre}", producto.Id, producto.Nombre);
        return true;
    }
    public Producto? QuitarDeStock(int id, int cantidad)
    {
        var producto = _productos.FirstOrDefault(p => p.Id == id);
        if (producto == null)
        {
            return null;
        }
        producto.DescontarStock(cantidad);
        if (producto.Stock == 0)
            logger.LogWarning("Producto Agotado en Stock: {Id} {Nombre}", producto.Id, producto.Nombre);
        
        return producto;

     
    }
    public IEnumerable<Producto> ObtenerStockBajo()
    {
        return _productos.Where(p => p.Stock <= _opciones.UmbralStockBajo);

    }



}
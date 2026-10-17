using Inventario.Api.Models;
namespace Inventario.Api.Services;

public interface IProductoService
{
    IEnumerable<Producto> ObtenerTodos();
    Producto? ObtenerPorId(int id);
    Producto Crear(string nombre, decimal precio, int stock);
    IEnumerable<Producto> ObtenerPorNombre(string nombre);
    bool Eliminar(int id);
    Producto? QuitarDeStock(int id, int cantidad);
    IEnumerable<Producto> ObtenerStockBajo();
}
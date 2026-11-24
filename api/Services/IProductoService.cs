// api/Services/IProductoService.cs
using Inventario.Api.Models;
namespace Inventario.Api.Services;

public interface IProductoService
{
    Task<List<Producto>> ObtenerTodosAsync(CancellationToken ct = default);
    Task<Producto?> ObtenerPorIdAsync(int id, CancellationToken ct = default);
    Task<Producto> CrearAsync(string nombre, decimal precio, int stock, int categoriaId, CancellationToken ct = default);
    Task<List<Producto>> ObtenerPorNombreAsync(string nombre, CancellationToken ct = default);
    Task<bool> EliminarAsync(int id, CancellationToken ct = default);
    Task<Producto?> QuitarDeStockAsync(int id, int cantidad, CancellationToken ct = default);
    Task<List<Producto>> ObtenerStockBajoAsync(CancellationToken ct = default);
    Task<List<Producto>> ObtenerPorCategoriaAsync(int categoriaId, CancellationToken ct = default);
}
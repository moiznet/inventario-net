namespace Inventario.Api.Services;
using Inventario.Api.Models;
 
public interface ICategoriaService
{
    Task<List<Categoria>> ObtenerTodasAsync(CancellationToken ct = default);
    Task<Categoria?> ObtenerPorIdAsync(int id, CancellationToken ct = default);
    Task<Categoria> CrearAsync(string nombre, CancellationToken ct = default);
    Task<bool> RenombrarAsync(int id, string nombre, CancellationToken ct = default);
    Task<bool> EliminarAsync(int id, CancellationToken ct = default);
    Task<bool> ExisteNombreAsync(string nombre, CancellationToken ct = default);
}
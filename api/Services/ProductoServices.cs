// api/Services/ProductoServices.cs
using Inventario.Api.Data;
using Inventario.Api.Models;
using Inventario.Api.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Inventario.Api.Services;

public class ProductoServices(
    AppDbContext db,
    IOptions<InventarioOptions> options,
    ILogger<ProductoServices> logger) : IProductoService
{
    private readonly InventarioOptions _opciones = options.Value;

    // ── LECTURAS: AsNoTracking, porque solo leemos ──

    public Task<List<Producto>> ObtenerTodosAsync(CancellationToken ct = default) =>
        db.Productos.AsNoTracking().OrderBy(p => p.Id).ToListAsync(ct);

    public Task<Producto?> ObtenerPorIdAsync(int id, CancellationToken ct = default) =>
        db.Productos.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id, ct);

    public Task<List<Producto>> ObtenerPorNombreAsync(string nombre, CancellationToken ct = default) =>
        db.Productos.AsNoTracking().Where(p => p.Nombre.Contains(nombre)).ToListAsync(ct);

    public Task<List<Producto>> ObtenerStockBajoAsync(CancellationToken ct = default) =>
        db.Productos.AsNoTracking().Where(p => p.Stock <= _opciones.UmbralStockBajo).ToListAsync(ct);

    public Task<List<Producto>> ObtenerPorCategoriaAsync(int categoriaId, CancellationToken ct = default) =>
        db.Productos.AsNoTracking().Where(p => p.CategoriaId == categoriaId).ToListAsync(ct);

    // ── ESCRITURAS: con tracking, y SaveChangesAsync al final ──

    public async Task<Producto> CrearAsync(string nombre, decimal precio, int stock, int categoriaId, CancellationToken ct = default)
    {
        var producto = new Producto(nombre, precio, stock, categoriaId);
        db.Productos.Add(producto);
        await db.SaveChangesAsync(ct);   // aquí SQL Server le asigna el Id

        logger.LogInformation("Producto creado: {Id} {Nombre}", producto.Id, producto.Nombre);
        return producto;
    }

    public async Task<bool> EliminarAsync(int id, CancellationToken ct = default)
    {
        var producto = await db.Productos.FindAsync([id], ct);
        if (producto is null)
            return false;

        db.Productos.Remove(producto);
        await db.SaveChangesAsync(ct);

        logger.LogInformation("Producto eliminado: {Id} {Nombre}", producto.Id, producto.Nombre);
        return true;
    }

    public async Task<Producto?> QuitarDeStockAsync(int id, int cantidad, CancellationToken ct = default)
    {
        var producto = await db.Productos.FindAsync([id], ct);   // rastreado: EF Core guarda la copia
        if (producto is null)
            return null;

        producto.DescontarStock(cantidad);   // tu regla de negocio, sin tocarla
        await db.SaveChangesAsync(ct);       // EF Core detecta que Stock cambió → UPDATE

        if (producto.Stock == 0)
            logger.LogWarning("Producto agotado: {Id} {Nombre}", producto.Id, producto.Nombre);

        return producto;
    }

    public Task<bool> ExistenEnCategoriaAsync(int categoriaId, CancellationToken ct = default) =>
    db.Productos.AnyAsync(p => p.CategoriaId == categoriaId, ct);
}
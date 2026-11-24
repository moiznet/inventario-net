namespace Inventario.Api.Services;
using Inventario.Api.Services;
using Inventario.Api.Models;
using Inventario.Api.Data;
using Microsoft.EntityFrameworkCore;

public class CategoriaServices(AppDbContext db, ILogger<CategoriaServices> logger): ICategoriaService 
{   

   
    public Task<List<Categoria>> ObtenerTodasAsync(CancellationToken ct = default)
    {
        return db.Categorias.AsNoTracking().ToListAsync(ct);
    }
  
    public Task<Categoria?> ObtenerPorIdAsync(int id, CancellationToken ct = default)
    {
        return db.Categorias.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id, ct);
    }

    public async Task<Categoria> CrearAsync(string nombre, CancellationToken ct = default)
    {
        var categoria = new Categoria(nombre);
        db.Categorias.Add(categoria);
        await db.SaveChangesAsync(ct);
        logger.LogInformation("Categoria creada: {Id} {Nombre}", categoria.Id, categoria.Nombre);
        return categoria;
    }

    public async Task<bool> RenombrarAsync(int id, string name, CancellationToken ct = default)
    {
        var categoria = await db.Categorias.FindAsync([id], ct);
        if(categoria == null)
        {
            return false;
        }
        
        categoria.RenombrarCategoria(name);
        await db.SaveChangesAsync(ct);
        logger.LogInformation("Categoria renombrada: {Id} {Nombre}", categoria.Id, categoria.Nombre);
        return true;

    }

    public async Task<bool> EliminarAsync(int id, CancellationToken ct = default)
    {
        var categoria = await db.Categorias.FindAsync([id], ct);
        if (categoria is null)
            return false;

        db.Categorias.Remove(categoria);
        await db.SaveChangesAsync(ct);
        logger.LogInformation("Categoria eliminada: {Id} {Nombre}", categoria.Id, categoria.Nombre);
        return true;
    }

        

    public async Task<bool> ExisteNombreAsync(string nombre, CancellationToken ct = default)
    {
        return await db.Categorias.AnyAsync(c => string.Equals(c.Nombre, nombre), ct);
    }




}
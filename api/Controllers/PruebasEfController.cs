using Inventario.Api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Inventario.Api.Controllers;

[ApiController]
[Route("api/pruebas-ef")]
public class PruebasEfController(AppDbContext db) : ControllerBase
{
    // 1. Productos con stock, del más barato al más caro
    [HttpGet("con-stock")]
    public async Task<IActionResult> ConStock()
    {
        var productos = await db.Productos
            .Where(p => p.Stock > 0)
            .OrderBy(p => p.Precio)
            .ToListAsync();
        return Ok(productos);
    }

    // 2. Buscar por nombre
    [HttpGet("buscar")]
    public async Task<IActionResult> Buscar(string texto)
    {
        var productos = await db.Productos
            .Where(p => p.Nombre.Contains(texto))
            .ToListAsync();
        return Ok(productos);
    }

    // 3. resumen
    [HttpGet("resumen")]
    public async Task<IActionResult> Resumen()
    {
        var resumen = await db.Productos
            .GroupBy(p => 1) // Agrupa todos los registros en un solo bloque (toda la tabla)
            .Select(g => new 
            { 
                Total = g.Count(), // Cuenta cuántos registros tiene ese bloque (toda la tabla)
                Agotados = g.Count(p => p.Stock == 0),// Cuenta cuántos registros tienen stock = 0 
                precioMaximo = g.Max(p => p.Precio)
            })
            .ToListAsync();   
            return Ok(resumen);
    }
}
       

 
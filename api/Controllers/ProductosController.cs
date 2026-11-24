using Microsoft.AspNetCore.Mvc;
using Inventario.Api.Models;
using Inventario.Api.Dtos;
using Inventario.Api.Services;

namespace Inventario.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductosController( IProductoService productoService, ICategoriaService categoriaService ) : ControllerBase
{
    
    [HttpGet]
    [HttpGet]
    public async Task<IEnumerable<ProductoResponse>> GetAll(CancellationToken ct)
    {
        var productos = await productoService.ObtenerTodosAsync(ct);
        return productos.Select(ProductoResponse.Desde).ToList();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductoResponse>> GetById(int id, CancellationToken ct)
    {
        var producto = await productoService.ObtenerPorIdAsync(id, ct);
        if (producto is null)
            return NotFound();
        return ProductoResponse.Desde(producto);
    }


    [HttpPost]
    public async Task<ActionResult<ProductoResponse>> CreateProducto(CrearProductoRequest request, CancellationToken ct)
    {
        if (await categoriaService.ObtenerPorIdAsync(request.Categoria, ct) is null)
            throw new ArgumentException($"La categoría {request.Categoria} no existe");

        var producto = await productoService.CrearAsync(request.Nombre, request.Precio, request.Stock, request.Categoria, ct);
        return CreatedAtAction(nameof(GetById), new { id = producto.Id }, ProductoResponse.Desde(producto));
    }

    [HttpGet("buscar")]
    public async Task<ActionResult<IEnumerable<ProductoResponse>>> GetByName(string? nombre, CancellationToken ct)
    {   
        if (string.IsNullOrWhiteSpace(nombre))
        return BadRequest("Debes enviar el parámetro 'nombre'.");

        var productos = await productoService.ObtenerPorNombreAsync(nombre, ct);
        return productos.Select(p => ProductoResponse.Desde(p)).ToList();
    }

    [HttpDelete("{id}")]
    public  async Task<ActionResult>  DeleteProducto(int id, CancellationToken ct)
    {
        var producto = await productoService.EliminarAsync(id, ct);
        if (producto is false)
            return NotFound();
        return NoContent();
    }

    [HttpPost("{id}/descontar-stock")]
    public async Task<ActionResult<ProductoResponse>> DescontarStock(int id, DescontarStockRequest request, CancellationToken ct)
    {
        var producto = await productoService.QuitarDeStockAsync(id, request.Cantidad, ct);
        if (producto is null)
            return NotFound();
        return Ok(ProductoResponse.Desde(producto));
    }

    [HttpGet("stock-bajo")]
    public async Task<ActionResult<IEnumerable<ProductoResponse>>> GetStockBajo(CancellationToken ct)
    {
        var productos = await productoService.ObtenerStockBajoAsync(ct);
        return Ok(productos.Select(p => ProductoResponse.Desde(p)).ToList());
    }

    

}
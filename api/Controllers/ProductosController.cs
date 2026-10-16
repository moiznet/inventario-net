using Microsoft.AspNetCore.Mvc;
using Inventario.Api.Models;
using Inventario.Api.Dtos;


namespace Inventario.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductosController : ControllerBase
{
    private static readonly List<Producto> _productos =
    [
        new Producto(1, "Mouse", 50_000m, 10),
        new Producto(2, "Teclado", 120_000m, 5),
        new Producto(3, "Monitor", 850_000m, 3)
    ];




    [HttpGet]
    public IEnumerable<ProductoResponse> GetAll()
    {
        return _productos.Select(p => ProductoResponse.Desde(p)).ToList();
    }

    [HttpGet("{id}")]
    public ActionResult<ProductoResponse> GetById(int id)
    {
        var producto = _productos.FirstOrDefault(p => p.Id == id);
        if (producto == null)
        {
            return NotFound();
        }
        return ProductoResponse.Desde(producto);
    }

    [HttpPost]
    public ActionResult<ProductoResponse> CreateProducto(CrearProductoRequest request)
    {
        var nuevoId = _productos.Count == 0 ? 1 : _productos.Max(p => p.Id) + 1;
        var producto = new Producto(nuevoId, request.Nombre, request.Precio, request.Stock);
        _productos.Add(producto); 
        return CreatedAtAction(nameof(GetById), new { id = producto.Id }, ProductoResponse.Desde(producto));    
    }

    [HttpGet("buscar")]
    public ActionResult<IEnumerable<ProductoResponse>> GetByName(string? nombre)
    {   
        if (string.IsNullOrWhiteSpace(nombre))
        return BadRequest("Debes enviar el parámetro 'nombre'.");

        List<Producto> productos = _productos.Where(p => p.Nombre.Contains(nombre, StringComparison.OrdinalIgnoreCase)).ToList();
        return productos.Select(p => ProductoResponse.Desde(p)).ToList();
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteProducto(int id)
    {
        var producto = _productos.FirstOrDefault(p => p.Id == id);
        if (producto == null)
        {
            return NotFound();
        }
        _productos.Remove(producto);
        return NoContent();

    }

    [HttpPost("{id}/descontar-stock")]
    public ActionResult<ProductoResponse> DescontarStock(int id, DescontarStockRequest request)
    {
        var producto = _productos.FirstOrDefault(p => p.Id == id);
        if (producto is null)
        {
            return NotFound();
        }
        try
        {
            producto.DescontarStock(request.Cantidad);
            return Ok(ProductoResponse.Desde(producto));
        }
        catch(StockInsuficienteException ex)
        {
            return Conflict(ex.Message);
        }
        catch(ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
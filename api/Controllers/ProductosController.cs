using Microsoft.AspNetCore.Mvc;
using Inventario.Api.Models;
using Inventario.Api.Dtos;
using Inventario.Api.Services;

namespace Inventario.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductosController( IProductoService productoService ) : ControllerBase
{
    
    [HttpGet]
    public IEnumerable<ProductoResponse> GetAll()
    {
        return productoService.ObtenerTodos().Select(p => ProductoResponse.Desde(p)).ToList();
    }

    [HttpGet("{id}")]
    public ActionResult<ProductoResponse> GetById(int id)
    {   

        var producto = productoService.ObtenerPorId(id);
        if (producto == null)
        {
            return NotFound();
        }
        return ProductoResponse.Desde(producto);
    }

    [HttpPost]
    public ActionResult<ProductoResponse> CreateProducto(CrearProductoRequest request)
    {
        var producto = productoService.Crear(request.Nombre, request.Precio, request.Stock);
        return CreatedAtAction(nameof(GetById), new { id = producto.Id }, ProductoResponse.Desde(producto));    
    }

    [HttpGet("buscar")]
    public ActionResult<IEnumerable<ProductoResponse>> GetByName(string? nombre)
    {   
        if (string.IsNullOrWhiteSpace(nombre))
        return BadRequest("Debes enviar el parámetro 'nombre'.");

        var productos = productoService.ObtenerPorNombre(nombre);
        return productos.Select(p => ProductoResponse.Desde(p)).ToList();
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteProducto(int id)
    {
        var producto = productoService.Eliminar(id);
        if (producto == false)
        {
            return NotFound();
        }
        return NoContent();

    }

    [HttpPost("{id}/descontar-stock")]
    public ActionResult<ProductoResponse> DescontarStock(int id, DescontarStockRequest request)
    {
       
        try
        {
            var producto = productoService.QuitarDeStock(id, request.Cantidad);
            if (producto == null)
            {
                return NotFound();
            }
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

    [HttpGet("stock-bajo")]
    public ActionResult<Producto> GetStockBajo()
    {
        var productos = productoService.ObtenerStockBajo();
        return Ok(productos.Select(p => ProductoResponse.Desde(p) ).ToList());

    }
}
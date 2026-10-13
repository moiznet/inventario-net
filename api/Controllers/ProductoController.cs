using Microsoft.AspNetCore.Mvc;

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
    public IEnumerable<Producto> GetAll()
    {
        return _productos;
    }

    [HttpGet("{id}")]
    public ActionResult<Producto> Get(int id)
    {
        var producto = _productos.FirstOrDefault(p => p.Id == id);
        if (producto == null)
        {
            return NotFound();
        }
        return producto;
    }
}
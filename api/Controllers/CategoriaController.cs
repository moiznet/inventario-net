using Microsoft.AspNetCore.Mvc;
namespace Inventario.Api.Controllers;
using Inventario.Api.Services;
using Inventario.Api.Dtos;
using Inventario.Api.Models;

[ApiController]
[Route("api/[Controller]")]
public class CategoriaController(ICategoriaService categoriaService, IProductoService productoService ): ControllerBase
{
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoriaResponse>>> GetCategories(CancellationToken ct)
    {
        var categorias = await categoriaService.ObtenerTodasAsync(ct);
        return Ok(categorias.Select(c => CategoriaResponse.Desde(c)).ToList());
    }
   
    [HttpGet("{id}")]
    public async Task<ActionResult<CategoriaResponse>> GetCategoriaPorId(int id, CancellationToken ct)
    {
        var categoria = await categoriaService.ObtenerPorIdAsync(id, ct);
        if(categoria == null)
        {
            return NotFound();
        }
        return CategoriaResponse.Desde(categoria);
    }
    
    
    [HttpGet("{id}/productos")]
    public async Task<ActionResult<IEnumerable<ProductoResponse>>> GetProductosDeCategoria(int id, CancellationToken ct)
    {   
        if (await categoriaService.ObtenerPorIdAsync(id, ct) is null)
            return NotFound();

            var productos = await productoService.ObtenerPorCategoriaAsync(id, ct);   // una sola consulta
            return productos.Select(ProductoResponse.Desde).ToList();

    }
    [HttpPost]
    public async Task<ActionResult> Crearcategoria(CrearCategoriaRequest request, CancellationToken ct){

        if (await categoriaService.ExisteNombreAsync(request.Nombre, ct))
            throw new ArgumentException($"El nombre de categoria {request.Nombre} ya existe");

        var nuevaCategoria = await categoriaService.CrearAsync(request.Nombre, ct);
        return CreatedAtAction(nameof(GetCategoriaPorId), new { id = nuevaCategoria.Id }, CategoriaResponse.Desde(nuevaCategoria));    

    }
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCategoria(int id, CrearCategoriaRequest request, CancellationToken ct )
    {
         if (await categoriaService.ExisteNombreAsync(request.Nombre, ct))
        return Conflict($"Ya existe una categoría llamada '{request.Nombre}'");   // 409, sin llegar a la base

        var actualizada = await categoriaService.RenombrarAsync(id, request.Nombre, ct);
        if (!actualizada)
            return NotFound();

        return NoContent();

    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> EliminarCategoria(int id, CancellationToken ct)
    {
 
        if (await productoService.ExistenEnCategoriaAsync(id,ct))
            return Conflict("No se puede eliminar una categoría que tiene productos.");

        if(!await categoriaService.EliminarAsync(id, ct))
            return NotFound();


        return NoContent();

    }










}
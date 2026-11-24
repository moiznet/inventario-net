using Microsoft.AspNetCore.Mvc;
namespace Inventario.Api.controllers;
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
        if (await productoService.ObtenerPorCategoriaAsync(id, ct) is null)  
            return NotFound();

        return (await productoService.ObtenerPorCategoriaAsync(id, ct)).Select(p => ProductoResponse.Desde(p)).ToList();

    }
    [HttpPost]
    public async Task<ActionResult> Crearcategoria(CrearCategoriaRequest request, CancellationToken ct){

        if (await categoriaService.ExisteNombreAsync(request.Nombre, ct))
            throw new ArgumentException($"El nombre de categoria {request.Nombre} ya existe");

        var NuevaCat = await categoriaService.CrearAsync(request.Nombre, ct);
        return CreatedAtAction(nameof(GetCategoriaPorId), new { id = NuevaCat.Id }, CategoriaResponse.Desde(NuevaCat));    

    }
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCategoria(int id, CrearCategoriaRequest request, CancellationToken ct )
    {
        var udated = await categoriaService.RenombrarAsync(id,request.Nombre, ct);
        if(udated == false)
        {
            return NotFound();
        }
        return NoContent(); 

    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> EliminarCategoria(int id, CancellationToken ct)
    {
 
        if (await productoService.ObtenerPorCategoriaAsync(id,ct) != null)
            return Conflict("No se puede eliminar una categoría que tiene productos.");

        if(!await categoriaService.EliminarAsync(id, ct))
            return NotFound();


        return NoContent();

    }










}
using Microsoft.AspNetCore.Mvc;
namespace Inventario.Api.controller;
using Inventario.Api.Services;
using Inventario.Api.Dtos;
using Inventario.Api.Models;

[ApiController]
[Route("api/[Controller]")]
public class CategoriaController(ICategoriaService categoriaService, IProductoService productoService ): ControllerBase
{
    
    [HttpGet]
    public IEnumerable<CategoriaResponse> GetCategories()
    {
        return categoriaService.GetAll().Select(p => CategoriaResponse.Desde(p)).ToList();
    }

    [HttpGet("{id}")]
    public ActionResult<CategoriaResponse> GetCategoriaPorId(int id)
    {
        var categoria = categoriaService.getById(id);
        if(categoria == null)
        {
            return NotFound();
        }
        return CategoriaResponse.Desde(categoria);

    }
    [HttpGet("{id}/productos")]
    public IEnumerable<ProductoResponse> GetProductosDeCategoria(int id)
    {
        return productoService.GetProductByCat(id).Select(p => ProductoResponse.Desde(p)).ToList();

    }
    [HttpPost]
    public ActionResult<CategoriaResponse> Crearcategoria(CrearCategoriaRequest request){

        if (!categoriaService.ExistName(request.Nombre))
            throw new ArgumentException($"El nombre de categoria {request.Nombre} ya existe");

        var NuevaCat = categoriaService.CreateCategory(request.Nombre);
        return CreatedAtAction(nameof(GetCategoriaPorId), new { id = NuevaCat.Id }, CategoriaResponse.Desde(NuevaCat));    

    }
    [HttpPut("{id}")]
    public IActionResult UpdateCategoria(int id, CrearCategoriaRequest request )
    {
        var udated = categoriaService.UpdateCat(id,request.Nombre);
        if(udated == false)
        {
            throw new ArgumentException($"La categoría {id} no existe");
        }
        return NoContent(); 

    }
    [HttpDelete("{id}")]
    public IActionResult EliminarCategoria(int id)
    {
 
        if (productoService.GetProductByCat(id).Any())
        return Conflict("No se puede eliminar una categoría que tiene productos.");

        if(!categoriaService.DeleteCategoria(id))
            return NotFound();

            
        return NoContent();

    }










}
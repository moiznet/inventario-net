namespace Inventario.Api.Dtos;
using Inventario.Api.Models;

public record CategoriaResponse(int Id, string Nombre)
{
    
    public static CategoriaResponse Desde(Categoria cat)
    {
        return new CategoriaResponse(cat.Id,cat.Nombre);

    }


}
namespace Inventario.Api.Dtos;
using Inventario.Api.Models;

public record CategoriaResponse(int id, string nombre)
{
    
    public static CategoriaResponse Desde(Categoria cat)
    {
        return new CategoriaResponse(cat.Id,cat.Nombre);

    }


}
namespace Inventario.Api.Models;

public class Categoria
{
    
    public int Id { set; get;}
    public string Nombre {private set; get;}

    public Categoria(int id, string name)
    {
        if(string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("El nombre de la categoría es obligatorio", nameof(name));

        Id = id;
        Nombre = name;

    }



    public void RenombrarCategoria(string nuevoNombre)
    {
        Nombre = nuevoNombre;

    }



}
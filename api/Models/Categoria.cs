namespace Inventario.Api.Models;

public class Categoria
{
    public int Id { get; private set; }
    public string Nombre { get; private set; } = string.Empty;

    public List<Producto> Productos { get; private set; } = [];   // ← navegación: sus productos

    public Categoria(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new ArgumentException("El nombre de la categoría es obligatorio", nameof(nombre));
        Nombre = nombre;
    }



    public void RenombrarCategoria(string nuevoNombre)
    {
        if(string.IsNullOrWhiteSpace(nuevoNombre))
            throw new ArgumentException("El nombre de la categoría es obligatorio", nameof(nuevoNombre));
        Nombre = nuevoNombre;

    }



}
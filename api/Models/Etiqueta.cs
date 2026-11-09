namespace Inventario.Api.Models;

public class Etiqueta
{
    public int Id { get; private set; }
    public string Nombre { get; private set; } = string.Empty;

    public List<Producto> Productos { get; private set; } = [];   // ← sus muchos productos

    public Etiqueta(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new ArgumentException("El nombre de la etiqueta es obligatorio", nameof(nombre));
        Nombre = nombre;
    }
}
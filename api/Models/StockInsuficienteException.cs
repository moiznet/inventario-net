
namespace Inventario.Api.Models;

public class StockInsuficienteException(string nombre, int cantidad , int stock): Exception($"Stock insuficiente de {nombre}: pediste {cantidad}, hay {stock}")
{
    public string Nombre { get; } = nombre;
    public int Cantidad { get; } = cantidad;
    public int Stock { get; } = stock;

}
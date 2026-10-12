public class StockInsuficienteException(string nombre, int cantidad , int stock): Exception($"Stock insuficiente de {nombre}: pediste {cantidad}, hay {stock}")
{
    public string Nombre = nombre;
    public int Cantidad = cantidad;
    public int Stock = stock;

}
using Inventario.Api.Models;

public record ProductoResponse(int Id, string Nombre, decimal Precio, int Stock, bool TieneStock){
    public static ProductoResponse Desde(Producto p) =>
        new ProductoResponse(p.Id, p.Nombre, p.Precio, p.Stock, p.Stock > 0);
};
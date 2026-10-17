namespace Inventario.Api.Options;


public class InventarioOptions()
{
    public const string Seccion = "Inventario";
    public int UmbralStockbajo  {get; set;} 
    public string NombreTienda {get; set;} = string.Empty;   // ✅ arranca vacío, no null
}
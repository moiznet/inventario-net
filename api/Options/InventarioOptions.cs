namespace Inventario.Api.Options;

using System.ComponentModel.DataAnnotations;

public class InventarioOptions
{
    public const string Seccion = "Inventario";
    
    [Range(1,1000)]
    public int UmbralStockBajo  {get; set;}

    [Required] 
    public string NombreTienda {get; set;} = string.Empty;   // ✅ arranca vacío, no null
}
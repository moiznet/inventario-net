using System.ComponentModel.DataAnnotations;
namespace Inventario.Api.Dtos;
public record CrearProductoRequest(
    
    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "El nombre debe tener entre 1 y 100 caracteres")]
    string Nombre, 
    
    [Range(typeof(decimal), "0", "100000000", ErrorMessage = "El precio debe ser mayor que cero")]
    decimal Precio, 
    
    
    [Range(0, 100_000, ErrorMessage = "El stock debe estar entre 0 y 100.000")]
    int Stock
);

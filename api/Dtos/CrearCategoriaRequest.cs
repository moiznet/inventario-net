using System.ComponentModel.DataAnnotations;
namespace Inventario.Api.Dtos;

public record CrearCategoriaRequest(
    
    [Required]
    [StringLength(50,MinimumLength =2)]
    string Nombre
    
    
        );

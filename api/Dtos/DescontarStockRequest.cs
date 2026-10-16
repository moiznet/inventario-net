using System.ComponentModel.DataAnnotations;

namespace Inventario.Api.Dtos;
public record DescontarStockRequest(
    [Range(1, 1000, ErrorMessage = "La cantidad debe estar entre 1 y 1.000")]
    int Cantidad
    
    );
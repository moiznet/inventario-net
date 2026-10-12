public record LineaPedido( int ProductoId, string NombreProducto, int Cantidad, decimal ValorUnitario)
    {
        public decimal Subtotal => Cantidad * ValorUnitario;
    };
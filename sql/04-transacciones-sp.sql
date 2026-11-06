
-- 1. Ejecuta el ejemplo de la transacción y comprueba que el stock y la cantidad de pedidos quedan igual después del error.
SELECT Stock AS StockActual FROM Productos WHERE Id = 1;
SELECT COUNT(Id) FROM Pedidos;
BEGIN TRY
    BEGIN TRANSACTION;
    INSERT INTO Pedidos (ClienteId, Estado) VALUES (1, N'Pendiente');
    DECLARE @PedidoId INT = SCOPE_IDENTITY();
    INSERT INTO DetallePedido (PedidoId, ProductoId, Cantidad, PrecioUnitario) VALUES (@PedidoId, 1, 2, 100000);
    UPDATE Productos SET Stock = Stock - 20 WHERE Id = 1;
    COMMIT;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK;
    PRINT ERROR_MESSAGE();
END CATCH
SELECT Stock AS StockActual FROM Productos WHERE Id = 1;
SELECT COUNT(Id) FROM Pedidos;

-- 2. Un ROLLBACK a mano: dentro de una transacción, duplica el precio de todos los productos, muestra los precios, haz ROLLBACK y vuelve a mostrarlos. Deben quedar como al principio.

SELECT Nombre,Precio FROM Productos;
BEGIN TRY
    BEGIN TRANSACTION;
    UPDATE Productos SET Precio = Precio * 2;
    SELECT Nombre,Precio FROM Productos; 
    ROLLBACK;
    SELECT Nombre,Precio FROM Productos; 
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK;
    PRINT ERROR_MESSAGE();
END CATCH

-- 3. El stored procedure principal: dbo.sp_CrearPedido, con los parámetros @ClienteId, @ProductoId y @Cantidad. Dentro de una transacción con TRY...CATCH, debe...
GO
CREATE OR ALTER PROCEDURE dbo.sp_CrearPedido
    @ClienteId  INT,
    @ProductoId INT,
    @Cantidad   INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @Precio DECIMAL(18,2), @Stock INT;
        SELECT @Precio = Precio, @Stock = Stock
        FROM Productos
        WHERE Id = @ProductoId;

        IF @Stock IS NULL
            THROW 50001, N'El producto no existe.', 1;

        IF @Stock < @Cantidad
            THROW 50002, N'Stock insuficiente.', 1;

        INSERT INTO Pedidos (ClienteId, Estado) VALUES (@ClienteId, N'Pendiente');
        DECLARE @PedidoId INT = SCOPE_IDENTITY();

        INSERT INTO DetallePedido (PedidoId, ProductoId, Cantidad, PrecioUnitario)
        VALUES (@PedidoId, @ProductoId, @Cantidad, @Precio);

        UPDATE Productos SET Stock = Stock - @Cantidad WHERE Id = @ProductoId;

        COMMIT;
        SELECT @PedidoId AS PedidoId;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK;
        THROW;
    END CATCH
END;
GO

-- Pruebas
EXEC dbo.sp_CrearPedido @ClienteId = 5, @ProductoId = 6,  @Cantidad = 2;   -- válido
EXEC dbo.sp_CrearPedido @ClienteId = 5, @ProductoId = 2,  @Cantidad = 50;  -- stock insuficiente
EXEC dbo.sp_CrearPedido @ClienteId = 5, @ProductoId = 99, @Cantidad = 1;   -- no existe

 --
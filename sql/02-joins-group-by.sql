
-- 1. Productos con el nombre de su categoría, ordenados por categoría y precio
SELECT p.Nombre AS Producto, c.Nombre AS Categoria
FROM Productos p
LEFT JOIN Categorias c ON p.CategoriaId = c.Id
ORDER BY c.Nombre ASC, p.Precio ASC;


--2. Todas las categorías con sus productos, incluyendo las que no tienen ninguno. (9 filas; Software con NULL)

SELECT c.Nombre AS Categoria, p.Nombre AS Productos
FROM Categorias C
LEFT JOIN Productos p ON c.Id = p.CategoriaId;
 
--3. Los clientes que nunca han hecho un pedido. (Sofía Mora)

SELECT c.Nombre AS Cliente
FROM Clientes c
LEFT JOIN Pedidos p ON c.Id = p.ClienteId
WHERE p.Id IS NULL;

-- 4. Cuántos productos tiene cada categoría, incluyendo Software con 0. (Ojo: aquí hay una trampa entre COUNT(*) y COUNT(p.Id). Prueba los dos y mira la diferencia.)

SELECT c.Nombre AS Categoria, COUNT(p.Id) AS CantidadProductos
FROM Categorias c
LEFT JOIN PRoductos p ON c.Id = p.CategoriaId
GROUP BY c.Nombre;

-- 5. El total de cada pedido (Cantidad * PrecioUnitario, sumado), con el nombre del cliente. (6 filas; el pedido 1 suma 220.000)

SELECT pe.Id AS Pedido , c.Nombre AS Cliente, SUM(dp.Cantidad * dp.PrecioUnitario) AS Total
FROM Pedidos pe
INNER JOIN CLientes c ON pe.ClienteId = c.Id
INNER JOIN DetallePedido dp ON dp.PedidoId = pe.Id
GROUP BY pe.Id, c.Nombre;

-- 6. Cuánto ha gastado cada cliente, sin contar los pedidos cancelados, de mayor a menor. (Carlos primero, con 1.170.000)

SELECT c.Nombre AS Cliente, COUNT(DISTINCT pe.Id) AS CantidadPedidos, ISNULL(SUM(dp.Cantidad * dp.PrecioUnitario), 0) AS TotalGastado
FROM Clientes c
LEFT JOIN Pedidos pe ON c.Id = pe.ClienteId AND pe.Estado <> N'Cancelado'
LEFT JOIN DetallePedido dp ON dp.PedidoId = pe.Id
GROUP BY c.Nombre
ORDER BY TotalGastado DESC;

-- 7. Los clientes que han gastado más de 500.000, sin contar los cancelados. (Usa HAVING)

SELECT c.Nombre AS Cliente, COUNT(DISTINCT pe.Id) AS CantidadPedidos, ISNULL(SUM(dp.Cantidad * dp.PrecioUnitario), 0) AS TotalGastado
FROM Clientes c
LEFT JOIN Pedidos pe ON c.Id = pe.ClienteId AND pe.Estado <> N'Cancelado'
LEFT JOIN DetallePedido dp ON dp.PedidoId = pe.Id
GROUP BY c.Nombre
HAVING SUM(dp.Cantidad * dp.PrecioUnitario) > 500000;

-- 8. El producto más vendido en unidades. (Pista: TOP 1 con ORDER BY ... DESC)

SELECT TOP 1 WITH TIES p.Nombre AS Producto, SUM(dp.Cantidad) AS UnidadesVendidas
FROM Productos p
INNER JOIN DetallePedido dp ON dp.ProductoId = p.Id
GROUP BY p.Nombre
ORDER BY UnidadesVendidas DESC;

---------------

SELECT @@TRANCOUNT;   -- si da 1 o más, hay una transacción abierta
ROLLBACK;             -- la cierra y deshace los cambios
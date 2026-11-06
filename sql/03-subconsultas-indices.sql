-- 1. Productos con precio mayor al promedio, usando una subconsulta. (Monitor 24" y Monitor 27")

SELECT p.Nombre AS Producto, p.Precio AS Precio
FROM Productos p
WHERE p.Precio > (SELECT AVG(p2.Precio) FROM Productos p2);

-- 2. Productos que nunca se han vendido, usando NOT EXISTS. (Micrófono)

SELECT p.Nombre AS Producto
FROM Productos p
WHERE NOT EXISTS (SELECT 1 FROM DetallePedido dp WHERE dp.ProductoId = p.Id);

-- 3. Clientes que tienen al menos un pedido Pendiente, usando IN. (Laura y Carlos)

SELECT c.Nombre AS Cliente
FROM Clientes c
WHERE c.Id IN ( SELECT p.ClienteId FROM Pedidos p WHERE p.Estado = N'Pendiente' );

-- 4. Cada producto con las unidades vendidas, usando una subconsulta correlacionada en el SELECT (sin JOIN ni GROUP BY), mostrando 0 para los que no se han vendido. (8 filas; pista: ISNULL)

SELECT p.Nombre AS Producto ,( SELECT ISNULL(SUM(dp.Cantidad),0) FROM DetallePedido dp WHERE dp.ProductoId = p.Id  ) AS UnidadesVendidas
FROM Productos p; 

-- 5. Con una CTE: los pedidos cuyo total supera el promedio de los totales, mostrando también el nombre del cliente. (Pedido 2 de Carlos y pedido 5 de Jorge)

WITH TotalesPedidos AS (
    SELECT pe.Id AS PedidoId, c.Nombre AS Cliente,  SUM(dp.Cantidad * dp.PrecioUnitario) AS TotalPedido
    FROM DetallePedido dp 
    JOIN Pedidos pe ON dp.PedidoId = pe.Id
    JOIN Clientes c ON pe.ClienteId = c.Id
    GROUP BY pe.Id, c.Nombre) 
SELECT PedidoId, Cliente, TotalPedido
FROM TotalesPedidos WHERE TotalPedido > (SELECT AVG(TotalPedido) FROM TotalesPedidos);

--6. Crear índices para mejorar el rendimiento de las consultas que involucran las tablas Productos, Pedidos y DetallePedido.

CREATE INDEX IX_Productos_CategoriaId ON Productos(CategoriaId);
CREATE INDEX IX_Pedidos_ClienteId ON Pedidos(ClienteId);
CREATE INDEX IX_DetallePedido_PedidoId ON DetallePedido(PedidoId);
CREATE INDEX IX_DetallePedido_ProductoId ON DetallePedido(ProductoId);

--7 Cmprobar Indices creados

   SELECT t.name AS Tabla, i.name AS Indice, i.type_desc AS Tipo
   FROM sys.indexes i
   INNER JOIN sys.tables t ON t.object_id = i.object_id
   WHERE i.name IS NOT NULL
   ORDER BY t.name;



 
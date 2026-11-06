-- =============================================================
-- Inventario .NET · Esquema y datos de práctica para el repaso de SQL
-- Se puede ejecutar varias veces: borra y recrea las tablas.
-- =============================================================

USE InventarioDb;
GO

-- Borrar en orden inverso a las dependencias (primero las tablas "hijas")
DROP TABLE IF EXISTS DetallePedido;
DROP TABLE IF EXISTS Pedidos;
DROP TABLE IF EXISTS Clientes;
DROP TABLE IF EXISTS Productos;
DROP TABLE IF EXISTS Categorias;
GO

-- -------------------------------------------------------------
-- Tablas
-- -------------------------------------------------------------

CREATE TABLE Categorias (
    Id      INT IDENTITY(1,1) PRIMARY KEY,
    Nombre  NVARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE Productos (
    Id           INT IDENTITY(1,1) PRIMARY KEY,
    Nombre       NVARCHAR(100)  NOT NULL,
    Precio       DECIMAL(18,2)  NOT NULL CHECK (Precio > 0),
    Stock        INT            NOT NULL CHECK (Stock >= 0),
    CategoriaId  INT            NOT NULL REFERENCES Categorias(Id)
);

CREATE TABLE Clientes (
    Id      INT IDENTITY(1,1) PRIMARY KEY,
    Nombre  NVARCHAR(100) NOT NULL,
    Email   NVARCHAR(100) NOT NULL UNIQUE,
    Ciudad  NVARCHAR(50)  NOT NULL
);

CREATE TABLE Pedidos (
    Id         INT IDENTITY(1,1) PRIMARY KEY,
    ClienteId  INT          NOT NULL REFERENCES Clientes(Id),
    Fecha      DATETIME2    NOT NULL DEFAULT SYSDATETIME(),
    Estado     NVARCHAR(20) NOT NULL DEFAULT N'Pendiente'
);

CREATE TABLE DetallePedido (
    Id              INT IDENTITY(1,1) PRIMARY KEY,
    PedidoId        INT           NOT NULL REFERENCES Pedidos(Id),
    ProductoId      INT           NOT NULL REFERENCES Productos(Id),
    Cantidad        INT           NOT NULL CHECK (Cantidad > 0),
    PrecioUnitario  DECIMAL(18,2) NOT NULL   -- precio al momento de la compra
);
GO

-- -------------------------------------------------------------
-- Datos
-- -------------------------------------------------------------

INSERT INTO Categorias (Nombre) VALUES
    (N'Periféricos'),   -- 1
    (N'Pantallas'),     -- 2
    (N'Audio'),         -- 3
    (N'Software');      -- 4  (sin productos)

INSERT INTO Productos (Nombre, Precio, Stock, CategoriaId) VALUES
    (N'Mouse',        50000,   10, 1),   -- 1
    (N'Teclado',      120000,   5, 1),   -- 2
    (N'Monitor 24"',  850000,   3, 2),   -- 3
    (N'Monitor 27"',  1200000,  0, 2),   -- 4  (agotado)
    (N'Audífonos',    80000,    7, 3),   -- 5
    (N'Parlantes',    150000,   2, 3),   -- 6
    (N'Webcam',       180000,   4, 1),   -- 7
    (N'Micrófono',    220000,   0, 3);   -- 8  (agotado y nunca vendido)

INSERT INTO Clientes (Nombre, Email, Ciudad) VALUES
    (N'Ana Gómez',    N'ana@correo.com',    N'Bogotá'),     -- 1
    (N'Carlos Ruiz',  N'carlos@correo.com', N'Medellín'),   -- 2
    (N'Laura Pérez',  N'laura@correo.com',  N'Bogotá'),     -- 3
    (N'Jorge Díaz',   N'jorge@correo.com',  N'Cali'),       -- 4
    (N'Sofía Mora',   N'sofia@correo.com',  N'Bogotá');     -- 5  (sin pedidos)

INSERT INTO Pedidos (ClienteId, Fecha, Estado) VALUES
    (1, '2026-10-01', N'Entregado'),   -- 1
    (2, '2026-10-03', N'Entregado'),   -- 2
    (1, '2026-10-10', N'Enviado'),     -- 3
    (3, '2026-10-15', N'Pendiente'),   -- 4
    (4, '2026-10-20', N'Cancelado'),   -- 5
    (2, '2026-10-25', N'Pendiente');   -- 6

INSERT INTO DetallePedido (PedidoId, ProductoId, Cantidad, PrecioUnitario) VALUES
    (1, 1, 2,   50000),    -- Pedido 1: 2 Mouse
    (1, 2, 1,  120000),    --           1 Teclado
    (2, 3, 1,  850000),    -- Pedido 2: 1 Monitor 24"
    (3, 5, 2,   80000),    -- Pedido 3: 2 Audífonos
    (3, 1, 1,   45000),    --           1 Mouse (estaba en promoción)
    (4, 7, 1,  180000),    -- Pedido 4: 1 Webcam
    (4, 6, 1,  150000),    --           1 Parlantes
    (5, 4, 1, 1200000),    -- Pedido 5: 1 Monitor 27" (cancelado)
    (6, 2, 2,  120000),    -- Pedido 6: 2 Teclados
    (6, 5, 1,   80000);    --           1 Audífonos
GO

-- Comprobación rápida: debe mostrar 4 · 8 · 5 · 6 · 10
SELECT
    (SELECT COUNT(*) FROM Categorias)    AS Categorias,
    (SELECT COUNT(*) FROM Productos)     AS Productos,
    (SELECT COUNT(*) FROM Clientes)      AS Clientes,
    (SELECT COUNT(*) FROM Pedidos)       AS Pedidos,
    (SELECT COUNT(*) FROM DetallePedido) AS Detalles;

--

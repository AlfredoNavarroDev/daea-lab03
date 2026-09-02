-- =============================================
-- ReservasDB - Script de creación y datos de prueba
-- =============================================

IF DB_ID('ReservasDB') IS NULL
BEGIN
    CREATE DATABASE ReservasDB;
END
GO

USE ReservasDB;
GO

-- =============================================
-- Tabla Usuarios
-- =============================================
IF OBJECT_ID('dbo.Reservas', 'U') IS NOT NULL DROP TABLE dbo.Reservas;
IF OBJECT_ID('dbo.Aulas', 'U') IS NOT NULL DROP TABLE dbo.Aulas;
IF OBJECT_ID('dbo.Usuarios', 'U') IS NOT NULL DROP TABLE dbo.Usuarios;
GO

CREATE TABLE dbo.Usuarios (
    UsuarioId       INT IDENTITY(1,1) PRIMARY KEY,
    Username        NVARCHAR(50)  NOT NULL,
    Password        NVARCHAR(100) NOT NULL,
    NombreCompleto  NVARCHAR(150) NOT NULL
);
GO

-- =============================================
-- Tabla Aulas
-- =============================================
CREATE TABLE dbo.Aulas (
    AulaId      INT IDENTITY(1,1) PRIMARY KEY,
    Nombre      NVARCHAR(50) NOT NULL,
    Capacidad   INT NOT NULL
);
GO

-- =============================================
-- Tabla Reservas
-- =============================================
CREATE TABLE dbo.Reservas (
    ReservaId   INT IDENTITY(1,1) PRIMARY KEY,
    AulaId      INT NOT NULL,
    UsuarioId   INT NOT NULL,
    Fecha       DATE NOT NULL,
    Hora        TIME NOT NULL,
    Motivo      NVARCHAR(200) NOT NULL,
    CONSTRAINT FK_Reservas_Aulas FOREIGN KEY (AulaId) REFERENCES dbo.Aulas(AulaId),
    CONSTRAINT FK_Reservas_Usuarios FOREIGN KEY (UsuarioId) REFERENCES dbo.Usuarios(UsuarioId)
);
GO

-- =============================================
-- Insertar 10 Usuarios de prueba
-- =============================================
INSERT INTO dbo.Usuarios (Username, Password, NombreCompleto) VALUES
('jperez',    'jperez123',    'Juan Pérez'),
('mgomez',    'mgomez123',    'María Gómez'),
('lrodriguez','lrodriguez123','Luis Rodríguez'),
('acastro',   'acastro123',   'Ana Castro'),
('dfernandez','dfernandez123','Diego Fernández'),
('cvargas',   'cvargas123',   'Carla Vargas'),
('rtorres',   'rtorres123',   'Ricardo Torres'),
('pmendoza',  'pmendoza123',  'Paola Mendoza'),
('jsilva',    'jsilva123',    'Jorge Silva'),
('nchavez',   'nchavez123',   'Natalia Chávez');
GO

-- =============================================
-- Insertar 15 Aulas de prueba
-- =============================================
INSERT INTO dbo.Aulas (Nombre, Capacidad) VALUES
('Aula 101', 30),
('Aula 102', 25),
('Aula 103', 40),
('Aula 104', 20),
('Aula 105', 35),
('Aula 201', 50),
('Aula 202', 45),
('Aula 203', 30),
('Aula 204', 28),
('Aula 205', 32),
('Laboratorio A', 22),
('Laboratorio B', 22),
('Auditorio 1', 100),
('Auditorio 2', 80),
('Sala de Conferencias', 15);
GO

-- =============================================
-- Insertar 30 Reservas de prueba
-- =============================================
INSERT INTO dbo.Reservas (AulaId, UsuarioId, Fecha, Hora, Motivo) VALUES
(1,  1, '2026-09-03', '08:00', 'Clase de Matemáticas'),
(2,  2, '2026-09-03', '09:00', 'Clase de Física'),
(3,  3, '2026-09-03', '10:00', 'Reunión de coordinación'),
(4,  4, '2026-09-03', '11:00', 'Tutoría de Programación'),
(5,  5, '2026-09-03', '14:00', 'Clase de Base de Datos'),
(6,  6, '2026-09-04', '08:00', 'Examen parcial'),
(7,  7, '2026-09-04', '09:00', 'Clase de Redes'),
(8,  8, '2026-09-04', '10:00', 'Taller de Robótica'),
(9,  9, '2026-09-04', '11:00', 'Clase de Inglés'),
(10, 10, '2026-09-04', '15:00', 'Sesión de tutoría grupal'),
(11, 1, '2026-09-05', '08:00', 'Práctica de laboratorio'),
(12, 2, '2026-09-05', '09:00', 'Práctica de Química'),
(13, 3, '2026-09-05', '10:00', 'Conferencia de egresados'),
(14, 4, '2026-09-05', '16:00', 'Charla de emprendimiento'),
(15, 5, '2026-09-05', '17:00', 'Reunión de comité académico'),
(1,  6, '2026-09-06', '08:00', 'Clase de Algoritmos'),
(2,  7, '2026-09-06', '09:00', 'Clase de Estadística'),
(3,  8, '2026-09-06', '10:00', 'Defensa de proyecto'),
(4,  9, '2026-09-06', '11:00', 'Clase de Sistemas Operativos'),
(5,  10, '2026-09-06', '13:00', 'Clase de Arquitectura de Software'),
(6,  1, '2026-09-07', '08:00', 'Simulacro de examen'),
(7,  2, '2026-09-07', '09:00', 'Clase de Inteligencia Artificial'),
(8,  3, '2026-09-07', '10:00', 'Reunión de proyecto de tesis'),
(9,  4, '2026-09-07', '11:00', 'Clase de Cálculo Avanzado'),
(10, 5, '2026-09-07', '15:00', 'Taller de Innovación'),
(11, 6, '2026-09-08', '08:00', 'Práctica de Redes'),
(12, 7, '2026-09-08', '09:00', 'Práctica de Programación Web'),
(13, 8, '2026-09-08', '10:00', 'Charla motivacional'),
(14, 9, '2026-09-08', '16:00', 'Feria de proyectos'),
(15, 10, '2026-09-08', '17:00', 'Reunión general de docentes');
GO

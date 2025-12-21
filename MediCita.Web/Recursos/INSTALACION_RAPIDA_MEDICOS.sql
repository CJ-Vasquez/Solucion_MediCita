-- =============================================
-- SCRIPT RÁPIDO DE INSTALACIÓN COMPLETA
-- Copiar y pegar todo en SQL Server Management Studio
-- =============================================

USE BD_MediCita
GO

-- =============================================
-- PASO 1: CREAR/VERIFICAR TABLA tb_Medicos
-- =============================================

-- Verificar si la tabla existe
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tb_Medicos]') AND type in (N'U'))
BEGIN
    PRINT '?? Creando tabla tb_Medicos...'
    
    CREATE TABLE tb_Medicos (
        IdMedico INT PRIMARY KEY IDENTITY(1,1),
        NombreCompleto VARCHAR(100) NOT NULL,
        IdEspecialidad INT NOT NULL,
        CMP VARCHAR(20) NOT NULL,
        Correo VARCHAR(100) NULL,
        Telefono VARCHAR(20) NULL,
        CONSTRAINT FK_Medicos_Especialidades FOREIGN KEY (IdEspecialidad)
            REFERENCES tb_Especialidades(IdEspecialidad)
    )
    
    PRINT '? Tabla tb_Medicos creada exitosamente'
END
ELSE
BEGIN
    PRINT '?? La tabla tb_Medicos ya existe. Verificando columnas...'
    
    -- Verificar y agregar columnas si no existen
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('tb_Medicos') AND name = 'Correo')
    BEGIN
        ALTER TABLE tb_Medicos ADD Correo VARCHAR(100) NULL
        PRINT '? Columna Correo agregada'
    END
    
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('tb_Medicos') AND name = 'Telefono')
    BEGIN
        ALTER TABLE tb_Medicos ADD Telefono VARCHAR(20) NULL
        PRINT '? Columna Telefono agregada'
    END
END
GO

-- =============================================
-- PASO 2: INSERTAR DATOS DE PRUEBA (SI LA TABLA ESTÁ VACÍA)
-- =============================================

IF NOT EXISTS (SELECT * FROM tb_Medicos)
BEGIN
    PRINT '?? Insertando datos de prueba...'
    
    -- Verificar que existan especialidades
    IF EXISTS (SELECT * FROM tb_Especialidades)
    BEGIN
        -- Obtener los IDs de las especialidades disponibles
        DECLARE @IdEsp1 INT, @IdEsp2 INT, @IdEsp3 INT, @IdEsp4 INT
        
        SELECT TOP 1 @IdEsp1 = IdEspecialidad FROM tb_Especialidades ORDER BY IdEspecialidad
        SELECT @IdEsp2 = ISNULL((SELECT TOP 1 IdEspecialidad FROM tb_Especialidades WHERE IdEspecialidad > @IdEsp1 ORDER BY IdEspecialidad), @IdEsp1)
        SELECT @IdEsp3 = ISNULL((SELECT TOP 1 IdEspecialidad FROM tb_Especialidades WHERE IdEspecialidad > @IdEsp2 ORDER BY IdEspecialidad), @IdEsp1)
        SELECT @IdEsp4 = ISNULL((SELECT TOP 1 IdEspecialidad FROM tb_Especialidades WHERE IdEspecialidad > @IdEsp3 ORDER BY IdEspecialidad), @IdEsp1)
        
        INSERT INTO tb_Medicos (NombreCompleto, IdEspecialidad, CMP, Correo, Telefono) VALUES
        ('Carlos Rodríguez Pérez', @IdEsp1, '45678', 'crodriguez@medicita.com', '987654321'),
        ('María González López', @IdEsp2, '45679', 'mgonzalez@medicita.com', '987654322'),
        ('José Martínez Ruiz', @IdEsp3, '45680', 'jmartinez@medicita.com', '987654323'),
        ('Ana Fernández Torres', @IdEsp4, '45681', 'afernandez@medicita.com', '987654324'),
        ('Luis Sánchez Morales', @IdEsp1, '45682', 'lsanchez@medicita.com', '987654325')
        
        PRINT '? Datos de prueba insertados: 5 médicos'
    END
    ELSE
    BEGIN
        PRINT '?? No se pueden insertar médicos porque no hay especialidades en la base de datos'
    END
END
ELSE
BEGIN
    PRINT '?? Ya existen médicos en la tabla. No se insertan datos de prueba.'
END
GO

-- =============================================
-- PASO 3: CREAR STORED PROCEDURES
-- =============================================

PRINT '?? Creando Stored Procedures...'
GO

-- 1. LISTAR TODOS LOS MÉDICOS
CREATE OR ALTER PROCEDURE usp_ListarMedicos
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        m.IdMedico,
        m.NombreCompleto,
        m.IdEspecialidad,
        e.NombreEspec AS Especialidad,
        m.CMP,
        m.Correo,
        m.Telefono
    FROM tb_Medicos m
    INNER JOIN tb_Especialidades e ON m.IdEspecialidad = e.IdEspecialidad
    ORDER BY m.NombreCompleto
END
GO
PRINT '? SP usp_ListarMedicos creado'
GO

-- 2. OBTENER UN MÉDICO POR ID
CREATE OR ALTER PROCEDURE usp_ObtenerMedico
    @IdMedico INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        IdMedico,
        NombreCompleto,
        IdEspecialidad,
        CMP,
        Correo,
        Telefono
    FROM tb_Medicos
    WHERE IdMedico = @IdMedico
END
GO
PRINT '? SP usp_ObtenerMedico creado'
GO

-- 3. REGISTRAR UN NUEVO MÉDICO
CREATE OR ALTER PROCEDURE usp_RegistrarMedico
    @NombreCompleto VARCHAR(100),
    @IdEspecialidad INT,
    @CMP VARCHAR(20),
    @Correo VARCHAR(100),
    @Telefono VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    
    INSERT INTO tb_Medicos (NombreCompleto, IdEspecialidad, CMP, Correo, Telefono)
    VALUES (@NombreCompleto, @IdEspecialidad, @CMP, @Correo, @Telefono)
    
    SELECT SCOPE_IDENTITY() AS IdMedico
END
GO
PRINT '? SP usp_RegistrarMedico creado'
GO

-- 4. ACTUALIZAR UN MÉDICO EXISTENTE
CREATE OR ALTER PROCEDURE usp_EditarMedico
    @IdMedico INT,
    @NombreCompleto VARCHAR(100),
    @IdEspecialidad INT,
    @CMP VARCHAR(20),
    @Correo VARCHAR(100),
    @Telefono VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE tb_Medicos
    SET 
        NombreCompleto = @NombreCompleto,
        IdEspecialidad = @IdEspecialidad,
        CMP = @CMP,
        Correo = @Correo,
        Telefono = @Telefono
    WHERE IdMedico = @IdMedico
END
GO
PRINT '? SP usp_EditarMedico creado'
GO

-- 5. ELIMINAR UN MÉDICO
CREATE OR ALTER PROCEDURE usp_EliminarMedico
    @IdMedico INT
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Verificar si tiene citas asociadas (comentar si quieres eliminar sin validación)
    IF EXISTS (SELECT 1 FROM tb_Citas WHERE IdMedico = @IdMedico)
    BEGIN
        RAISERROR('No se puede eliminar el médico porque tiene citas registradas', 16, 1)
        RETURN
    END
    
    DELETE FROM tb_Medicos
    WHERE IdMedico = @IdMedico
END
GO
PRINT '? SP usp_EliminarMedico creado'
GO

-- =============================================
-- PASO 4: VERIFICACIÓN FINAL
-- =============================================

PRINT ''
PRINT '======================================'
PRINT '? INSTALACIÓN COMPLETADA'
PRINT '======================================'
PRINT ''

-- Mostrar cantidad de médicos
DECLARE @CantidadMedicos INT
SELECT @CantidadMedicos = COUNT(*) FROM tb_Medicos
PRINT '?? Médicos registrados: ' + CAST(@CantidadMedicos AS VARCHAR(10))

-- Mostrar stored procedures creados
PRINT ''
PRINT '?? Stored Procedures creados:'
SELECT '  ? ' + name AS [Stored Procedure]
FROM sys.procedures 
WHERE name LIKE 'usp_%Medico%'
ORDER BY name

-- Mostrar primeros 5 médicos
PRINT ''
PRINT '????? Primeros médicos en la base de datos:'
SELECT TOP 5
    IdMedico,
    NombreCompleto,
    CMP,
    Correo
FROM tb_Medicos
ORDER BY IdMedico

PRINT ''
PRINT '?? ¡Todo listo! Ahora puedes ejecutar la aplicación y acceder al módulo de médicos.'
PRINT ''
GO

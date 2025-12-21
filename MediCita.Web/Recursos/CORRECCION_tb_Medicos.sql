-- =============================================
-- SCRIPT DE CORRECCIÓN SIMPLIFICADO PARA tb_Medicos
-- Versión corregida sin errores
-- =============================================

USE BD_MediCita
GO

PRINT '?? INICIANDO CORRECCIÓN DE tb_Medicos...'
PRINT ''

-- =============================================
-- PASO 1: Agregar columna NombreCompleto
-- =============================================

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('tb_Medicos') AND name = 'NombreCompleto')
BEGIN
    PRINT '? Agregando columna NombreCompleto...'
    ALTER TABLE tb_Medicos ADD NombreCompleto VARCHAR(100) NULL
    PRINT '? Columna NombreCompleto agregada'
END
ELSE
BEGIN
    PRINT '? La columna NombreCompleto ya existe'
END
GO

-- =============================================
-- PASO 2: Migrar datos a NombreCompleto
-- =============================================

-- Verificar si existe columna 'Nombre'
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('tb_Medicos') AND name = 'Nombre')
BEGIN
    -- Verificar si también existe 'Apellido'
    IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('tb_Medicos') AND name = 'Apellido')
    BEGIN
        PRINT '?? Migrando datos desde Nombre + Apellido...'
        UPDATE tb_Medicos
        SET NombreCompleto = LTRIM(RTRIM(ISNULL(Nombre, '') + ' ' + ISNULL(Apellido, '')))
        WHERE NombreCompleto IS NULL
    END
    ELSE
    BEGIN
        PRINT '?? Migrando datos desde Nombre...'
        UPDATE tb_Medicos
        SET NombreCompleto = Nombre
        WHERE NombreCompleto IS NULL
    END
END
ELSE
BEGIN
    PRINT '?? No se encontró columna Nombre. Generando nombres por defecto...'
    UPDATE tb_Medicos
    SET NombreCompleto = 'Médico ' + CAST(IdMedico AS VARCHAR(10))
    WHERE NombreCompleto IS NULL OR NombreCompleto = ''
END
GO

-- Hacer que NombreCompleto sea NOT NULL
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('tb_Medicos') AND name = 'NombreCompleto' AND is_nullable = 1)
BEGIN
    -- Primero llenar cualquier NULL restante
    UPDATE tb_Medicos
    SET NombreCompleto = 'Médico ' + CAST(IdMedico AS VARCHAR(10))
    WHERE NombreCompleto IS NULL OR NombreCompleto = ''
    
    -- Ahora cambiar a NOT NULL
    ALTER TABLE tb_Medicos ALTER COLUMN NombreCompleto VARCHAR(100) NOT NULL
    PRINT '? Columna NombreCompleto configurada como NOT NULL'
END
GO

-- =============================================
-- PASO 3: Agregar columna IdEspecialidad
-- =============================================

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('tb_Medicos') AND name = 'IdEspecialidad')
BEGIN
    PRINT '? Agregando columna IdEspecialidad...'
    
    -- Primero agregar la columna como NULL
    ALTER TABLE tb_Medicos ADD IdEspecialidad INT NULL
    
    -- Obtener el primer IdEspecialidad disponible y actualizar
    DECLARE @PrimeraEspecialidad INT
    SELECT TOP 1 @PrimeraEspecialidad = IdEspecialidad FROM tb_Especialidades ORDER BY IdEspecialidad
    
    IF @PrimeraEspecialidad IS NOT NULL
    BEGIN
        -- Actualizar todos los registros con la primera especialidad
        UPDATE tb_Medicos SET IdEspecialidad = @PrimeraEspecialidad WHERE IdEspecialidad IS NULL
        
        -- Cambiar a NOT NULL
        ALTER TABLE tb_Medicos ALTER COLUMN IdEspecialidad INT NOT NULL
        
        -- Agregar foreign key
        IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Medicos_Especialidades')
        BEGIN
            ALTER TABLE tb_Medicos ADD CONSTRAINT FK_Medicos_Especialidades 
                FOREIGN KEY (IdEspecialidad) REFERENCES tb_Especialidades(IdEspecialidad)
        END
        
        PRINT '? Columna IdEspecialidad creada con FK'
    END
    ELSE
    BEGIN
        -- Si no hay especialidades, usar valor 1 por defecto
        UPDATE tb_Medicos SET IdEspecialidad = 1 WHERE IdEspecialidad IS NULL
        ALTER TABLE tb_Medicos ALTER COLUMN IdEspecialidad INT NOT NULL
        PRINT '??  Columna IdEspecialidad creada pero sin FK (no hay especialidades)'
    END
END
ELSE
BEGIN
    PRINT '? La columna IdEspecialidad ya existe'
END
GO

-- =============================================
-- PASO 4: Agregar columna CMP
-- =============================================

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('tb_Medicos') AND name = 'CMP')
BEGIN
    PRINT '? Agregando columna CMP...'
    ALTER TABLE tb_Medicos ADD CMP VARCHAR(20) NULL
    PRINT '? Columna CMP creada'
END
ELSE
BEGIN
    PRINT '? La columna CMP ya existe'
END
GO

-- =============================================
-- PASO 5: Agregar columna Correo
-- =============================================

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('tb_Medicos') AND name = 'Correo')
BEGIN
    PRINT '? Agregando columna Correo...'
    ALTER TABLE tb_Medicos ADD Correo VARCHAR(100) NULL
    PRINT '? Columna Correo creada'
END
ELSE
BEGIN
    PRINT '? La columna Correo ya existe'
END
GO

-- =============================================
-- PASO 6: Agregar columna Telefono
-- =============================================

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('tb_Medicos') AND name = 'Telefono')
BEGIN
    PRINT '? Agregando columna Telefono...'
    ALTER TABLE tb_Medicos ADD Telefono VARCHAR(20) NULL
    PRINT '? Columna Telefono creada'
END
ELSE
BEGIN
    PRINT '? La columna Telefono ya existe'
END
GO

-- =============================================
-- PASO 7: Actualizar datos NULL o vacíos
-- =============================================

PRINT ''
PRINT '?? Actualizando datos NULL...'

-- Actualizar CMP NULL
UPDATE tb_Medicos
SET CMP = 'CMP' + RIGHT('00000' + CAST(IdMedico AS VARCHAR(10)), 5)
WHERE CMP IS NULL OR CMP = ''

-- Actualizar Correo NULL con formato genérico
UPDATE tb_Medicos
SET Correo = LOWER(REPLACE(NombreCompleto, ' ', '.')) + '@medicita.com'
WHERE Correo IS NULL OR Correo = ''

-- Actualizar Telefono NULL
UPDATE tb_Medicos
SET Telefono = '999' + RIGHT('000000' + CAST(IdMedico AS VARCHAR(10)), 6)
WHERE Telefono IS NULL OR Telefono = ''

PRINT '? Datos actualizados'
GO

-- =============================================
-- PASO 8: Verificación final
-- =============================================

PRINT ''
PRINT '======================================'
PRINT '? CORRECCIÓN COMPLETADA'
PRINT '======================================'
PRINT ''

-- Mostrar estructura actual
PRINT '?? ESTRUCTURA ACTUAL DE tb_Medicos:'
SELECT 
    COLUMN_NAME AS [Columna],
    DATA_TYPE AS [Tipo],
    CHARACTER_MAXIMUM_LENGTH AS [Longitud],
    IS_NULLABLE AS [NULL]
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'tb_Medicos'
ORDER BY ORDINAL_POSITION

PRINT ''

-- Contar registros
DECLARE @Total INT
SELECT @Total = COUNT(*) FROM tb_Medicos
PRINT '?? REGISTROS EN LA TABLA: ' + CAST(@Total AS VARCHAR(10)) + ' médico(s)'

PRINT ''
PRINT '????? MUESTRA DE DATOS (primeros 5):'
SELECT TOP 5
    IdMedico,
    NombreCompleto,
    IdEspecialidad,
    CMP,
    Correo,
    Telefono
FROM tb_Medicos
ORDER BY IdMedico

PRINT ''
PRINT '======================================'
PRINT '?? SIGUIENTE PASO:'
PRINT '   Ejecuta: CREAR_SP_MEDICOS.sql'
PRINT '======================================'
PRINT ''
GO

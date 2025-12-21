-- =============================================
-- SCRIPT DE DIAGNÓSTICO Y CORRECCIÓN
-- Ejecuta este script para ver la estructura actual
-- y luego corregirla
-- =============================================

USE BD_MediCita
GO

PRINT '?? DIAGNÓSTICO DE LA TABLA tb_Medicos'
PRINT '====================================='
PRINT ''

-- Ver todas las columnas actuales de la tabla
PRINT '?? COLUMNAS ACTUALES EN tb_Medicos:'
SELECT 
    COLUMN_NAME AS [Nombre Columna],
    DATA_TYPE AS [Tipo de Dato],
    CHARACTER_MAXIMUM_LENGTH AS [Longitud],
    IS_NULLABLE AS [Permite NULL]
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'tb_Medicos'
ORDER BY ORDINAL_POSITION

PRINT ''
PRINT '====================================='
PRINT ''

-- Ver datos actuales (primeros 3 registros)
PRINT '?? PRIMEROS 3 REGISTROS ACTUALES:'
SELECT TOP 3 * FROM tb_Medicos

PRINT ''
PRINT '====================================='
PRINT ''
PRINT '?? INSTRUCCIONES:'
PRINT ''
PRINT 'Revisa las columnas que muestra arriba.'
PRINT 'Si NO existe la columna "NombreCompleto", ejecuta el script de corrección que está abajo.'
PRINT ''
PRINT '====================================='
GO

/*
-- =============================================
-- SCRIPT DE CORRECCIÓN
-- Descomenta (quita los -- ) y ejecuta SOLO si necesitas corregir
-- =============================================

-- OPCIÓN 1: Si la tabla tiene Nombre y Apellido separados
-- Descomentar estas líneas:

-- Agregar columna NombreCompleto
ALTER TABLE tb_Medicos ADD NombreCompleto VARCHAR(100) NULL
GO

-- Migrar datos existentes (combinar Nombre y Apellido si existen)
-- AJUSTA LOS NOMBRES DE COLUMNAS SEGÚN TU TABLA:
UPDATE tb_Medicos
SET NombreCompleto = CONCAT(Nombre, ' ', Apellido)  -- AJUSTA ESTO
WHERE NombreCompleto IS NULL
GO

-- Hacer que NombreCompleto sea NOT NULL
ALTER TABLE tb_Medicos ALTER COLUMN NombreCompleto VARCHAR(100) NOT NULL
GO

-- Opcional: Eliminar columnas viejas si ya no las necesitas
-- ALTER TABLE tb_Medicos DROP COLUMN Nombre
-- ALTER TABLE tb_Medicos DROP COLUMN Apellido
GO

*/

/*
-- OPCIÓN 2: Si la tabla tiene solo "Nombre" (sin apellido)
-- Descomentar estas líneas:

-- Renombrar columna existente
EXEC sp_rename 'tb_Medicos.Nombre', 'NombreCompleto', 'COLUMN'
GO

*/

/*
-- OPCIÓN 3: Si necesitas agregar columnas faltantes
-- Descomentar estas líneas:

-- Agregar NombreCompleto si no existe
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('tb_Medicos') AND name = 'NombreCompleto')
BEGIN
    ALTER TABLE tb_Medicos ADD NombreCompleto VARCHAR(100) NOT NULL DEFAULT 'Sin Nombre'
END
GO

-- Agregar IdEspecialidad si no existe
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('tb_Medicos') AND name = 'IdEspecialidad')
BEGIN
    ALTER TABLE tb_Medicos ADD IdEspecialidad INT NOT NULL DEFAULT 1
    ALTER TABLE tb_Medicos ADD CONSTRAINT FK_Medicos_Especialidades 
        FOREIGN KEY (IdEspecialidad) REFERENCES tb_Especialidades(IdEspecialidad)
END
GO

-- Agregar CMP si no existe
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('tb_Medicos') AND name = 'CMP')
BEGIN
    ALTER TABLE tb_Medicos ADD CMP VARCHAR(20) NULL
END
GO

-- Agregar Correo si no existe (ya se agregó en el script anterior)
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('tb_Medicos') AND name = 'Correo')
BEGIN
    ALTER TABLE tb_Medicos ADD Correo VARCHAR(100) NULL
END
GO

-- Agregar Telefono si no existe
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('tb_Medicos') AND name = 'Telefono')
BEGIN
    ALTER TABLE tb_Medicos ADD Telefono VARCHAR(20) NULL
END
GO

*/

PRINT ''
PRINT '? Diagnóstico completado.'
PRINT '?? Copia el resultado de las columnas y compártelo para ayudarte mejor.'
PRINT ''
GO

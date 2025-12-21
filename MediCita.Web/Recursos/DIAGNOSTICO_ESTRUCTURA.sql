-- =============================================
-- SCRIPT DE DIAGNÓSTICO - ESTRUCTURA DE BASE DE DATOS
-- Ejecuta este script para ver qué estructura tiene tu BD
-- =============================================

USE BD_MediCita
GO

PRINT '?? DIAGNÓSTICO DE ESTRUCTURA DE BASE DE DATOS'
PRINT '=============================================='
PRINT ''

-- 1. ESTRUCTURA DE tb_Medicos
PRINT '?? ESTRUCTURA DE tb_Medicos:'
PRINT ''
SELECT 
    COLUMN_NAME AS Columna,
    DATA_TYPE AS TipoDato,
    IS_NULLABLE AS PermiteNull,
    CHARACTER_MAXIMUM_LENGTH AS Longitud
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'tb_Medicos'
ORDER BY ORDINAL_POSITION

PRINT ''
PRINT '?? RESTRICCIONES DE tb_Medicos:'
SELECT 
    CONSTRAINT_NAME AS Restriccion,
    CONSTRAINT_TYPE AS Tipo
FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
WHERE TABLE_NAME = 'tb_Medicos'

PRINT ''
PRINT '?? DATOS ACTUALES EN tb_Medicos:'
SELECT 
    IdMedico,
    CASE 
        WHEN EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('tb_Medicos') AND name = 'NombreCompleto') 
        THEN NombreCompleto
        ELSE 'N/A'
    END AS Nombre,
    CMP,
    IdEspecialidad
FROM tb_Medicos

PRINT ''
PRINT '=============================================='
PRINT ''

-- 2. ESTRUCTURA DE tb_Usuarios
PRINT '?? ESTRUCTURA DE tb_Usuarios:'
PRINT ''
SELECT 
    COLUMN_NAME AS Columna,
    DATA_TYPE AS TipoDato,
    IS_NULLABLE AS PermiteNull,
    CHARACTER_MAXIMUM_LENGTH AS Longitud
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'tb_Usuarios'
ORDER BY ORDINAL_POSITION

PRINT ''
PRINT '?? DATOS ACTUALES EN tb_Usuarios:'
SELECT 
    IdUsuario,
    NombreCompleto,
    Correo
FROM tb_Usuarios

PRINT ''
PRINT '=============================================='
PRINT ''

-- 3. VERIFICAR RESTRICCIÓN UNIQUE EN tb_Medicos
PRINT '?? VERIFICANDO RESTRICCIÓN UNIQUE EN tb_Medicos:'
PRINT ''
SELECT 
    i.name AS IndiceNombre,
    COL_NAME(ic.object_id, ic.column_id) AS Columna,
    i.is_unique AS EsUnico
FROM sys.indexes i
INNER JOIN sys.index_columns ic ON i.object_id = ic.object_id AND i.index_id = ic.index_id
WHERE i.object_id = OBJECT_ID('tb_Medicos')
    AND i.is_unique = 1

PRINT ''
PRINT '=============================================='
PRINT ''

-- 4. VERIFICAR VALORES NULL EN COLUMNAS UNIQUE
PRINT '?? VERIFICANDO VALORES NULL EN COLUMNAS CON RESTRICCIÓN UNIQUE:'
PRINT ''

-- Verificar si hay NULLs en CMP
SELECT 'CMP con NULL' AS Problema, COUNT(*) AS Cantidad
FROM tb_Medicos
WHERE CMP IS NULL

-- Verificar si hay duplicados de NULL en otras columnas unique
DECLARE @UniqueColumn VARCHAR(100)
DECLARE col_cursor CURSOR FOR
SELECT COL_NAME(ic.object_id, ic.column_id)
FROM sys.indexes i
INNER JOIN sys.index_columns ic ON i.object_id = ic.object_id AND i.index_id = ic.index_id
WHERE i.object_id = OBJECT_ID('tb_Medicos')
    AND i.is_unique = 1
    AND COL_NAME(ic.object_id, ic.column_id) != 'IdMedico'

OPEN col_cursor
FETCH NEXT FROM col_cursor INTO @UniqueColumn

WHILE @@FETCH_STATUS = 0
BEGIN
    PRINT 'Columna UNIQUE: ' + @UniqueColumn
    
    DECLARE @SQL NVARCHAR(MAX)
    SET @SQL = 'SELECT COUNT(*) AS ValoresNull FROM tb_Medicos WHERE ' + @UniqueColumn + ' IS NULL'
    EXEC sp_executesql @SQL
    
    FETCH NEXT FROM col_cursor INTO @UniqueColumn
END

CLOSE col_cursor
DEALLOCATE col_cursor

PRINT ''
PRINT '=============================================='
PRINT '? DIAGNÓSTICO COMPLETADO'
PRINT '=============================================='
GO

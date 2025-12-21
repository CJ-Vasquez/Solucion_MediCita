-- =============================================
-- STORED PROCEDURE: Registrar Cliente
-- Descripción: Inserta un nuevo usuario con rol de Cliente (IdRol = 2)
-- =============================================

USE BD_MediCita
GO

-- Eliminar el SP si ya existe
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'usp_RegistrarCliente')
    DROP PROCEDURE usp_RegistrarCliente
GO

CREATE PROCEDURE usp_RegistrarCliente
    @NombreCompleto VARCHAR(100),
    @Correo VARCHAR(100),
    @Clave VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        -- Insertar el nuevo cliente con IdRol = 2 (Cliente/Paciente)
        INSERT INTO tb_Usuarios (NombreCompleto, Correo, Clave, IdRol)
        VALUES (@NombreCompleto, @Correo, @Clave, 2)
        
        -- Retornar el ID del usuario recién creado
        SELECT SCOPE_IDENTITY() AS IdUsuario
        
    END TRY
    BEGIN CATCH
        -- En caso de error, retornar 0
        SELECT 0 AS IdUsuario
        
        -- Opcional: Registrar el error
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE()
        PRINT 'Error al registrar cliente: ' + @ErrorMessage
    END CATCH
END
GO

-- =============================================
-- PRUEBA DEL STORED PROCEDURE
-- =============================================

PRINT '? Stored Procedure usp_RegistrarCliente creado exitosamente'
PRINT ''
PRINT '?? PRUEBA RÁPIDA:'

-- Ejecutar el SP con datos de prueba
EXEC usp_RegistrarCliente 
    @NombreCompleto = 'Cliente Prueba Test',
    @Correo = 'cliente.test@ejemplo.com',
    @Clave = 'password123'

PRINT ''
PRINT '? Si ves un IdUsuario mayor a 0, el SP funciona correctamente'
PRINT ''

-- Verificar que se insertó correctamente (SIN usar tb_Rol)
SELECT TOP 1 
    IdUsuario,
    NombreCompleto,
    Correo,
    IdRol,
    CASE 
        WHEN IdRol = 1 THEN 'Administrador'
        WHEN IdRol = 2 THEN 'Cliente'
        ELSE 'Desconocido'
    END AS NombreRol
FROM tb_Usuarios 
WHERE Correo = 'cliente.test@ejemplo.com'
ORDER BY IdUsuario DESC

PRINT ''
PRINT '?? Cliente registrado exitosamente'
PRINT ''
PRINT '?? VALORES DE IdRol:'
PRINT '   1 = Administrador'
PRINT '   2 = Cliente/Paciente'
GO

-- =============================================
-- VERIFICAR ESTRUCTURA DE LA TABLA
-- =============================================

PRINT ''
PRINT '?? Estructura de tb_Usuarios:'
SELECT 
    COLUMN_NAME AS Columna,
    DATA_TYPE AS TipoDato,
    IS_NULLABLE AS PermiteNulos
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'tb_Usuarios'
ORDER BY ORDINAL_POSITION
GO

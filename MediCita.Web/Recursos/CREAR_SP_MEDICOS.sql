-- =============================================
-- SCRIPT SOLO PARA CREAR STORED PROCEDURES
-- Ejecuta este DESPUÉS de corregir la tabla
-- =============================================

USE BD_MediCita
GO

PRINT '?? Creando Stored Procedures para Médicos...'
PRINT ''

-- =============================================
-- 1. LISTAR TODOS LOS MÉDICOS
-- =============================================

IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'usp_ListarMedicos')
    DROP PROCEDURE usp_ListarMedicos
GO

CREATE PROCEDURE usp_ListarMedicos
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        m.IdMedico,
        m.NombreCompleto,
        m.IdEspecialidad,
        e.NombreEspec AS Especialidad,
        ISNULL(m.CMP, '') AS CMP,
        ISNULL(m.Correo, '') AS Correo,
        ISNULL(m.Telefono, '') AS Telefono
    FROM tb_Medicos m
    LEFT JOIN tb_Especialidades e ON m.IdEspecialidad = e.IdEspecialidad
    ORDER BY m.NombreCompleto
END
GO
PRINT '? SP usp_ListarMedicos creado'

-- =============================================
-- 2. OBTENER UN MÉDICO POR ID
-- =============================================

IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'usp_ObtenerMedico')
    DROP PROCEDURE usp_ObtenerMedico
GO

CREATE PROCEDURE usp_ObtenerMedico
    @IdMedico INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        IdMedico,
        NombreCompleto,
        IdEspecialidad,
        ISNULL(CMP, '') AS CMP,
        ISNULL(Correo, '') AS Correo,
        ISNULL(Telefono, '') AS Telefono
    FROM tb_Medicos
    WHERE IdMedico = @IdMedico
END
GO
PRINT '? SP usp_ObtenerMedico creado'

-- =============================================
-- 3. REGISTRAR UN NUEVO MÉDICO
-- =============================================

IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'usp_RegistrarMedico')
    DROP PROCEDURE usp_RegistrarMedico
GO

CREATE PROCEDURE usp_RegistrarMedico
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

-- =============================================
-- 4. ACTUALIZAR UN MÉDICO EXISTENTE
-- =============================================

IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'usp_EditarMedico')
    DROP PROCEDURE usp_EditarMedico
GO

CREATE PROCEDURE usp_EditarMedico
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

-- =============================================
-- 5. ELIMINAR UN MÉDICO
-- =============================================

IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'usp_EliminarMedico')
    DROP PROCEDURE usp_EliminarMedico
GO

CREATE PROCEDURE usp_EliminarMedico
    @IdMedico INT
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Verificar si tiene citas asociadas
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

-- =============================================
-- VERIFICACIÓN FINAL
-- =============================================

PRINT ''
PRINT '======================================'
PRINT '? STORED PROCEDURES CREADOS'
PRINT '======================================'
PRINT ''

PRINT '?? Stored Procedures disponibles:'
SELECT name AS [Stored Procedure]
FROM sys.procedures 
WHERE name LIKE 'usp_%Medico%'
ORDER BY name

PRINT ''
PRINT '?? PRUEBA RÁPIDA:'
PRINT ''

-- Probar el SP de listar
EXEC usp_ListarMedicos

PRINT ''
PRINT '?? ¡Todo listo! Ahora puedes usar el módulo de médicos en la aplicación.'
PRINT ''
GO

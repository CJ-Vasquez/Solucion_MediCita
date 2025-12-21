-- =============================================
-- STORED PROCEDURES PARA GESTIÓN DE MÉDICOS
-- =============================================

-- 1. Listar todos los médicos con su especialidad
GO
CREATE OR ALTER PROCEDURE usp_ListarMedicos
AS
BEGIN
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

-- 2. Obtener un médico por su ID
GO
CREATE OR ALTER PROCEDURE usp_ObtenerMedico
    @IdMedico INT
AS
BEGIN
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

-- 3. Registrar un nuevo médico
GO
CREATE OR ALTER PROCEDURE usp_RegistrarMedico
    @NombreCompleto VARCHAR(100),
    @IdEspecialidad INT,
    @CMP VARCHAR(20),
    @Correo VARCHAR(100),
    @Telefono VARCHAR(20)
AS
BEGIN
    INSERT INTO tb_Medicos (NombreCompleto, IdEspecialidad, CMP, Correo, Telefono)
    VALUES (@NombreCompleto, @IdEspecialidad, @CMP, @Correo, @Telefono)
END
GO

-- 4. Actualizar un médico existente
GO
CREATE OR ALTER PROCEDURE usp_EditarMedico
    @IdMedico INT,
    @NombreCompleto VARCHAR(100),
    @IdEspecialidad INT,
    @CMP VARCHAR(20),
    @Correo VARCHAR(100),
    @Telefono VARCHAR(20)
AS
BEGIN
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

-- 5. Eliminar un médico
GO
CREATE OR ALTER PROCEDURE usp_EliminarMedico
    @IdMedico INT
AS
BEGIN
    DELETE FROM tb_Medicos
    WHERE IdMedico = @IdMedico
END
GO

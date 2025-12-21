-- =============================================
-- SCRIPT DE VALIDACIÓN Y CREACIÓN DE ESTRUCTURA
-- PARA TABLA DE MÉDICOS
-- =============================================

-- Verificar si la tabla tb_Medicos existe, si no, crearla
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tb_Medicos]') AND type in (N'U'))
BEGIN
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
    PRINT 'Tabla tb_Medicos creada exitosamente'
END
ELSE
BEGIN
    PRINT 'La tabla tb_Medicos ya existe'
    
    -- Verificar y agregar columnas si no existen
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('tb_Medicos') AND name = 'Correo')
    BEGIN
        ALTER TABLE tb_Medicos ADD Correo VARCHAR(100) NULL
        PRINT 'Columna Correo agregada'
    END
    
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('tb_Medicos') AND name = 'Telefono')
    BEGIN
        ALTER TABLE tb_Medicos ADD Telefono VARCHAR(20) NULL
        PRINT 'Columna Telefono agregada'
    END
END
GO

-- =============================================
-- DATOS DE PRUEBA (OPCIONAL)
-- Comentar si no se desea insertar datos de prueba
-- =============================================

-- Solo insertar si la tabla está vacía
IF NOT EXISTS (SELECT * FROM tb_Medicos)
BEGIN
    -- Asegurarse de que existan especialidades
    IF EXISTS (SELECT * FROM tb_Especialidades WHERE IdEspecialidad = 1)
    BEGIN
        INSERT INTO tb_Medicos (NombreCompleto, IdEspecialidad, CMP, Correo, Telefono) VALUES
        ('Carlos Rodríguez Pérez', 1, '45678', 'crodriguez@medicita.com', '987654321'),
        ('María González López', 2, '45679', 'mgonzalez@medicita.com', '987654322'),
        ('José Martínez Ruiz', 3, '45680', 'jmartinez@medicita.com', '987654323'),
        ('Ana Fernández Torres', 4, '45681', 'afernandez@medicita.com', '987654324')
        
        PRINT 'Datos de prueba insertados exitosamente'
    END
END
GO

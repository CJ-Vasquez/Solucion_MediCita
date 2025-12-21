-- =============================================
-- SCRIPT FINAL ADAPTADO A TU ESTRUCTURA REAL
-- Versión 3.0 - Basado en diagnóstico real
-- =============================================

USE BD_MediCita
GO

PRINT '?? INICIANDO CARGA DE DATOS V3.0 (ADAPTADO A TU BD)...'
PRINT ''

-- =============================================
-- ANÁLISIS DE TU ESTRUCTURA:
-- tb_Medicos tiene:
-- - IdUsuario (INT, UNIQUE) <- Esto es diferente!
-- - IdEspecialidad (INT, FK)
-- - CMP (VARCHAR 20, NOT NULL)
-- - NombreCompleto (VARCHAR 100, NOT NULL)
-- - Correo (VARCHAR 100)
-- - Telefono (VARCHAR 15)
-- =============================================

-- =============================================
-- 1. INSERTAR ESPECIALIDADES
-- =============================================

PRINT '?? Verificando Especialidades...'

IF NOT EXISTS (SELECT * FROM tb_Especialidades WHERE NombreEspec = 'Cardiología')
BEGIN
    INSERT INTO tb_Especialidades (NombreEspec, Descripcion) VALUES
    ('Cardiología', 'Especialidad médica que se encarga del estudio, diagnóstico y tratamiento de las enfermedades del corazón'),
    ('Pediatría', 'Rama de la medicina que se especializa en la salud y las enfermedades de los niños'),
    ('Dermatología', 'Especialidad médica que se ocupa del conocimiento y estudio de la piel humana'),
    ('Traumatología', 'Rama de la medicina que se dedica al estudio de las lesiones del aparato locomotor'),
    ('Neurología', 'Especialidad médica que trata los trastornos del sistema nervioso'),
    ('Ginecología', 'Especialidad médica que trata las enfermedades del sistema reproductor femenino'),
    ('Oftalmología', 'Rama de la medicina que estudia las enfermedades de ojo y su tratamiento'),
    ('Otorrinolaringología', 'Especialidad médica que se encarga de la prevención y tratamiento de enfermedades del oído, nariz y garganta'),
    ('Psiquiatría', 'Rama de la medicina dedicada al estudio y tratamiento de los trastornos mentales'),
    ('Medicina General', 'Atención médica integral de primer nivel para todo tipo de pacientes')
    
    PRINT '? ' + CAST(@@ROWCOUNT AS VARCHAR) + ' especialidades insertadas'
END
ELSE
BEGIN
    PRINT '? Las especialidades ya existen'
END
GO

-- =============================================
-- 2. CREAR USUARIOS PRIMERO (porque IdUsuario es UNIQUE en tb_Medicos)
-- =============================================

PRINT ''
PRINT '?? Creando Usuarios para Médicos...'

DECLARE @UsuariosCreados INT = 0

-- Usuario para médico 1
IF NOT EXISTS (SELECT * FROM tb_Usuarios WHERE Correo = 'carlos.rodriguez@medicita.com')
BEGIN
    INSERT INTO tb_Usuarios (NombreCompleto, Correo, Clave, IdRol)
    VALUES ('Dr. Carlos Rodríguez Pérez', 'carlos.rodriguez@medicita.com', 'medico123', 2)
    SET @UsuariosCreados = @UsuariosCreados + 1
END

-- Usuario para médico 2
IF NOT EXISTS (SELECT * FROM tb_Usuarios WHERE Correo = 'maria.torres@medicita.com')
BEGIN
    INSERT INTO tb_Usuarios (NombreCompleto, Correo, Clave, IdRol)
    VALUES ('Dra. María Elena Torres', 'maria.torres@medicita.com', 'medico123', 2)
    SET @UsuariosCreados = @UsuariosCreados + 1
END

-- Usuario para médico 3
IF NOT EXISTS (SELECT * FROM tb_Usuarios WHERE Correo = 'jose.martinez@medicita.com')
BEGIN
    INSERT INTO tb_Usuarios (NombreCompleto, Correo, Clave, IdRol)
    VALUES ('Dr. José Luis Martínez', 'jose.martinez@medicita.com', 'medico123', 2)
    SET @UsuariosCreados = @UsuariosCreados + 1
END

-- Usuario para médico 4
IF NOT EXISTS (SELECT * FROM tb_Usuarios WHERE Correo = 'ana.fernandez@medicita.com')
BEGIN
    INSERT INTO tb_Usuarios (NombreCompleto, Correo, Clave, IdRol)
    VALUES ('Dra. Ana Patricia Fernández', 'ana.fernandez@medicita.com', 'medico123', 2)
    SET @UsuariosCreados = @UsuariosCreados + 1
END

-- Usuario para médico 5
IF NOT EXISTS (SELECT * FROM tb_Usuarios WHERE Correo = 'roberto.sanchez@medicita.com')
BEGIN
    INSERT INTO tb_Usuarios (NombreCompleto, Correo, Clave, IdRol)
    VALUES ('Dr. Roberto Carlos Sánchez', 'roberto.sanchez@medicita.com', 'medico123', 2)
    SET @UsuariosCreados = @UsuariosCreados + 1
END

-- Usuario para médico 6
IF NOT EXISTS (SELECT * FROM tb_Usuarios WHERE Correo = 'laura.gonzalez@medicita.com')
BEGIN
    INSERT INTO tb_Usuarios (NombreCompleto, Correo, Clave, IdRol)
    VALUES ('Dra. Laura Isabel González', 'laura.gonzalez@medicita.com', 'medico123', 2)
    SET @UsuariosCreados = @UsuariosCreados + 1
END

-- Usuario para médico 7
IF NOT EXISTS (SELECT * FROM tb_Usuarios WHERE Correo = 'miguel.ramirez@medicita.com')
BEGIN
    INSERT INTO tb_Usuarios (NombreCompleto, Correo, Clave, IdRol)
    VALUES ('Dr. Miguel Ángel Ramírez', 'miguel.ramirez@medicita.com', 'medico123', 2)
    SET @UsuariosCreados = @UsuariosCreados + 1
END

-- Usuario para médico 8
IF NOT EXISTS (SELECT * FROM tb_Usuarios WHERE Correo = 'fernando.lopez@medicita.com')
BEGIN
    INSERT INTO tb_Usuarios (NombreCompleto, Correo, Clave, IdRol)
    VALUES ('Dr. Fernando Javier López', 'fernando.lopez@medicita.com', 'medico123', 2)
    SET @UsuariosCreados = @UsuariosCreados + 1
END

-- Usuario para médico 9
IF NOT EXISTS (SELECT * FROM tb_Usuarios WHERE Correo = 'carmen.diaz@medicita.com')
BEGIN
    INSERT INTO tb_Usuarios (NombreCompleto, Correo, Clave, IdRol)
    VALUES ('Dra. Carmen Rosa Díaz', 'carmen.diaz@medicita.com', 'medico123', 2)
    SET @UsuariosCreados = @UsuariosCreados + 1
END

-- Usuario para médico 10
IF NOT EXISTS (SELECT * FROM tb_Usuarios WHERE Correo = 'ricardo.herrera@medicita.com')
BEGIN
    INSERT INTO tb_Usuarios (NombreCompleto, Correo, Clave, IdRol)
    VALUES ('Dr. Ricardo Alberto Herrera', 'ricardo.herrera@medicita.com', 'medico123', 2)
    SET @UsuariosCreados = @UsuariosCreados + 1
END

-- Usuarios para pacientes
IF NOT EXISTS (SELECT * FROM tb_Usuarios WHERE Correo = 'paciente1@test.com')
BEGIN
    INSERT INTO tb_Usuarios (NombreCompleto, Correo, Clave, IdRol)
    VALUES ('Juan Pérez García', 'paciente1@test.com', 'pass123', 3)
    SET @UsuariosCreados = @UsuariosCreados + 1
END

IF NOT EXISTS (SELECT * FROM tb_Usuarios WHERE Correo = 'paciente2@test.com')
BEGIN
    INSERT INTO tb_Usuarios (NombreCompleto, Correo, Clave, IdRol)
    VALUES ('María López Santos', 'paciente2@test.com', 'pass123', 3)
    SET @UsuariosCreados = @UsuariosCreados + 1
END

PRINT '? ' + CAST(@UsuariosCreados AS VARCHAR) + ' usuarios creados'
GO

-- =============================================
-- 3. INSERTAR MÉDICOS (usando IdUsuario)
-- =============================================

PRINT ''
PRINT '????? Insertando Médicos...'

DECLARE @MedicosInsertados INT = 0
DECLARE @IdUsu INT, @IdEsp INT

-- Obtener IDs de especialidades
SELECT @IdEsp = IdEspecialidad FROM tb_Especialidades WHERE NombreEspec LIKE '%Cardio%'

-- Médico 1: Cardiólogo
SELECT @IdUsu = IdUsuario FROM tb_Usuarios WHERE Correo = 'carlos.rodriguez@medicita.com'
IF @IdUsu IS NOT NULL AND NOT EXISTS (SELECT * FROM tb_Medicos WHERE IdUsuario = @IdUsu)
BEGIN
    INSERT INTO tb_Medicos (IdUsuario, IdEspecialidad, CMP, NombreCompleto, Correo, Telefono)
    VALUES (@IdUsu, @IdEsp, 'CMP45678', 'Dr. Carlos Rodríguez Pérez', 'carlos.rodriguez@medicita.com', '987654321')
    SET @MedicosInsertados = @MedicosInsertados + 1
END

-- Médico 2: Cardiólogo
SELECT @IdUsu = IdUsuario FROM tb_Usuarios WHERE Correo = 'maria.torres@medicita.com'
IF @IdUsu IS NOT NULL AND NOT EXISTS (SELECT * FROM tb_Medicos WHERE IdUsuario = @IdUsu)
BEGIN
    INSERT INTO tb_Medicos (IdUsuario, IdEspecialidad, CMP, NombreCompleto, Correo, Telefono)
    VALUES (@IdUsu, @IdEsp, 'CMP45679', 'Dra. María Elena Torres', 'maria.torres@medicita.com', '987654322')
    SET @MedicosInsertados = @MedicosInsertados + 1
END

-- Obtener ID de Pediatría
SELECT @IdEsp = IdEspecialidad FROM tb_Especialidades WHERE NombreEspec LIKE '%Pediatr%'

-- Médico 3: Pediatra
SELECT @IdUsu = IdUsuario FROM tb_Usuarios WHERE Correo = 'jose.martinez@medicita.com'
IF @IdUsu IS NOT NULL AND NOT EXISTS (SELECT * FROM tb_Medicos WHERE IdUsuario = @IdUsu)
BEGIN
    INSERT INTO tb_Medicos (IdUsuario, IdEspecialidad, CMP, NombreCompleto, Correo, Telefono)
    VALUES (@IdUsu, @IdEsp, 'CMP45680', 'Dr. José Luis Martínez', 'jose.martinez@medicita.com', '987654323')
    SET @MedicosInsertados = @MedicosInsertados + 1
END

-- Médico 4: Pediatra
SELECT @IdUsu = IdUsuario FROM tb_Usuarios WHERE Correo = 'ana.fernandez@medicita.com'
IF @IdUsu IS NOT NULL AND NOT EXISTS (SELECT * FROM tb_Medicos WHERE IdUsuario = @IdUsu)
BEGIN
    INSERT INTO tb_Medicos (IdUsuario, IdEspecialidad, CMP, NombreCompleto, Correo, Telefono)
    VALUES (@IdUsu, @IdEsp, 'CMP45681', 'Dra. Ana Patricia Fernández', 'ana.fernandez@medicita.com', '987654324')
    SET @MedicosInsertados = @MedicosInsertados + 1
END

-- Médico 5: Pediatra
SELECT @IdUsu = IdUsuario FROM tb_Usuarios WHERE Correo = 'roberto.sanchez@medicita.com'
IF @IdUsu IS NOT NULL AND NOT EXISTS (SELECT * FROM tb_Medicos WHERE IdUsuario = @IdUsu)
BEGIN
    INSERT INTO tb_Medicos (IdUsuario, IdEspecialidad, CMP, NombreCompleto, Correo, Telefono)
    VALUES (@IdUsu, @IdEsp, 'CMP45682', 'Dr. Roberto Carlos Sánchez', 'roberto.sanchez@medicita.com', '987654325')
    SET @MedicosInsertados = @MedicosInsertados + 1
END

-- Obtener ID de Dermatología
SELECT @IdEsp = IdEspecialidad FROM tb_Especialidades WHERE NombreEspec LIKE '%Dermato%'

-- Médico 6: Dermatólogo
SELECT @IdUsu = IdUsuario FROM tb_Usuarios WHERE Correo = 'laura.gonzalez@medicita.com'
IF @IdUsu IS NOT NULL AND NOT EXISTS (SELECT * FROM tb_Medicos WHERE IdUsuario = @IdUsu)
BEGIN
    INSERT INTO tb_Medicos (IdUsuario, IdEspecialidad, CMP, NombreCompleto, Correo, Telefono)
    VALUES (@IdUsu, @IdEsp, 'CMP45683', 'Dra. Laura Isabel González', 'laura.gonzalez@medicita.com', '987654326')
    SET @MedicosInsertados = @MedicosInsertados + 1
END

-- Médico 7: Dermatólogo
SELECT @IdUsu = IdUsuario FROM tb_Usuarios WHERE Correo = 'miguel.ramirez@medicita.com'
IF @IdUsu IS NOT NULL AND NOT EXISTS (SELECT * FROM tb_Medicos WHERE IdUsuario = @IdUsu)
BEGIN
    INSERT INTO tb_Medicos (IdUsuario, IdEspecialidad, CMP, NombreCompleto, Correo, Telefono)
    VALUES (@IdUsu, @IdEsp, 'CMP45684', 'Dr. Miguel Ángel Ramírez', 'miguel.ramirez@medicita.com', '987654327')
    SET @MedicosInsertados = @MedicosInsertados + 1
END

-- Obtener ID de Traumatología
SELECT @IdEsp = IdEspecialidad FROM tb_Especialidades WHERE NombreEspec LIKE '%Trauma%'

-- Médico 8: Traumatólogo
SELECT @IdUsu = IdUsuario FROM tb_Usuarios WHERE Correo = 'fernando.lopez@medicita.com'
IF @IdUsu IS NOT NULL AND NOT EXISTS (SELECT * FROM tb_Medicos WHERE IdUsuario = @IdUsu)
BEGIN
    INSERT INTO tb_Medicos (IdUsuario, IdEspecialidad, CMP, NombreCompleto, Correo, Telefono)
    VALUES (@IdUsu, @IdEsp, 'CMP45685', 'Dr. Fernando Javier López', 'fernando.lopez@medicita.com', '987654328')
    SET @MedicosInsertados = @MedicosInsertados + 1
END

-- Médico 9: Traumatólogo
SELECT @IdUsu = IdUsuario FROM tb_Usuarios WHERE Correo = 'carmen.diaz@medicita.com'
IF @IdUsu IS NOT NULL AND NOT EXISTS (SELECT * FROM tb_Medicos WHERE IdUsuario = @IdUsu)
BEGIN
    INSERT INTO tb_Medicos (IdUsuario, IdEspecialidad, CMP, NombreCompleto, Correo, Telefono)
    VALUES (@IdUsu, @IdEsp, 'CMP45686', 'Dra. Carmen Rosa Díaz', 'carmen.diaz@medicita.com', '987654329')
    SET @MedicosInsertados = @MedicosInsertados + 1
END

-- Obtener ID de Neurología
SELECT @IdEsp = IdEspecialidad FROM tb_Especialidades WHERE NombreEspec LIKE '%Neuro%'

-- Médico 10: Neurólogo
SELECT @IdUsu = IdUsuario FROM tb_Usuarios WHERE Correo = 'ricardo.herrera@medicita.com'
IF @IdUsu IS NOT NULL AND NOT EXISTS (SELECT * FROM tb_Medicos WHERE IdUsuario = @IdUsu)
BEGIN
    INSERT INTO tb_Medicos (IdUsuario, IdEspecialidad, CMP, NombreCompleto, Correo, Telefono)
    VALUES (@IdUsu, @IdEsp, 'CMP45687', 'Dr. Ricardo Alberto Herrera', 'ricardo.herrera@medicita.com', '987654330')
    SET @MedicosInsertados = @MedicosInsertados + 1
END

PRINT '? ' + CAST(@MedicosInsertados AS VARCHAR) + ' médicos insertados'
GO

-- =============================================
-- 4. INSERTAR MEDICAMENTOS
-- =============================================

PRINT ''
PRINT '?? Insertando Medicamentos...'

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'tb_Medicamentos') AND type in (N'U'))
BEGIN
    IF NOT EXISTS (SELECT * FROM tb_Medicamentos WHERE Nombre = 'Paracetamol 500mg')
    BEGIN
        INSERT INTO tb_Medicamentos (Nombre, Laboratorio, Precio, Stock) VALUES
        ('Paracetamol 500mg', 'Bayer', 8.50, 500),
        ('Ibuprofeno 400mg', 'Pfizer', 12.00, 450),
        ('Naproxeno 250mg', 'Genfar', 15.50, 300),
        ('Amoxicilina 500mg', 'Bayer', 25.00, 400),
        ('Azitromicina 500mg', 'Pfizer', 35.00, 350),
        ('Loratadina 10mg', 'Bayer', 18.00, 400),
        ('Omeprazol 20mg', 'Novartis', 28.00, 400),
        ('Metformina 850mg', 'Genfar', 28.00, 350),
        ('Losartán 50mg', 'Novartis', 38.00, 300),
        ('Aspirina 100mg', 'Bayer', 12.00, 500)
        
        PRINT '? ' + CAST(@@ROWCOUNT AS VARCHAR) + ' medicamentos insertados'
    END
    ELSE
    BEGIN
        PRINT '? Los medicamentos ya existen'
    END
END
GO

-- =============================================
-- 5. VERIFICACIÓN FINAL
-- =============================================

PRINT ''
PRINT '======================================'
PRINT '? CARGA DE DATOS COMPLETADA V3.0'
PRINT '======================================'
PRINT ''

DECLARE @Especialidades INT = 0, @Medicos INT = 0, @Medicamentos INT = 0, @Usuarios INT = 0

SELECT @Especialidades = COUNT(*) FROM tb_Especialidades
SELECT @Medicos = COUNT(*) FROM tb_Medicos
SELECT @Usuarios = COUNT(*) FROM tb_Usuarios

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'tb_Medicamentos') AND type in (N'U'))
    SELECT @Medicamentos = COUNT(*) FROM tb_Medicamentos

PRINT '?? RESUMEN DE DATOS:'
PRINT '   ? Especialidades: ' + CAST(@Especialidades AS VARCHAR)
PRINT '   ? Médicos: ' + CAST(@Medicos AS VARCHAR)
PRINT '   ? Medicamentos: ' + CAST(@Medicamentos AS VARCHAR)
PRINT '   ? Usuarios: ' + CAST(@Usuarios AS VARCHAR)

PRINT ''
PRINT '????? MUESTRA DE MÉDICOS:'
SELECT TOP 5
    m.IdMedico,
    m.NombreCompleto,
    e.NombreEspec AS Especialidad,
    m.CMP,
    m.Correo
FROM tb_Medicos m
INNER JOIN tb_Especialidades e ON m.IdEspecialidad = e.IdEspecialidad
ORDER BY m.IdMedico

PRINT ''
PRINT '?? ¡Base de datos lista para usar!'
PRINT '? Script completado exitosamente'
GO

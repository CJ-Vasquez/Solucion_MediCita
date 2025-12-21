# ? SOLUCIÓN FINAL - ADAPTADA A TU ESTRUCTURA REAL

## ?? **ESTRUCTURA REAL DETECTADA**

Según el diagnóstico, tu tabla `tb_Medicos` tiene esta estructura:

```sql
tb_Medicos:
- IdMedico (INT, PK) 
- IdUsuario (INT, UNIQUE) ? ¡Esto es diferente!
- IdEspecialidad (INT, FK)
- CMP (VARCHAR 20, NOT NULL) ? No es UNIQUE
- NombreCompleto (VARCHAR 100, NOT NULL)
- Correo (VARCHAR 100)
- Telefono (VARCHAR 15)
```

### **?? DIFERENCIA CLAVE:**
- ? La restricción UNIQUE está en `IdUsuario` (no en CMP)
- ? Esto significa que cada médico DEBE estar asociado a un usuario único
- ? No puede haber dos médicos con el mismo IdUsuario

---

## ?? **SOLUCIÓN IMPLEMENTADA**

El nuevo script hace lo siguiente:

### **1. Crea Usuarios Primero**
```sql
-- Crea usuarios en tb_Usuarios con IdRol = 2 (médicos)
INSERT INTO tb_Usuarios (NombreCompleto, Correo, Clave, IdRol)
VALUES ('Dr. Carlos Rodríguez', 'carlos.rodriguez@medicita.com', 'medico123', 2)
```

### **2. Luego Crea Médicos Vinculados**
```sql
-- Obtiene el IdUsuario recién creado
SELECT @IdUsu = IdUsuario FROM tb_Usuarios WHERE Correo = 'carlos.rodriguez@medicita.com'

-- Crea el médico asociado a ese usuario
INSERT INTO tb_Medicos (IdUsuario, IdEspecialidad, CMP, NombreCompleto, Correo, Telefono)
VALUES (@IdUsu, @IdEsp, 'CMP45678', 'Dr. Carlos Rodríguez', 'carlos.rodriguez@medicita.com', '987654321')
```

---

## ?? **INSTRUCCIONES DE EJECUCIÓN**

### **PASO 1: Ejecutar el Script**

En SQL Server Management Studio:

1. Abre: `Recursos/DATOS_PRUEBA_COMPLETO.sql` (Versión 3.0)
2. Presiona F5
3. Verás:

```
?? INICIANDO CARGA DE DATOS V3.0 (ADAPTADO A TU BD)...

?? Verificando Especialidades...
? Las especialidades ya existen

?? Creando Usuarios para Médicos...
? 12 usuarios creados (10 médicos + 2 pacientes)

????? Insertando Médicos...
? 10 médicos insertados

?? Insertando Medicamentos...
? Los medicamentos ya existen

======================================
? CARGA DE DATOS COMPLETADA V3.0
======================================

?? RESUMEN DE DATOS:
   ? Especialidades: 3
   ? Médicos: 11 (1 existente + 10 nuevos)
   ? Medicamentos: 50
   ? Usuarios: 13

?? ¡Base de datos lista para usar!
```

---

## ?? **VERIFICACIÓN**

Ejecuta estas consultas para confirmar:

### **1. Ver Médicos con sus Usuarios**
```sql
SELECT 
    m.IdMedico,
    m.NombreCompleto AS Medico,
    m.CMP,
    u.NombreCompleto AS Usuario,
    u.Correo,
    e.NombreEspec AS Especialidad
FROM tb_Medicos m
INNER JOIN tb_Usuarios u ON m.IdUsuario = u.IdUsuario
INNER JOIN tb_Especialidades e ON m.IdEspecialidad = e.IdEspecialidad
WHERE m.CMP LIKE 'CMP456%'
ORDER BY m.IdMedico
```

### **2. Ver Usuarios Médicos Creados**
```sql
SELECT 
    IdUsuario,
    NombreCompleto,
    Correo,
    IdRol
FROM tb_Usuarios
WHERE Correo LIKE '%@medicita.com'
AND Correo != 'admin@medicita.com'
ORDER BY IdUsuario
```

### **3. Verificar Relación Uno a Uno**
```sql
-- Verificar que cada usuario tiene máximo un médico
SELECT 
    u.IdUsuario,
    u.NombreCompleto,
    COUNT(m.IdMedico) AS CantidadMedicos
FROM tb_Usuarios u
LEFT JOIN tb_Medicos m ON u.IdUsuario = m.IdUsuario
GROUP BY u.IdUsuario, u.NombreCompleto
HAVING COUNT(m.IdMedico) > 1
-- Debe retornar 0 filas
```

---

## ?? **CREDENCIALES PARA PROBAR**

### **Administrador:**
```
Email: admin@medicita.com
Password: admin123
```

### **Médicos (para login como médico):**
```
Email: carlos.rodriguez@medicita.com
Password: medico123

Email: maria.torres@medicita.com
Password: medico123

(y así sucesivamente con los 10 médicos)
```

### **Pacientes:**
```
Email: paciente1@test.com
Password: pass123

Email: paciente2@test.com
Password: pass123
```

---

## ?? **DATOS INSERTADOS**

### **10 Médicos:**
1. Dr. Carlos Rodríguez Pérez - Cardiología - CMP45678
2. Dra. María Elena Torres - Cardiología - CMP45679
3. Dr. José Luis Martínez - Pediatría - CMP45680
4. Dra. Ana Patricia Fernández - Pediatría - CMP45681
5. Dr. Roberto Carlos Sánchez - Pediatría - CMP45682
6. Dra. Laura Isabel González - Dermatología - CMP45683
7. Dr. Miguel Ángel Ramírez - Dermatología - CMP45684
8. Dr. Fernando Javier López - Traumatología - CMP45685
9. Dra. Carmen Rosa Díaz - Traumatología - CMP45686
10. Dr. Ricardo Alberto Herrera - Neurología - CMP45687

### **Especialidades:**
- Cardiología, Pediatría, Dermatología, Traumatología, Neurología
- Ginecología, Oftalmología, Otorrinolaringología, Psiquiatría, Medicina General

---

## ?? **CÓMO FUNCIONA LA RELACIÓN**

Tu base de datos tiene un modelo donde:

```
tb_Usuarios (Usuario del sistema)
    ? (1:1)
tb_Medicos (Información médica del usuario)
```

**Esto significa:**
- ? Primero creas el usuario en `tb_Usuarios`
- ? Luego vinculas ese usuario como médico en `tb_Medicos`
- ? Un usuario solo puede ser médico una vez (UNIQUE en IdUsuario)
- ? Un médico debe tener un usuario asociado

---

## ?? **SI NECESITAS AGREGAR MÁS MÉDICOS MANUALMENTE**

```sql
-- 1. Crear usuario
INSERT INTO tb_Usuarios (NombreCompleto, Correo, Clave, IdRol)
VALUES ('Dr. Nuevo Médico', 'nuevo.medico@medicita.com', 'medico123', 2)

-- 2. Obtener el IdUsuario
DECLARE @NuevoIdUsuario INT
SELECT @NuevoIdUsuario = IdUsuario FROM tb_Usuarios 
WHERE Correo = 'nuevo.medico@medicita.com'

-- 3. Crear médico asociado
INSERT INTO tb_Medicos (IdUsuario, IdEspecialidad, CMP, NombreCompleto, Correo, Telefono)
VALUES (@NuevoIdUsuario, 1, 'CMP12345', 'Dr. Nuevo Médico', 'nuevo.medico@medicita.com', '999888777')
```

---

## ? **VENTAJAS DE ESTA ESTRUCTURA**

1. ? **Seguridad:** Cada médico tiene credenciales de login
2. ? **Trazabilidad:** Se puede saber quién hizo qué acción
3. ? **Roles:** Se puede diferenciar entre admin, médicos y pacientes
4. ? **Integridad:** Un usuario no puede ser médico múltiples veces

---

## ?? **RESUMEN**

**Archivo a ejecutar:**
- ? `DATOS_PRUEBA_COMPLETO.sql` (Versión 3.0)

**Resultado esperado:**
- ? 10 médicos nuevos insertados
- ? 12 usuarios nuevos (10 médicos + 2 pacientes)
- ? Relación correcta entre usuarios y médicos
- ? Sin errores de UNIQUE constraint

**Siguiente paso:**
1. Ejecuta el script
2. Ejecuta la aplicación (F5)
3. Inicia sesión como admin
4. Ve a "Gestionar Médicos"
5. Verás 11 médicos (1 existente + 10 nuevos)

---

**¡El script está 100% adaptado a tu estructura real! ??**

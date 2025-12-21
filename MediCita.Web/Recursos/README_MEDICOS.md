# ?? MÓDULO DE GESTIÓN DE MÉDICOS - INSTRUCCIONES DE INSTALACIÓN

## ?? ERROR DETECTADO

El error que estás experimentando es:
```
Could not find stored procedure 'usp_ListarMedicos'
```

Esto significa que **los stored procedures no existen en tu base de datos**.

---

## ?? PASOS PARA SOLUCIONAR EL ERROR

### 1?? Ejecutar el Script de Estructura de Tabla

1. Abre **SQL Server Management Studio (SSMS)** o **Azure Data Studio**
2. Conéctate a tu base de datos `BD_MEDICITA`
3. Abre el archivo: `MediCita.Web\Recursos\Tabla_Medicos_Estructura.sql`
4. Ejecuta el script completo (F5)

Este script:
- ? Verificará si la tabla `tb_Medicos` existe
- ? La creará si no existe
- ? Agregará las columnas `Correo` y `Telefono` si faltan
- ? Insertará datos de prueba (opcional)

---

### 2?? Ejecutar el Script de Stored Procedures

1. En SSMS/Azure Data Studio
2. Abre el archivo: `MediCita.Web\Recursos\StoredProcedures_Medicos.sql`
3. Ejecuta el script completo (F5)

Este script creará los siguientes procedures:
- ? `usp_ListarMedicos` - Lista todos los médicos
- ? `usp_ObtenerMedico` - Obtiene un médico por ID
- ? `usp_RegistrarMedico` - Registra un nuevo médico
- ? `usp_EditarMedico` - Actualiza un médico existente
- ? `usp_EliminarMedico` - Elimina un médico

---

### 3?? Verificar la Estructura de la Tabla tb_Medicos

La tabla debe tener esta estructura:

```sql
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
```

---

### 4?? Verificar que los Stored Procedures se Crearon Correctamente

Ejecuta esta consulta en SQL Server:

```sql
-- Ver todos los procedures de médicos
SELECT name 
FROM sys.procedures 
WHERE name LIKE 'usp_%Medico%'
ORDER BY name
```

Deberías ver 5 procedures:
- usp_EditarMedico
- usp_EliminarMedico
- usp_ListarMedicos
- usp_ObtenerMedico
- usp_RegistrarMedico

---

### 5?? Probar un Stored Procedure

```sql
-- Probar que funciona el SP de listar
EXEC usp_ListarMedicos
```

---

## ?? FUNCIONALIDADES IMPLEMENTADAS

### ? Backend (C#)
- **Entidad**: `Medico.cs` con propiedades expandidas
- **Interfaz**: `IMedicoService.cs` con métodos CRUD
- **Servicio**: `MedicoService.cs` con lógica de negocio
- **Controlador**: `MedicosController.cs` con acciones web
- **Inyección de Dependencias**: Registrado en `Program.cs`

### ? Frontend (Razor Views)
- **Index.cshtml**: Lista de médicos con tabla responsive
- **Crear.cshtml**: Formulario para registrar médicos
- **Editar.cshtml**: Formulario para actualizar médicos
- **Dashboard actualizado**: Botón habilitado para gestionar médicos

---

## ?? CÓMO USAR EL MÓDULO

1. **Iniciar sesión** en la aplicación
2. Ir al **Dashboard Administrativo**
3. Hacer clic en **"Gestionar Médicos"**
4. Desde allí podrás:
   - Ver la lista de médicos
   - Crear nuevo médico
   - Editar médico existente
   - Eliminar médico

---

## ?? ESTRUCTURA DE LA BASE DE DATOS

```
tb_Especialidades (ya existe)
    ??? IdEspecialidad (PK)
    ??? NombreEspec
    ??? Descripcion

tb_Medicos (nueva/actualizada)
    ??? IdMedico (PK)
    ??? NombreCompleto
    ??? IdEspecialidad (FK ? tb_Especialidades)
    ??? CMP (Colegio Médico del Perú)
    ??? Correo
    ??? Telefono
```

---

## ? SOLUCIÓN RÁPIDA

Si tienes prisa, ejecuta estos comandos en SQL Server en este orden:

```sql
-- 1. Ejecutar todo el contenido de: Tabla_Medicos_Estructura.sql
-- 2. Ejecutar todo el contenido de: StoredProcedures_Medicos.sql
-- 3. Reiniciar la aplicación
-- 4. Navegar a: https://localhost:7077/Medicos/Index
```

---

## ?? VERIFICACIÓN FINAL

Después de ejecutar los scripts, verifica:

```sql
-- 1. Ver si la tabla existe
SELECT * FROM tb_Medicos

-- 2. Ver los stored procedures
SELECT name FROM sys.procedures WHERE name LIKE '%Medico%'

-- 3. Probar un SP
EXEC usp_ListarMedicos
```

---

## ?? SOPORTE

Si después de ejecutar los scripts sigues teniendo problemas:

1. Verifica la cadena de conexión en `appsettings.json`
2. Asegúrate de estar conectado a la base de datos correcta
3. Verifica que el usuario tenga permisos para crear objetos en la BD
4. Revisa que la tabla `tb_Especialidades` exista y tenga datos

---

## ? ARCHIVOS CREADOS/MODIFICADOS

### Nuevos Archivos:
- ? `Servicios\Contrato\IMedicoService.cs`
- ? `Servicios\Implementacion\MedicoService.cs`
- ? `Controllers\MedicosController.cs`
- ? `Views\Medicos\Index.cshtml`
- ? `Views\Medicos\Crear.cshtml`
- ? `Views\Medicos\Editar.cshtml`
- ? `Recursos\StoredProcedures_Medicos.sql`
- ? `Recursos\Tabla_Medicos_Estructura.sql`

### Archivos Modificados:
- ? `Entidades\Medico.cs` - Agregadas propiedades
- ? `Program.cs` - Registrado IMedicoService
- ? `Views\Admin\Dashboard.cshtml` - Habilitado botón de médicos

---

**¡Listo! Una vez ejecutes los scripts SQL, el módulo de médicos funcionará correctamente! ??**

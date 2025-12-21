# ?? SCRIPT CORREGIDO - DATOS_PRUEBA_COMPLETO.sql

## ? PROBLEMAS SOLUCIONADOS

El script anterior tenía varios errores. Esta versión corregida soluciona:

### **1. Error de Clave Foránea en tb_Medicos ???**
**Error anterior:**
```
The DELETE statement conflicted with the REFERENCE constraint "FK__tb_Citas__IdMedi__46E78A0C"
```

**Solución:**
- ? Ahora NO elimina médicos existentes
- ? Verifica si ya existen antes de insertar (usando CMP como referencia)
- ? Respeta las referencias en tb_Citas

### **2. Error de Clave Única en tb_Medicos ???**
**Error anterior:**
```
Violation of UNIQUE KEY constraint 'UQ__tb_Medic__5B65BF96BBA42231'
```

**Solución:**
- ? Verifica si el CMP ya existe antes de insertar
- ? No intenta insertar duplicados

### **3. Error de Clave Foránea en tb_Medicamentos ???**
**Error anterior:**
```
The DELETE statement conflicted with the REFERENCE constraint "FK__tb_Detall__IdMed__4F7CD00D"
```

**Solución:**
- ? Ahora NO elimina medicamentos existentes
- ? Verifica si ya existen antes de insertar
- ? Respeta las referencias en tb_DetalleVenta

### **4. Error de Columnas en tb_Usuarios ???**
**Error anterior:**
```
Invalid column name 'Telefono'
Invalid column name 'Direccion'
Invalid column name 'EsAdministrador'
```

**Solución:**
- ? Verifica qué columnas existen en la tabla
- ? Adapta el INSERT según las columnas disponibles
- ? Solo inserta en columnas que existen

---

## ?? CÓMO USAR EL SCRIPT CORREGIDO

### **Opción 1: Primera Vez (Base de Datos Vacía)**

Si tu base de datos está vacía:

1. Abre **SQL Server Management Studio**
2. Conéctate a tu servidor
3. Selecciona la base de datos **BD_MediCita**
4. Abre el archivo: `Recursos/DATOS_PRUEBA_COMPLETO.sql`
5. Presiona **F5** para ejecutar

**Resultado esperado:**
```
? 10 especialidades insertadas
? 21 médicos insertados
? 47 medicamentos insertados
? 3 usuarios insertados
```

---

### **Opción 2: Base de Datos Con Datos**

Si ya tienes datos en tu base de datos:

1. El script **NO eliminará** tus datos existentes
2. Solo agregará los datos que **NO existan**
3. Respetará todas las relaciones de clave foránea

**Resultado esperado:**
```
? Las especialidades ya existen
? Los médicos de prueba ya existen
? Los medicamentos ya existen
? Los usuarios ya existen
```

---

## ?? QUÉ HACE EL SCRIPT

### **1. Especialidades**
- Verifica si existe "Cardiología"
- Si NO existe, inserta 10 especialidades
- Si existe, las deja intactas

### **2. Médicos**
- Obtiene los IDs de las especialidades existentes
- Verifica si existe el CMP 'CMP45678'
- Si NO existe, inserta 21 médicos
- Si existe, los deja intactos
- **NO elimina médicos que tengan citas**

### **3. Medicamentos**
- Verifica si existe "Paracetamol 500mg"
- Si NO existe, inserta 47 medicamentos
- Si existe, los deja intactos
- **NO elimina medicamentos que estén en ventas**

### **4. Usuarios**
- Verifica qué columnas tiene la tabla tb_Usuarios
- Adapta el INSERT según las columnas disponibles
- Inserta admin y 2 pacientes de prueba
- Si ya existe admin@medicita.com, no inserta nada

---

## ? VERIFICACIÓN DESPUÉS DE EJECUTAR

Ejecuta estas consultas para verificar:

```sql
-- 1. Ver especialidades
SELECT COUNT(*) AS Total FROM tb_Especialidades
SELECT * FROM tb_Especialidades

-- 2. Ver médicos
SELECT COUNT(*) AS Total FROM tb_Medicos
SELECT TOP 5 
    m.NombreCompleto,
    e.NombreEspec,
    m.CMP
FROM tb_Medicos m
INNER JOIN tb_Especialidades e ON m.IdEspecialidad = e.IdEspecialidad

-- 3. Ver medicamentos
SELECT COUNT(*) AS Total FROM tb_Medicamentos
SELECT TOP 5 Nombre, Laboratorio, Precio, Stock FROM tb_Medicamentos

-- 4. Ver usuarios
SELECT COUNT(*) AS Total FROM tb_Usuarios
SELECT NombreCompleto, Correo FROM tb_Usuarios
```

---

## ?? RESULTADOS ESPERADOS

### **Si la BD estaba vacía:**
| Tabla | Cantidad |
|-------|----------|
| Especialidades | 10 |
| Médicos | 21 |
| Medicamentos | 47 |
| Usuarios | 3 |

### **Si la BD tenía datos:**
| Tabla | Resultado |
|-------|-----------|
| Especialidades | Sin cambios |
| Médicos | Sin cambios o +21 |
| Medicamentos | Sin cambios o +47 |
| Usuarios | Sin cambios o +3 |

---

## ?? DIAGNÓSTICO DE PROBLEMAS

### **Si ves "Los médicos ya existen":**
- ? Normal: Ya tienes médicos con CMP45678 a CMP45698
- ? Tus datos están protegidos
- ? No se eliminó nada

### **Si ves "La tabla tb_Usuarios no tiene la estructura esperada":**
- ?? Tu tabla tb_Usuarios tiene columnas diferentes
- ? El script se adaptó y no causó error
- ?? Revisa la estructura de tu tabla:

```sql
SELECT COLUMN_NAME, DATA_TYPE 
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_NAME = 'tb_Usuarios'
ORDER BY ORDINAL_POSITION
```

### **Si aparece otro error:**
Comparte el mensaje de error completo para ayudarte mejor.

---

## ?? ESTRUCTURA MÍNIMA REQUERIDA

### **tb_Especialidades:**
```sql
IdEspecialidad INT (PK)
NombreEspec VARCHAR
Descripcion VARCHAR
```

### **tb_Medicos:**
```sql
IdMedico INT (PK)
NombreCompleto VARCHAR
IdEspecialidad INT (FK ? tb_Especialidades)
CMP VARCHAR (UNIQUE)
Correo VARCHAR
Telefono VARCHAR
```

### **tb_Medicamentos:**
```sql
IdMedicamento INT (PK)
Nombre VARCHAR
Laboratorio VARCHAR
Precio DECIMAL
Stock INT
```

### **tb_Usuarios (Columnas detectadas automáticamente):**
```sql
Mínimo:
- NombreCompleto VARCHAR
- Correo VARCHAR
- Clave VARCHAR

Opcionales (detectadas automáticamente):
- Telefono VARCHAR
- Direccion VARCHAR
- EsAdministrador BIT
```

---

## ?? VENTAJAS DEL SCRIPT CORREGIDO

1. ? **No destruye datos existentes**
2. ? **Respeta claves foráneas**
3. ? **Detecta columnas automáticamente**
4. ? **Se puede ejecutar múltiples veces sin error**
5. ? **Muestra mensajes claros de qué hizo**
6. ? **Verifica la estructura antes de insertar**
7. ? **Protege tus datos actuales**

---

## ?? PRÓXIMOS PASOS

1. ? **Ejecuta el script corregido**
2. ? **Verifica los resultados** con las consultas de verificación
3. ? **Ejecuta la aplicación** (F5 en Visual Studio)
4. ? **Inicia sesión** con admin@medicita.com / admin123
5. ? **Prueba el módulo de médicos**

---

## ?? CREDENCIALES PARA PROBAR

### **Administrador:**
```
Email: admin@medicita.com
Password: admin123
```

### **Pacientes:**
```
Email: paciente1@test.com
Password: pass123

Email: paciente2@test.com
Password: pass123
```

---

**¡El script está listo para usar de forma segura! ??**

# HPA III – Parcial #3 (Parte práctica)

**Herramientas de la Programación Aplicada III (.NET)**
Universidad Tecnológica de Panamá – Facultad de Ingeniería en Sistemas
Grupo: 1IL133 · II Semestre 2026 · Instructora: Ing. Irina Fong
Estudiante: Michael Hunt

Solución en C# (.NET 10) con tres proyectos, uno por cada parte del parcial.

```
HPA III parcial #3/
├── Parcial3.slnx
├── Parte1_Figuras/        Clase abstracta Figura + polimorfismo con Figura[]
├── Parte2_Json/           Serialización de un objeto a JSON con Newtonsoft.Json
├── Parte3_LoginBCrypt/    Login en Windows Forms con BCrypt.Net-Next + SQL Server
└── BaseDatos/
    ├── script.sql               Creación de la BD, tablas, procedimientos y vista
    ├── consultas_analisis.sql   Consultas para analizar los intentos de login
    └── LoginBCrypt_HPA3.bak     Backup de la base de datos
```

## Requisitos

- .NET SDK 10
- SQL Server (probado con SQL Server 2022 Express, instancia `.\SQLEXPRESS`)
- Windows (la Parte 3 es Windows Forms)

---

## Parte 1 – Jerarquía de figuras (15 pts)

| Elemento pedido | Dónde está |
|---|---|
| Clase abstracta `Figura` (molde obligatorio) | `Parte1_Figuras/Figura.cs` |
| Métodos abstractos `CalcularArea()` y `CalcularPerimetro()` implementados en los hijos | `Rectangulo.cs`, `Cuadrado.cs`, `Triangulo.cs` |
| Arreglo unidimensional del tipo padre `Figura[]` | `Program.cs` |
| Recorrido polimórfico con un bucle (nombre de la clase, área y perímetro) | `Program.cs` |

El triángulo recibe sus tres lados, valida la desigualdad triangular y calcula el área con la fórmula de Herón.

```
dotnet run --project Parte1_Figuras
```

```
#  Clase                 Área     Perímetro
-------------------------------------------
1  Rectangulo           15.00         16.00
2  Cuadrado             16.00         16.00
3  Triangulo             6.00         12.00
4  Rectangulo           15.00         19.00
5  Triangulo            15.59         18.00
```

En consola: 
<img width="1467" height="677" alt="image" src="https://github.com/user-attachments/assets/d7059600-cbb1-47ee-85d8-0ed697a30047" />


## Parte 2 – Trabajar con JSON usando Newtonsoft.Json (15 pts)

Se intercambian los datos de una persona (Nombre: "Cristell", Edad: 20, Carrera: "Ingenieria en Sistemas") en formato JSON.

1. Instalar: `dotnet add package Newtonsoft.Json`
2. Importar: `using Newtonsoft.Json;`
3. Crear el objeto con los datos.
4. Convertirlo a cadena JSON con `JsonConvert.SerializeObject(persona)`.

```
dotnet run --project Parte2_Json
```

```
Hello, World!
{"Nombre":"Michael","Edad":20,"Carrera":"Ingenieria en Sistemas"}
```

En consola: 
<img width="1442" height="306" alt="image" src="https://github.com/user-attachments/assets/b6554410-2ae8-4db6-a65b-9c2db538f127" />


## Parte 3 – Login con hash BCrypt (20 pts)

Aplicación Windows Forms que usa la biblioteca **BCrypt.Net-Next**.

| Requisito | Implementación |
|---|---|
| Login en C# que genera el hash con BCrypt | `FrmLogin` / `FrmRegistro` → `Negocio/AuthService.cs` |
| BD con tabla de usuarios | `dbo.Usuarios` (`PasswordHash CHAR(60)`, nunca la contraseña en texto plano) |
| Otra tabla con intentos fallidos y exitosos, usuario y fecha | `dbo.IntentosLogin` + vista `dbo.vw_ResumenIntentos` |
| Generar el hash de una palabra clave en la BD | `BCrypt.HashPassword(clave, 11)` al registrar un usuario |
| Verificar la contraseña original con `BCrypt.Verify()` | `AuthService.VerificarClave` dentro de `IniciarSesion` |
| Mostrar el nombre del usuario al acceder | `FrmBienvenida`: "¡Bienvenido, {NombreCompleto}!" + historial de sus intentos |
| Backup de la BD | `BaseDatos/LoginBCrypt_HPA3.bak` |

Otros detalles:
- Acceso a datos solo con procedimientos almacenados y parámetros, así que no hay SQL concatenado.
- Cada intento se registra en la bitácora con su detalle: `Acceso concedido`, `Contraseña incorrecta` o `Usuario no existe`.
- Si el usuario no existe, igual se ejecuta una verificación BCrypt contra un hash ficticio. Así la respuesta tarda lo mismo y no revela qué usuarios existen.

### Instalar la base de datos

**Opción A – Restaurar el backup** (incluye los usuarios de prueba):

```sql
RESTORE DATABASE LoginBCrypt_HPA3
FROM DISK = N'C:\ruta\al\repo\BaseDatos\LoginBCrypt_HPA3.bak'
WITH MOVE 'LoginBCrypt_HPA3'     TO N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\LoginBCrypt_HPA3.mdf',
     MOVE 'LoginBCrypt_HPA3_log' TO N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\LoginBCrypt_HPA3_log.ldf',
     REPLACE;
```

En SSMS también se puede hacer con clic derecho en *Databases* → *Restore Database…*. Si SQL Server no puede leer el archivo, primero cópialo a la carpeta `MSSQL\Backup` de la instancia.

**Opción B – Script desde cero** (sin usuarios; se crean desde la app):

```
sqlcmd -S .\SQLEXPRESS -E -i BaseDatos\script.sql
```

Si tu servidor es distinto de `.\SQLEXPRESS`, cambia la cadena de conexión en `Parte3_LoginBCrypt/App.config`.

### Ejecutar

```
dotnet run --project Parte3_LoginBCrypt
```

Usuarios de prueba (incluidos en el backup):

| Usuario | Contraseña | Nombre |
|---|---|---|
| `admin` | `Admin123*` | Michael Hunt |
| `cristell` | `Sistemas2026` | Cristell Rodriguez |

También puedes crear usuarios nuevos con el botón **Crear cuenta**.

Capturas de Pantalla 
Usuarios de Prueba Usados 

Michael Hunt: 
<img width="362" height="357" alt="image" src="https://github.com/user-attachments/assets/5d7d40a2-5042-4329-80ce-406eb431060d" />



<img width="632" height="447" alt="image" src="https://github.com/user-attachments/assets/0441948c-a1b6-4cba-8a8b-062c3f4d6d70" />


Cristell Rodriguez:
<img width="375" height="357" alt="image" src="https://github.com/user-attachments/assets/f0e6087c-bf08-4a02-a006-696b6dcadb99" />



<img width="626" height="450" alt="image" src="https://github.com/user-attachments/assets/b4a9ba18-b9a6-4787-af17-119b7c4b5583" />



Creacion de cuenta nueva 


<img width="372" height="425" alt="image" src="https://github.com/user-attachments/assets/f24714de-abba-4dca-b4dd-2f2c461524f8" />



<img width="376" height="421" alt="image" src="https://github.com/user-attachments/assets/ff199e7d-875c-42de-9aa0-e735070ba8f5" />



<img width="621" height="456" alt="image" src="https://github.com/user-attachments/assets/89ea4259-fc92-4082-959e-a35e724d92e7" />


Ejemplo de hash almacenado: `$2a$11$Azd9OJgIrgeP58.ipdnns.zlVMZRXZNmgbCJSFK0jbqsvvJEcp.Ku`
(`$2a$` = versión del algoritmo, `11` = factor de costo, luego 22 caracteres de sal y 31 del hash).

---

## Compilar todo

```
dotnet build Parcial3.slnx
```

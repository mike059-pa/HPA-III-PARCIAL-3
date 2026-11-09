/* =====================================================================
   HPA III - Parcial #3 - Parte 3: Login con BCrypt
   Base de datos: LoginBCrypt_HPA3 (SQL Server)
   Ejecutar:  sqlcmd -S .\SQLEXPRESS -E -i script.sql
   ===================================================================== */
USE master;
GO
IF DB_ID('LoginBCrypt_HPA3') IS NOT NULL
BEGIN
    ALTER DATABASE LoginBCrypt_HPA3 SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE LoginBCrypt_HPA3;
END
GO
CREATE DATABASE LoginBCrypt_HPA3;
GO
USE LoginBCrypt_HPA3;
GO

/* ---------- Tabla de usuarios (la contraseña se guarda SOLO como hash BCrypt) ---------- */
CREATE TABLE dbo.Usuarios
(
    IdUsuario      INT IDENTITY(1,1) CONSTRAINT PK_Usuarios PRIMARY KEY,
    Usuario        NVARCHAR(50)  NOT NULL CONSTRAINT UQ_Usuarios_Usuario UNIQUE,
    NombreCompleto NVARCHAR(100) NOT NULL,
    PasswordHash   CHAR(60)      NOT NULL,   -- BCrypt siempre produce 60 caracteres ($2a$11$...)
    FechaCreacion  DATETIME2(0)  NOT NULL CONSTRAINT DF_Usuarios_Fecha DEFAULT SYSDATETIME()
);
GO

/* ---------- Bitácora de intentos de inicio de sesión (exitosos y fallidos) ---------- */
CREATE TABLE dbo.IntentosLogin
(
    IdIntento INT IDENTITY(1,1) CONSTRAINT PK_IntentosLogin PRIMARY KEY,
    Usuario   NVARCHAR(50)  NOT NULL,          -- usuario digitado (exista o no)
    Exitoso   BIT           NOT NULL,
    Fecha     DATETIME2(0)  NOT NULL CONSTRAINT DF_Intentos_Fecha DEFAULT SYSDATETIME(),
    Detalle   NVARCHAR(100) NULL
);
GO
CREATE INDEX IX_IntentosLogin_Usuario_Fecha ON dbo.IntentosLogin (Usuario, Fecha DESC);
GO

/* ---------- Procedimientos almacenados ---------- */
CREATE PROCEDURE dbo.sp_RegistrarUsuario
    @Usuario NVARCHAR(50), @NombreCompleto NVARCHAR(100), @PasswordHash CHAR(60)
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (SELECT 1 FROM dbo.Usuarios WHERE Usuario = @Usuario)
        THROW 50001, 'El nombre de usuario ya existe.', 1;
    INSERT INTO dbo.Usuarios (Usuario, NombreCompleto, PasswordHash)
    VALUES (@Usuario, @NombreCompleto, @PasswordHash);
END
GO

CREATE PROCEDURE dbo.sp_ObtenerUsuario
    @Usuario NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT IdUsuario, Usuario, NombreCompleto, PasswordHash
    FROM dbo.Usuarios WHERE Usuario = @Usuario;
END
GO

CREATE PROCEDURE dbo.sp_RegistrarIntento
    @Usuario NVARCHAR(50), @Exitoso BIT, @Detalle NVARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO dbo.IntentosLogin (Usuario, Exitoso, Detalle)
    VALUES (@Usuario, @Exitoso, @Detalle);
END
GO

CREATE PROCEDURE dbo.sp_ConsultarIntentos
    @Usuario NVARCHAR(50) = NULL, @Top INT = 50
AS
BEGIN
    SET NOCOUNT ON;
    SELECT TOP (@Top) IdIntento, Usuario,
           CASE Exitoso WHEN 1 THEN 'Exitoso' ELSE 'Fallido' END AS Resultado,
           Fecha, Detalle
    FROM dbo.IntentosLogin
    WHERE @Usuario IS NULL OR Usuario = @Usuario
    ORDER BY Fecha DESC, IdIntento DESC;
END
GO

/* ---------- Vista de resumen para análisis posterior ---------- */
CREATE VIEW dbo.vw_ResumenIntentos
AS
SELECT Usuario,
       COUNT(*)                                   AS TotalIntentos,
       SUM(CASE WHEN Exitoso = 1 THEN 1 ELSE 0 END) AS Exitosos,
       SUM(CASE WHEN Exitoso = 0 THEN 1 ELSE 0 END) AS Fallidos,
       MAX(Fecha)                                 AS UltimoIntento
FROM dbo.IntentosLogin
GROUP BY Usuario;
GO

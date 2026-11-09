/* Consultas de ejemplo para analizar la bitácora de intentos de login */
USE LoginBCrypt_HPA3;
GO

-- Resumen por usuario (total, exitosos, fallidos, último intento)
SELECT * FROM dbo.vw_ResumenIntentos ORDER BY Fallidos DESC;

-- Últimos 50 intentos (todos los usuarios)
EXEC dbo.sp_ConsultarIntentos;

-- Intentos de un usuario específico
EXEC dbo.sp_ConsultarIntentos @Usuario = N'admin';

-- Usuarios con 3 o más intentos fallidos en la última hora (posible ataque de fuerza bruta)
SELECT Usuario, COUNT(*) AS FallidosUltimaHora
FROM dbo.IntentosLogin
WHERE Exitoso = 0 AND Fecha >= DATEADD(HOUR, -1, SYSDATETIME())
GROUP BY Usuario
HAVING COUNT(*) >= 3;

-- Intentos por día
SELECT CAST(Fecha AS DATE) AS Dia,
       SUM(CASE WHEN Exitoso = 1 THEN 1 ELSE 0 END) AS Exitosos,
       SUM(CASE WHEN Exitoso = 0 THEN 1 ELSE 0 END) AS Fallidos
FROM dbo.IntentosLogin
GROUP BY CAST(Fecha AS DATE)
ORDER BY Dia DESC;

using System.Data;
using Microsoft.Data.SqlClient;
using Parte3_LoginBCrypt.Entidades;

namespace Parte3_LoginBCrypt.Datos;

/// <summary>Acceso a datos: solo procedimientos almacenados con parámetros (sin SQL concatenado).</summary>
public static class UsuarioDatos
{
    public static void Registrar(string usuario, string nombreCompleto, string passwordHash)
    {
        using var cn = Conexion.Crear();
        using var cmd = Conexion.Procedimiento(cn, "dbo.sp_RegistrarUsuario");
        cmd.Parameters.Add("@Usuario", SqlDbType.NVarChar, 50).Value = usuario;
        cmd.Parameters.Add("@NombreCompleto", SqlDbType.NVarChar, 100).Value = nombreCompleto;
        cmd.Parameters.Add("@PasswordHash", SqlDbType.Char, 60).Value = passwordHash;
        cmd.ExecuteNonQuery();
    }

    public static Usuario? Obtener(string usuario)
    {
        using var cn = Conexion.Crear();
        using var cmd = Conexion.Procedimiento(cn, "dbo.sp_ObtenerUsuario");
        cmd.Parameters.Add("@Usuario", SqlDbType.NVarChar, 50).Value = usuario;
        using var dr = cmd.ExecuteReader();
        if (!dr.Read()) return null;
        return new Usuario
        {
            IdUsuario = dr.GetInt32(0),
            NombreUsuario = dr.GetString(1),
            NombreCompleto = dr.GetString(2),
            PasswordHash = dr.GetString(3)
        };
    }

    public static void RegistrarIntento(string usuario, bool exitoso, string detalle)
    {
        using var cn = Conexion.Crear();
        using var cmd = Conexion.Procedimiento(cn, "dbo.sp_RegistrarIntento");
        cmd.Parameters.Add("@Usuario", SqlDbType.NVarChar, 50).Value = usuario;
        cmd.Parameters.Add("@Exitoso", SqlDbType.Bit).Value = exitoso;
        cmd.Parameters.Add("@Detalle", SqlDbType.NVarChar, 100).Value = detalle;
        cmd.ExecuteNonQuery();
    }

    public static DataTable ConsultarIntentos(string? usuario = null, int top = 50)
    {
        using var cn = Conexion.Crear();
        using var cmd = Conexion.Procedimiento(cn, "dbo.sp_ConsultarIntentos");
        cmd.Parameters.Add("@Usuario", SqlDbType.NVarChar, 50).Value = (object?)usuario ?? DBNull.Value;
        cmd.Parameters.Add("@Top", SqlDbType.Int).Value = top;
        var tabla = new DataTable();
        using var dr = cmd.ExecuteReader();
        tabla.Load(dr);
        return tabla;
    }
}

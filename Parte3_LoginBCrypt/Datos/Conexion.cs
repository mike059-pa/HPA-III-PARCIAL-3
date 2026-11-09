using System.Configuration;
using Microsoft.Data.SqlClient;

namespace Parte3_LoginBCrypt.Datos;

internal static class Conexion
{
    private static readonly string Cadena =
        ConfigurationManager.ConnectionStrings["LoginBCrypt"]?.ConnectionString
        ?? throw new InvalidOperationException("No se encontró la cadena de conexión 'LoginBCrypt' en App.config.");

    public static SqlConnection Crear() => new(Cadena);

    /// <summary>Crea un comando de procedimiento almacenado con la conexión ya abierta.</summary>
    public static SqlCommand Procedimiento(SqlConnection cn, string nombre)
    {
        if (cn.State != System.Data.ConnectionState.Open) cn.Open();
        return new SqlCommand(nombre, cn) { CommandType = System.Data.CommandType.StoredProcedure };
    }
}

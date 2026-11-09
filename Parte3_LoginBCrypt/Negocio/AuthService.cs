using Microsoft.Data.SqlClient;
using Parte3_LoginBCrypt.Datos;
using Parte3_LoginBCrypt.Entidades;

namespace Parte3_LoginBCrypt.Negocio;

/// <summary>
/// Lógica de autenticación con BCrypt (biblioteca BCrypt.Net-Next).
/// - HashPassword: genera el hash con sal aleatoria incluida (formato $2a$11$...).
/// - Verify: compara la contraseña en texto plano contra el hash almacenado.
/// </summary>
public static class AuthService
{
    /// <summary>Factor de trabajo (costo): 2^11 iteraciones.</summary>
    public const int FactorTrabajo = 11;

    // Hash ficticio para que la verificación tarde lo mismo aunque el usuario no exista
    // (evita revelar qué usuarios existen midiendo el tiempo de respuesta).
    private static readonly string HashFicticio = BCrypt.Net.BCrypt.HashPassword("usuario-inexistente", FactorTrabajo);

    public static string GenerarHash(string clave) => BCrypt.Net.BCrypt.HashPassword(clave, FactorTrabajo);

    public static bool VerificarClave(string clave, string hash) => BCrypt.Net.BCrypt.Verify(clave, hash);

    public static void RegistrarUsuario(string usuario, string nombreCompleto, string clave)
    {
        usuario = usuario.Trim();
        nombreCompleto = nombreCompleto.Trim();
        if (usuario.Length is < 3 or > 50)
            throw new ArgumentException("El usuario debe tener entre 3 y 50 caracteres.");
        if (nombreCompleto.Length is 0 or > 100)
            throw new ArgumentException("Ingrese el nombre completo (máximo 100 caracteres).");
        if (clave.Length < 6)
            throw new ArgumentException("La contraseña debe tener al menos 6 caracteres.");

        try
        {
            UsuarioDatos.Registrar(usuario, nombreCompleto, GenerarHash(clave));
        }
        catch (SqlException ex) when (ex.Number == 50001)
        {
            throw new ArgumentException(ex.Message);
        }
    }

    /// <summary>
    /// Intenta iniciar sesión. Siempre registra el intento (exitoso o fallido) en la bitácora.
    /// Devuelve el usuario autenticado o null si las credenciales no son válidas.
    /// </summary>
    public static Usuario? IniciarSesion(string usuario, string clave)
    {
        usuario = usuario.Trim();
        var encontrado = UsuarioDatos.Obtener(usuario);

        bool valido = VerificarClave(clave, encontrado?.PasswordHash ?? HashFicticio) && encontrado is not null;

        string detalle = valido ? "Acceso concedido"
                       : encontrado is null ? "Usuario no existe"
                       : "Contraseña incorrecta";
        UsuarioDatos.RegistrarIntento(usuario, valido, detalle);

        return valido ? encontrado : null;
    }
}

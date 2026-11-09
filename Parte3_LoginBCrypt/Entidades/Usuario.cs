namespace Parte3_LoginBCrypt.Entidades;

public sealed class Usuario
{
    public int IdUsuario { get; init; }
    public string NombreUsuario { get; init; } = "";
    public string NombreCompleto { get; init; } = "";
    public string PasswordHash { get; init; } = "";
}

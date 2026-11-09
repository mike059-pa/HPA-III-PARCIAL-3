namespace Parte1_Figuras;

/// <summary>
/// Clase abstracta que actúa como molde obligatorio para todas las figuras.
/// No puede instanciarse directamente; cada figura concreta debe implementar
/// sus propios cálculos de área y perímetro.
/// </summary>
public abstract class Figura
{
    /// <summary>Nombre de la clase concreta, obtenido en tiempo de ejecución.</summary>
    public string Nombre => GetType().Name;

    public abstract double CalcularArea();

    public abstract double CalcularPerimetro();
}

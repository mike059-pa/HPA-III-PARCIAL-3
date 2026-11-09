namespace Parte1_Figuras;

public class Cuadrado : Figura
{
    public double Lado { get; }

    public Cuadrado(double lado)
    {
        if (lado <= 0)
            throw new ArgumentException("El lado debe ser mayor que cero.");
        Lado = lado;
    }

    public override double CalcularArea() => Lado * Lado;

    public override double CalcularPerimetro() => 4 * Lado;
}

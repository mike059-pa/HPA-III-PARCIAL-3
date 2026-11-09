namespace Parte1_Figuras;

public class Triangulo : Figura
{
    public double LadoA { get; }
    public double LadoB { get; }
    public double LadoC { get; }

    public Triangulo(double ladoA, double ladoB, double ladoC)
    {
        if (ladoA <= 0 || ladoB <= 0 || ladoC <= 0)
            throw new ArgumentException("Los lados deben ser mayores que cero.");
        if (ladoA + ladoB <= ladoC || ladoA + ladoC <= ladoB || ladoB + ladoC <= ladoA)
            throw new ArgumentException("Los lados no cumplen la desigualdad triangular.");
        LadoA = ladoA;
        LadoB = ladoB;
        LadoC = ladoC;
    }

    /// <summary>Área mediante la fórmula de Herón.</summary>
    public override double CalcularArea()
    {
        double s = CalcularPerimetro() / 2;
        return Math.Sqrt(s * (s - LadoA) * (s - LadoB) * (s - LadoC));
    }

    public override double CalcularPerimetro() => LadoA + LadoB + LadoC;
}

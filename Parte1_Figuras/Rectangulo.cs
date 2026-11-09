namespace Parte1_Figuras;

public class Rectangulo : Figura
{
    public double Base { get; }
    public double Altura { get; }

    public Rectangulo(double @base, double altura)
    {
        if (@base <= 0 || altura <= 0)
            throw new ArgumentException("La base y la altura deben ser mayores que cero.");
        Base = @base;
        Altura = altura;
    }

    public override double CalcularArea() => Base * Altura;

    public override double CalcularPerimetro() => 2 * (Base + Altura);
}

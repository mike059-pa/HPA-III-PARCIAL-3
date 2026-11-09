namespace Parte1_Figuras;


public abstract class Figura
{
    
    public string Nombre => GetType().Name;

    public abstract double CalcularArea();

    public abstract double CalcularPerimetro();
}

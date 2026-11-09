using Parte1_Figuras;

Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.WriteLine("=== Parcial #3 - Parte 1: Jerarquía de Figuras (polimorfismo) ===\n");

// Arreglo unidimensional (vector) del tipo padre: Figura[]
Figura[] figuras =
{
    new Rectangulo(5, 3),
    new Cuadrado(4),
    new Triangulo(3, 4, 5),
    new Rectangulo(7.5, 2),
    new Triangulo(6, 6, 6)
};

Console.WriteLine($"{"#",-3}{"Clase",-14}{"Área",12}{"Perímetro",14}");
Console.WriteLine(new string('-', 43));

// Recorrido polimórfico: cada elemento se trata como Figura, pero en tiempo
// de ejecución se invoca la implementación de la clase hija correspondiente.
for (int i = 0; i < figuras.Length; i++)
{
    Figura figura = figuras[i];
    Console.WriteLine($"{i + 1,-3}{figura.Nombre,-14}{figura.CalcularArea(),12:F2}{figura.CalcularPerimetro(),14:F2}");
}

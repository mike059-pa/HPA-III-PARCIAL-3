
// Importar
using Newtonsoft.Json;

//: Crear objeto con datos
var persona = new
{
    Nombre = "Michael ",
    Edad = 20,
    Carrera = "Ingenieria en Sistemas"
};

//  Convertir objeto a cadena JSON
string json = JsonConvert.SerializeObject(persona);

Console.WriteLine(json);

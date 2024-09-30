using System;

using System;

namespace Ecuacion_cuadratica
{
    static class Program
    {
        static void Main(string[] args)
        {
            // Solicitar los valores de A, B y C al usuario
            Console.WriteLine("Ingrese el valor de A (si desconoces A, ingresa 'X'):");
            string? inputA = Console.ReadLine() ?? throw new ArgumentNullException(nameof(inputA), "El valor de A no puede ser nulo.");
            
            Console.WriteLine("Ingrese el valor de B (si desconoces B, ingresa 'X'):");
            string? inputB = Console.ReadLine() ?? throw new ArgumentNullException(nameof(inputB), "El valor de B no puede ser nulo.");
            
            Console.WriteLine("Ingrese el valor de C (si desconoces C, ingresa 'X'):");
            string? inputC = Console.ReadLine() ?? throw new ArgumentNullException(nameof(inputC), "El valor de C no puede ser nulo.");

            try
            {
                double A, B, C;

                if (string.Equals(inputA, "X", StringComparison.OrdinalIgnoreCase))
                {
                    // A² = C² - B²
                    B = Convert.ToDouble(inputB);
                    C = Convert.ToDouble(inputC);

                    if (C * C > B * B)
                    {
                        A = Math.Sqrt(Math.Pow(C, 2) - Math.Pow(B, 2));
                        Console.WriteLine($"El valor de A es: {A}");
                    }
                    else
                    {
                        Console.WriteLine("Error: C debe ser mayor que B");
                    }
                }
                else if (string.Equals(inputB, "X", StringComparison.OrdinalIgnoreCase))
                {
                    A = Convert.ToDouble(inputA);
                    C = Convert.ToDouble(inputC);

                    if (C * C > A * A)
                    {
                        B = Math.Sqrt(Math.Pow(C, 2) - Math.Pow(A, 2));
                        Console.WriteLine($"El valor de B es: {B}");
                    }
                    else
                    {
                        Console.WriteLine("Error: C debe ser mayor que A");
                    }
                }
                else if (string.Equals(inputC, "X", StringComparison.OrdinalIgnoreCase))
                {
                    A = Convert.ToDouble(inputA);
                    B = Convert.ToDouble(inputB);
                    C = Math.Sqrt(Math.Pow(A, 2) + Math.Pow(B, 2));
                    Console.WriteLine($"El valor de C es: {C}");
                }
                else
                {
                    Console.WriteLine("Por favor ingrese 'X' para el valor desconocido.");
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Error: Asegúrate de ingresar valores numéricos válidos.");
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
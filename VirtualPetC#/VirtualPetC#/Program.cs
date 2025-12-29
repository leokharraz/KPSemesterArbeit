using VirtualPetC_.Core;
using VirtualPetC_.UI;

namespace VirtualPetC_
{
    /// <summary>
    /// Entry point for the Virtual Pet Simulator.
    /// Demonstrates OOP principles: Abstraction, Inheritance, Polymorphism,
    /// Encapsulation, and Dependency Inversion.
    /// </summary>
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // DEPENDENCY INVERSION: Create concrete UI implementation
                IUserInterface ui = new MenuSystem();

                // DEPENDENCY INJECTION: GameManager depends on IUserInterface abstraction
                GameManager game = new GameManager(ui);

                // Start the game
                game.Start();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nAn error occurred: {ex.Message}");
                Console.ResetColor();
                Console.WriteLine("\nPress any key to exit...");
                Console.ReadKey();
            }
        }
    }
}

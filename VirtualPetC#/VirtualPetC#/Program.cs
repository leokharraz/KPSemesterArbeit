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
                
                IUserInterface ui = new MenuSystem();

               
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

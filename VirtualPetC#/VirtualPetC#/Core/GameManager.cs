using VirtualPetC_.Core.Pets;
using VirtualPetC_.Enums;
using VirtualPetC_.UI;

namespace VirtualPetC_.Core;

/// <summary>
/// GameManager orchestrates the game flow and manages the pet lifecycle.
/// Demonstrates DEPENDENCY INVERSION PRINCIPLE - depends on IUserInterface abstraction
/// rather than concrete MenuSystem implementation.
/// </summary>
public class GameManager
{
    private Pet? currentPet;
    private readonly IUserInterface ui;
    private bool isGameRunning;

    /// <summary>
    /// Constructor demonstrates DEPENDENCY INJECTION.
    /// GameManager depends on IUserInterface interface, not concrete implementation.
    /// This allows swapping UI implementations without changing GameManager code.
    /// </summary>
    /// <param name="userInterface">Any class that implements IUserInterface</param>
    public GameManager(IUserInterface userInterface)
    {
        ui = userInterface;
        isGameRunning = false;
    }

    /// <summary>
    /// Starts and runs the virtual pet simulator.
    /// </summary>
    public void Start()
    {
        ui.DisplayWelcome();
        CreateNewPet();

        if (currentPet != null)
        {
            RunGameLoop();
        }
    }

    /// <summary>
    /// Creates a new pet based on user choice.
    /// Demonstrates POLYMORPHISM - different pet types instantiated based on choice.
    /// </summary>
    private void CreateNewPet()
    {
        PetType petType = ui.GetPetTypeChoice();
        string petName = ui.GetPetName();

        // POLYMORPHISM: Create different pet types, all stored as Pet base class reference
        currentPet = petType switch
        {
            PetType.Dog => new Dog(petName),
            PetType.Cat => new Cat(petName),
            PetType.Bird => new Bird(petName),
            _ => new Dog(petName)
        };

        Console.Clear();
        Console.WriteLine($"\n✨ You've adopted a {petType} named {petName}! ✨\n");

        // POLYMORPHISM: MakeSound() behavior depends on runtime type
        currentPet.MakeSound();

        ui.PauseForUser();
    }

    /// <summary>
    /// Main game loop - continues until player exits or pet dies.
    /// </summary>
    private void RunGameLoop()
    {
        Console.Clear();
        if (currentPet == null) return;

        isGameRunning = true;

        while (isGameRunning && currentPet.IsAlive)
        {
            Console.Clear();
            Console.WriteLine("\x1b[3J");
            Console.SetCursorPosition(0, 0);

            // Show status and warnings (DisplayStatus() will call Update())
            currentPet.DisplayStatus();
            ui.DisplayWarnings(currentPet);
            ui.DisplaySeparator();

            // Display menu and get user choice
            ui.DisplayActionMenu(currentPet);
            int choice = ui.GetValidChoice(1, 7);

            // Execute action based on choice
            ExecuteAction(choice);

            // Check if pet died after action
            if (!currentPet.IsAlive)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n{currentPet.Name} has died from neglect...");
                Console.ResetColor();
                ui.PauseForUser();
                break;
            }

            ui.PauseForUser();
        }

        // Game over
        ui.DisplayGameOver(currentPet);
    }

    /// <summary>
    /// Executes the player's chosen action.
    /// Demonstrates ENCAPSULATION - actions go through Pet class methods.
    /// </summary>
    /// <param name="choice">Player's menu choice</param>
    private void ExecuteAction(int choice)
    {
        if (currentPet == null) return;

        ui.DisplaySeparator();

        switch (choice)
        {
            case 1: // Feed
                currentPet.Feed();
                break;

            case 2: // Play
                // POLYMORPHISM: Play() behavior differs for Dog, Cat, Bird
                currentPet.Play();
                break;

            case 3: // Sleep
                currentPet.Sleep();
                break;

            case 4: // Interact (Make Sound)
                // POLYMORPHISM: MakeSound() is different for each pet type
                currentPet.MakeSound();
                break;

            case 5: // Use Special Ability
                // POLYMORPHISM: Different special abilities for each pet type
                ui.UseSpecialAbility(currentPet);
                break;

            case 6: // View Status
                Console.WriteLine("(Status already displayed above)");
                break;

            case 7: // Exit
                Console.WriteLine("\nAre you sure you want to exit? (y/n)");
                string? confirm = Console.ReadLine();
                if (confirm?.ToLower() == "y")
                {
                    isGameRunning = false;
                    Console.WriteLine($"\nGoodbye! Thanks for caring for {currentPet.Name}!");
                }
                break;

            default:
                Console.WriteLine("Invalid choice!");
                break;
        }
    }
}

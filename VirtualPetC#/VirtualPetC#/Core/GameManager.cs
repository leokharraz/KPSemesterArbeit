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
    private BasePet? currentPet;
    private readonly IUserInterface ui;
    private bool isGameRunning;
    private DateTime lastUpdateTime;

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
        lastUpdateTime = DateTime.Now;
    }


    // Starts and runs the virtual pet simulator.
    public void Start()
    {
        ui.DisplayWelcome();
        CreateNewPet();

        if (currentPet != null)
        {
            RunGameLoop();
        }
    }


    // Creates a new pet based on user choice.
    private void CreateNewPet()
    {
        PetType petType = ui.GetPetTypeChoice();
        string petName = ui.GetPetName();

        // Create different pet types, all stored as Pet base class reference
        currentPet = petType switch
        {
            PetType.Dog => new Dog(petName),
            PetType.Cat => new Cat(petName),
            PetType.Bird => new Bird(petName),
            _ => new Dog(petName)
        };

        Console.Clear();
        Console.WriteLine($"\nYou've adopted a {petType} named {petName}!\n");

        //MakeSound() behavior depends on runtime type
        currentPet.MakeSound();

        lastUpdateTime = DateTime.Now;
        ui.PauseForUser();
    }


    /// Updates the pet's stats based on time elapsed since last update.
    private void UpdatePet()
    {
        if (currentPet == null) return;

        DateTime now = DateTime.Now;
        double deltaTime = (now - lastUpdateTime).TotalSeconds;

        currentPet.Update(deltaTime);
        lastUpdateTime = now;
    }


    /// Main game loop - continues until player exits or pet dies.
    private void RunGameLoop()
    {
        Console.Clear();
        Console.WriteLine("\x1b[3J");
        if (currentPet == null) return;

        isGameRunning = true;

        while (isGameRunning && currentPet.IsAlive)
        {
            Console.Clear();
            Console.WriteLine("\x1b[3J");
            Console.SetCursorPosition(0, 0);

            // Update pet stats based on elapsed time
            UpdatePet();

            // Show status and warnings
            currentPet.DisplayStatus();
            ui.DisplayWarnings(currentPet);
            ui.DisplaySeparator();

            // Display menu and get user choice
            ui.DisplayActionMenu(currentPet);
            int choice = ui.GetValidChoice(1, 8);

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
    /// </summary>
    /// <param name="choice">Player's menu choice</param>
    private void ExecuteAction(int choice)
    {
        if (currentPet == null) return;

        ui.DisplaySeparator();

        switch (choice)
        {
            case 1: // Feed
                string feedResult = currentPet.Feed();
                ui.DisplayMessage(feedResult);
                break;

            case 2: // Play
                // POLYMORPHISM: Play() behavior differs for Dog, Cat, Bird
                string playResult = currentPet.Play();
                ui.DisplayMessage(playResult);
                break;

            case 3: // Sleep
                string sleepResult = currentPet.Sleep();
                ui.DisplayMessage(sleepResult);
                break;

            case 4: // Clean
                string cleanResult = currentPet.Clean();
                ui.DisplayMessage(cleanResult);
                break;

            case 5: // Interact (Make Sound)
                currentPet.MakeSound();
                break;

            case 6: // Use Special Ability
                ui.UseSpecialAbility(currentPet);
                break;

            case 7: // View Status
                Console.WriteLine("(Status already displayed above)");
                break;

            case 8: // Exit
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

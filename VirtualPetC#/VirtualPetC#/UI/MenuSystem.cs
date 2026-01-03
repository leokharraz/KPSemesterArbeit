using VirtualPetC_.Core.Pets;
using VirtualPetC_.Enums;

namespace VirtualPetC_.UI;

/// <summary>
/// MenuSystem handles all console input/output for the virtual pet simulator.
/// Demonstrates separation of concerns - UI logic separated from business logic.
/// Implements IUserInterface to demonstrate DEPENDENCY INVERSION PRINCIPLE.
/// </summary>
public class MenuSystem : IUserInterface
{
    /// <summary>
    /// Displays the welcome message and game instructions.
    /// </summary>
    public void DisplayWelcome()
    {
        Console.Clear();
        Console.WriteLine("╔════════════════════════════════════════════╗");
        Console.WriteLine("║   VIRTUAL PET SIMULATOR - OOP EDITION      ║");
        Console.WriteLine("╚════════════════════════════════════════════╝");
        Console.WriteLine("\nWelcome to the Virtual Pet Simulator!");
        Console.WriteLine("Take care of your pet by feeding, playing, and resting.");
        Console.WriteLine("Watch your pet grow from Baby to Adult to Elderly!");
        Console.WriteLine("\nPress any key to begin...");
        Console.ReadKey();
    }

    /// <summary>
    /// Gets the pet type choice from the user.
    /// </summary>
    /// <returns>PetType enum value</returns>
    public PetType GetPetTypeChoice()
    {
        Console.Clear();
        Console.WriteLine("╔════════════════════════════════════════════╗");
        Console.WriteLine("║          CHOOSE YOUR PET TYPE              ║");
        Console.WriteLine("╚════════════════════════════════════════════╝\n");

        Console.WriteLine("1. Dog  - Loyal companion with extra happiness from play");
        Console.WriteLine("2. Cat  - Independent pet with health regeneration (9 lives)");
        Console.WriteLine("3. Bird - Melodious friend that can sing to boost all stats");

        int choice = GetValidChoice(1, 3, "\nEnter your choice (1-3): ");

        return choice switch
        {
            1 => PetType.Dog,
            2 => PetType.Cat,
            3 => PetType.Bird,
            _ => PetType.Dog
        };
    }

    /// <summary>
    /// Gets the pet name from the user.
    /// </summary>
    /// <returns>Pet name string</returns>
    public string GetPetName()
    {
        Console.Write("\nEnter your pet's name: ");
        string? name = Console.ReadLine();

        while (string.IsNullOrWhiteSpace(name))
        {
            Console.Write("Name cannot be empty. Enter your pet's name: ");
            name = Console.ReadLine();
        }

        return name.Trim();
    }

    /// <summary>
    /// Displays the main action menu.
    /// </summary>
    /// <param name="pet">The current pet to show actions for</param>
    public void DisplayActionMenu(Pet pet)
    {
        Console.WriteLine("\n╔════════════════════════════════════════════╗");
        Console.WriteLine("║              WHAT WILL YOU DO?             ║");
        Console.WriteLine("╚════════════════════════════════════════════╝");
        Console.WriteLine("1. Feed");
        Console.WriteLine("2. Play");
        Console.WriteLine("3. Sleep");
        Console.WriteLine("4. Clean");
        Console.WriteLine("5. Interact (Make Sound)");
        Console.WriteLine("6. Use Special Ability");
        Console.WriteLine("7. View Status");
        Console.WriteLine("8. Exit Game");
    }

    /// <summary>
    /// Gets a valid menu choice from the user.
    /// </summary>
    /// <param name="min">Minimum valid choice</param>
    /// <param name="max">Maximum valid choice</param>
    /// <param name="prompt">Prompt message</param>
    /// <returns>Valid choice integer</returns>
    public int GetValidChoice(int min, int max, string prompt = "\nEnter your choice: ")
    {
        int choice;
        Console.Write(prompt);

        while (!int.TryParse(Console.ReadLine(), out choice) || choice < min || choice > max)
        {
            Console.Write($"Invalid input. Please enter a number between {min} and {max}: ");
        }

        return choice;
    }

    /// <summary>
    /// Displays a pet-specific special ability menu and executes it.
    /// Demonstrates runtime polymorphism - different behavior based on pet type.
    /// </summary>
    /// <param name="pet">The pet to use special ability</param>
    public void UseSpecialAbility(Pet pet)
    {
        Console.WriteLine($"\n--- Using {pet.Name}'s Special Ability ---");

        // POLYMORPHISM: Different behavior based on runtime type
        string result = pet switch
        {
            Dog dog when dog.CanUseAbility() => dog.UseSpecialAbility(),
            Dog dog => $"{dog.Name}'s loyalty is already active!",
            Cat cat when cat.CanUseAbility() => cat.UseSpecialAbility(),
            Cat cat => $"{cat.Name} has no lives remaining!",
            Bird bird when bird.CanUseAbility() => bird.UseSpecialAbility(),
            Bird bird => $"{bird.Name} needs to rest their voice! (Cooldown: {(int)bird.SongCooldownRemaining}s)",
            _ => "This pet doesn't have a special ability yet!"
        };

        Console.WriteLine(result);
    }

    /// <summary>
    /// Displays a warning if pet stats are critical.
    /// </summary>
    /// <param name="pet">Pet to check</param>
    public void DisplayWarnings(Pet pet)
    {
        bool hasWarning = false;

        // Check for illness first (highest priority)
        if (pet.IsIll)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n⚕️ WARNING: Your pet is ILL! Clean them immediately to cure! ⚕️");
            Console.ResetColor();
            hasWarning = true;
        }

        if (!pet.IsAlive)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n⚠️  WARNING: Your pet's health is critical! ⚠️");
            Console.ResetColor();
            hasWarning = true;
        }
        else
        {
            if (pet.Hunger < 30)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"\n⚠️  {pet.Name} is very hungry!");
                Console.ResetColor();
                hasWarning = true;
            }

            if (pet.Happiness < 30)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"\n⚠️  {pet.Name} is feeling sad!");
                Console.ResetColor();
                hasWarning = true;
            }

            if (pet.Health < 30)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"\n⚠️  {pet.Name}'s health is low!");
                Console.ResetColor();
                hasWarning = true;
            }

            if (pet.Cleanliness < 30)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"\n⚠️  {pet.Name} is getting dirty!");
                Console.ResetColor();
                hasWarning = true;
            }
        }

        if (hasWarning)
        {
            Console.WriteLine();
        }
    }

    /// <summary>
    /// Displays a separator line for better readability.
    /// </summary>
    public void DisplaySeparator()
    {
        Console.WriteLine("\n" + new string('─', 48));
    }

    /// <summary>
    /// Pauses and waits for user to press a key.
    /// </summary>
    public void PauseForUser(string message = "\nPress any key to continue...")
    {
        Console.WriteLine(message);
        Console.ReadKey();
    }

    /// <summary>
    /// Displays the game over screen.
    /// </summary>
    /// <param name="pet">The pet that was being cared for</param>
    public void DisplayGameOver(Pet pet)
    {
        Console.Clear();
        Console.WriteLine("╔════════════════════════════════════════════╗");
        Console.WriteLine("║              GAME OVER                     ║");
        Console.WriteLine("╚════════════════════════════════════════════╝\n");

        if (!pet.IsAlive)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{pet.Name} has passed away...");
            Console.ResetColor();
            Console.WriteLine($"\nYour pet lived for {pet.Age} minutes and reached the {pet.AgeStage} stage.");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Thanks for playing with {pet.Name}!");
            Console.ResetColor();
            Console.WriteLine($"\nFinal Stats:");
            pet.DisplayStatus();
        }

        Console.WriteLine("\nThank you for playing Virtual Pet Simulator!");
    }

    /// <summary>
    /// Displays a message to the user.
    /// </summary>
    /// <param name="message">Message to display</param>
    public void DisplayMessage(string message)
    {
        Console.WriteLine(message);
    }
}

using VirtualPetC_.Core.Pets;
using VirtualPetC_.Enums;

namespace VirtualPetC_.UI;

/// <summary>
/// Interface for user interface operations.
/// Demonstrates DEPENDENCY INVERSION PRINCIPLE - high-level modules (GameManager)
/// depend on abstractions (this interface) rather than concrete implementations (MenuSystem).
/// This allows for easy UI swapping (Console UI, GUI, Web UI, etc.) without changing game logic.
/// </summary>
public interface IUserInterface
{
    /// <summary>
    /// Displays the welcome message and game instructions.
    /// </summary>
    void DisplayWelcome();

    /// <summary>
    /// Gets the pet type choice from the user.
    /// </summary>
    /// <returns>PetType enum value</returns>
    PetType GetPetTypeChoice();

    /// <summary>
    /// Gets the pet name from the user.
    /// </summary>
    /// <returns>Pet name string</returns>
    string GetPetName();

    /// <summary>
    /// Displays the main action menu.
    /// </summary>
    /// <param name="pet">The current pet to show actions for</param>
    void DisplayActionMenu(Pet pet);

    /// <summary>
    /// Gets a valid menu choice from the user.
    /// </summary>
    /// <param name="min">Minimum valid choice</param>
    /// <param name="max">Maximum valid choice</param>
    /// <param name="prompt">Prompt message</param>
    /// <returns>Valid choice integer</returns>
    int GetValidChoice(int min, int max, string prompt = "\nEnter your choice: ");

    /// <summary>
    /// Uses the pet's special ability based on its type.
    /// </summary>
    /// <param name="pet">The pet to use special ability</param>
    void UseSpecialAbility(Pet pet);

    /// <summary>
    /// Displays warnings if pet stats are critical.
    /// </summary>
    /// <param name="pet">Pet to check</param>
    void DisplayWarnings(Pet pet);

    /// <summary>
    /// Displays a separator line for better readability.
    /// </summary>
    void DisplaySeparator();

    /// <summary>
    /// Pauses and waits for user to press a key.
    /// </summary>
    /// <param name="message">Message to display</param>
    void PauseForUser(string message = "\nPress any key to continue...");

    /// <summary>
    /// Displays the game over screen.
    /// </summary>
    /// <param name="pet">The pet that was being cared for</param>
    void DisplayGameOver(Pet pet);
}

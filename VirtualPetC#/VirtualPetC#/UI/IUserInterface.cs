using VirtualPetC_.Core.Pets;
using VirtualPetC_.Enums;

namespace VirtualPetC_.UI;


// Interface for user interface operations.
public interface IUserInterface
{

    // Displays the welcome message and game instructions.
    void DisplayWelcome();

    
    // Gets the pet type choice from the user.
    PetType GetPetTypeChoice();


    // Gets the pet name from the user.
    string GetPetName();


    // Displays the main action menu.
    void DisplayActionMenu(BasePet pet);


    // Gets a valid menu choice from the user.
    int GetValidChoice(int min, int max, string prompt = "\nEnter your choice: ");


    // Uses the pet's special ability based on its type.
    void UseSpecialAbility(BasePet pet);


    /// Displays warnings if pet stats are critical.
    void DisplayWarnings(BasePet pet);


    /// Displays a separator line for better readability.
    void DisplaySeparator();


    /// Pauses and waits for user to press a key.
    void PauseForUser(string message = "\nPress any key to continue...");


    /// Displays the game over screen.
    void DisplayGameOver(BasePet pet);

    /// Displays a message to the user.
    void DisplayMessage(string message);
}

package ui

import (
	"VirtualPetGo/pet"
	"fmt"
	"os"
	"os/exec"
)

// IUserInterface defines what a UI implementation must provide
type IUserInterface interface {
	DisplayWelcome()
	DisplayMainMenu()
	DisplayStatus(pet.Pet)
	DisplayMessage(string)
	ClearScreen()
	DisplayPetSelection()
	DisplayWarnings(pet.Pet)
}
type ConsoleUI struct{}

func NewConsoleUI() *ConsoleUI {
	return &ConsoleUI{}
}

// DisplayMainMenu shows the main menu options
func (cui *ConsoleUI) DisplayMainMenu() {
	fmt.Println("\n────────────────────────────────────────────────")
	fmt.Println("╔════════════════════════════════════════════╗")
	fmt.Println("║              WHAT WILL YOU DO?             ║")
	fmt.Println("╚════════════════════════════════════════════╝")
	fmt.Println("1. Feed")
	fmt.Println("2. Play")
	fmt.Println("3. Sleep")
	fmt.Println("4. Clean")
	fmt.Println("5. Interact (Make Sound)")
	fmt.Println("6. Use Special Ability")
	fmt.Println("7. View Status")
	fmt.Println("8. Exit Game")
	fmt.Print("\nChoose an action: ")
}

// DisplayStatus shows the pet's current status with progress bars
func (cui *ConsoleUI) DisplayStatus(p pet.Pet) {
	if p == nil {
		fmt.Println("No pet created yet!")
		return
	}

	status := p.GetStatus()

	// Header
	fmt.Printf("\n=== %s's Status ===\n", status.Name)
	fmt.Printf("Type: %s\n", status.Type)
	fmt.Printf("Age: %.2f minutes (%s)\n", status.Age, status.AgeStage)

	// Stats with progress bars
	fmt.Printf("Health:      %d/100 [%s]\n", status.Health, makeProgressBar(status.Health))
	fmt.Printf("Hunger:      %d/100 [%s]\n", status.Hunger, makeProgressBar(status.Hunger))
	fmt.Printf("Happiness:   %d/100 [%s]\n", status.Happiness, makeProgressBar(status.Happiness))
	fmt.Printf("Cleanliness: %d/100 [%s]\n", status.Cleanliness, makeProgressBar(status.Cleanliness))

	// Illness status
	if status.IsIll {
		fmt.Printf("\n🤒 ILLNESS: %s is sick with %s!\n", status.Name, status.IllnessName)
	}

	// Special ability
	fmt.Printf("\nSpecial Ability: %s %s\n", status.SpecialAbility, status.AbilityStatus)

	// Overall status
	fmt.Printf("Status: %s\n", status.StatusMessage)
	fmt.Println("===================")
}

// DisplayMessage shows a general message to the user
func DisplayMessage(message string) {
	fmt.Println(message)
}

// DisplayWelcome shows the welcome screen
func (cui *ConsoleUI) DisplayWelcome() {
	fmt.Println("╔════════════════════════════════════════════╗")
	fmt.Println("║       WELCOME TO VIRTUAL PET SIMULATOR     ║")
	fmt.Println("╚════════════════════════════════════════════╝")
}

// makeProgressBar creates a 10-character progress bar
func makeProgressBar(value int) string {
	filled := value / 10 // 0-10 filled blocks
	empty := 10 - filled

	bar := ""
	for i := 0; i < filled; i++ {
		bar += "█"
	}
	for i := 0; i < empty; i++ {
		bar += "░"
	}

	return bar
}
func (cui *ConsoleUI) DisplayMessage(message string) {
	fmt.Println(message)
}

// DisplayPetSelection shows the pet type selection menu
func (cui *ConsoleUI) DisplayPetSelection() {
	fmt.Println("\n╔════════════════════════════════════════════╗")
	fmt.Println("║          CHOOSE YOUR PET TYPE              ║")
	fmt.Println("╚════════════════════════════════════════════╝")
	fmt.Println("1. Dog   - Loyal companion with happiness boost")
	fmt.Println("2. Cat   - Independent pet with 9 lives")
	fmt.Println("3. Bird  - Cheerful singer with stat boosts")
	fmt.Print("\nSelect pet type (1-3): ")
}

func (cui *ConsoleUI) ClearScreen() {
	cmd := exec.Command("cmd", "/c", "cls") //Windows example, its tested
	cmd.Stdout = os.Stdout
	cmd.Run()

}

// DisplayWarnings shows warnings when pet stats are critically low
func (cui *ConsoleUI) DisplayWarnings(p pet.Pet) {
	if p == nil {
		return
	}

	status := p.GetStatus()
	hasWarning := false

	if !status.IsAlive {
		fmt.Println("\n⚠️  WARNING: Your pet's health is critical! ⚠️")
		hasWarning = true
	} else {
		if status.Hunger < 30 {
			fmt.Printf("\n⚠️  %s is very hungry!\n", status.Name)
			hasWarning = true
		}

		if status.Happiness < 30 {
			fmt.Printf("\n⚠️  %s is feeling sad!\n", status.Name)
			hasWarning = true
		}

		if status.Health < 30 {
			fmt.Printf("\n⚠️  %s's health is low!\n", status.Name)
			hasWarning = true
		}

		if status.Cleanliness < 30 {
			fmt.Printf("\n⚠️  %s is getting dirty!\n", status.Name)
			hasWarning = true
		}
	}

	if hasWarning {
		fmt.Println()
	}
}

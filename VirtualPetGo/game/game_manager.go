package game

import (
	"VirtualPetGo/pet"
	"VirtualPetGo/ui"
	"VirtualPetGo/utils"
	"fmt"
	"time"
)

type GameManager struct {
	currentPet     pet.Pet
	lastUpdateTime time.Time
	ui             ui.IUserInterface
}

func NewGameManager(userInterface ui.IUserInterface) *GameManager {
	return &GameManager{
		currentPet:     nil,
		lastUpdateTime: time.Now(),
		ui:             userInterface,
	}
}

func (gm *GameManager) CreatePet(petType int, name string, variant string) {
	switch petType {
	case 1:
		gm.currentPet = pet.NewDog(name, variant)

	}
	gm.lastUpdateTime = time.Now()
}

func (gm *GameManager) UpdatePet() {
	if gm.currentPet == nil {
		return
	}

	//Calculate time since last Update
	now := time.Now()
	deltaTime := now.Sub(gm.lastUpdateTime).Seconds()

	gm.currentPet.Update(deltaTime)
	gm.lastUpdateTime = now
}
func (gm *GameManager) GetPet() pet.Pet {
	return gm.currentPet
}
func (gm *GameManager) Start() {
	// Display welcome screen
	gm.ui.DisplayWelcome()

	// Create the pet
	gm.createPet()

	// Run the main game loop
	gm.gameLoop()
}

// createPet handles pet creation phase
func (gm *GameManager) createPet() {
	fmt.Print("\nEnter your pet's name: ")
	name, _ := utils.ReadString()

	fmt.Print("Enter your dog's breed: ")
	breed, _ := utils.ReadString()

	// Create dog (MVP - only Dog for now)
	gm.currentPet = pet.NewDog(name, breed)
	gm.lastUpdateTime = time.Now()

	fmt.Printf("\n%s the %s has been born!\n", name, breed)
}

// gameLoop is the main game loop
func (gm *GameManager) gameLoop() {
	for {
		gm.ui.ClearScreen()
		// Update pet stats based on elapsed time
		gm.updatePet()

		// Check if pet is still alive
		if !gm.currentPet.IsAlive() {
			fmt.Printf("\n💀 %s has died... Game Over.\n", gm.currentPet.GetStatus().Name)
			break
		}
		gm.ui.DisplayStatus(gm.currentPet)
		// Display menu
		gm.ui.DisplayMainMenu()

		// Get user choice (1-8)
		choice, _ := utils.ReadIntInRange(1, 8)

		// Handle the action, returns false if user wants to exit
		if !gm.handleAction(choice) {
			fmt.Println("\nThanks for playing! Goodbye!")
			break
		}

		fmt.Print("\nPress Enter to continue...")
		utils.WaitForEnter()
		//bufio.Reader.ReadString('\n')
	}
}

// updatePet updates the pet's stats based on time elapsed
func (gm *GameManager) updatePet() {
	if gm.currentPet == nil {
		return
	}

	now := time.Now()
	deltaTime := now.Sub(gm.lastUpdateTime).Seconds()

	gm.currentPet.Update(deltaTime)
	gm.lastUpdateTime = now
}

// handleAction processes user's menu choice
// Returns false if user wants to exit, true otherwise
func (gm *GameManager) handleAction(choice int) bool {
	switch choice {
	case 1: // Feed
		result := gm.currentPet.Feed()
		gm.ui.DisplayMessage(result)

	case 2: // Play
		result := gm.currentPet.Play()
		gm.ui.DisplayMessage(result)

	case 3: // Sleep
		result := gm.currentPet.Sleep()
		gm.ui.DisplayMessage(result)

	case 4: // Clean
		result := gm.currentPet.Clean()
		gm.ui.DisplayMessage(result)

	case 5: // Interact (Make Sound)
		result := gm.currentPet.Interact()
		gm.ui.DisplayMessage(result)

	case 6: // Use Special Ability
		if gm.currentPet.CanUseAbility() {
			result := gm.currentPet.UseSpecialAbility()
			gm.ui.DisplayMessage(result)
		} else {
			gm.ui.DisplayMessage("Special ability is not available right now!")
		}

	case 7: // View Status
		gm.ui.DisplayStatus(gm.currentPet)

	case 8: // Exit Game
		return false
	}

	return true // Continue game
}

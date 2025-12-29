package main

import (
	"VirtualPetGo/game"
	"VirtualPetGo/ui"
)

//TIP <p>To run your code, right-click the code and select <b>Run</b>.</p> <p>Alternatively, click
// the <icon src="AllIcons.Actions.Execute"/> icon in the gutter and select the <b>Run</b> menu item from here.</p>

func main() {
	// Initialize UI
	userInterface := ui.NewConsoleUI()

	// Initialize Game Manager with UI
	gameManager := game.NewGameManager(userInterface)

	// Start the game
	gameManager.Start()

}

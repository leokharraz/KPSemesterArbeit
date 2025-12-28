package main

import (
	"VirtualPetGo/pet"
	"VirtualPetGo/utils"
	"fmt"
)

//TIP <p>To run your code, right-click the code and select <b>Run</b>.</p> <p>Alternatively, click
// the <icon src="AllIcons.Actions.Execute"/> icon in the gutter and select the <b>Run</b> menu item from here.</p>

func main() {
	dog := pet.NewDog("test", "dalmatian")

	fmt.Printf(dog.Play() + "\n")
	fmt.Printf("" + dog.GetName() + "\n")
	utils.ReadIntInRange(1, 9)
}

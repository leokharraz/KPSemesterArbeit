package pet

import "fmt"

type Cat struct {
	BasePet
	furColor       string
	livesRemaining int
}

func NewCat(name, furColor string) *Cat {
	return &Cat{
		BasePet:        newBasePet(name),
		furColor:       furColor,
		livesRemaining: MaxLives,
	}
}

func (c *Cat) MakeSound() string {
	return "Meow~"
}
func (c *Cat) Interact() string {
	return c.GetName() + "is meowing " + c.MakeSound()
}

func (c *Cat) Play() string {
	c.BasePet.Play()

	c.setHappiness(c.GetHappiness() + 18)
	c.setHunger(c.GetHunger() + 5)

	return c.GetName() + "plays independently! Purrs contentedly."
}

func (c *Cat) Update(deltaTime float64) {
	c.BasePet.Update(deltaTime)

	if !c.IsAlive() && c.CanUseAbility() {
		c.setHealth(100)
		c.livesRemaining--
		fmt.Printf("\n🐱 %s used a life! %d lives remaining.\n", c.GetName(), c.livesRemaining)
	}

}

func (c *Cat) UseSpecialAbility() string {
	if c.CanUseAbility() {
		return c.GetName() + " has no lives remaining"
	}

	//Restore health to full
	c.setHealth(100)
	c.livesRemaining--
	return fmt.Sprintf("%s used Nine Lives! Health restored to 100. (%d lives remaining)",
		c.GetName(), c.livesRemaining)
}

func (c *Cat) CanUseAbility() bool {
	return c.livesRemaining > 0
}
func (c *Cat) GetStatus() Status {
	// Determine ability status text
	abilityStatus := fmt.Sprintf("(%d lives remaining)", c.livesRemaining)

	return Status{
		Name:     c.GetName(),
		Type:     "Cat",
		Age:      c.GetAge(),
		AgeStage: c.BasePet.getAgeStage(),

		// Core stats
		Health:      c.GetHealth(),
		Hunger:      c.GetHunger(),
		Happiness:   c.GetHappiness(),
		Cleanliness: c.GetCleanliness(),

		// Special ability info
		SpecialAbility: "Nine Lives - Can regenerate health!",
		AbilityStatus:  abilityStatus,

		// Overall status
		StatusMessage: c.BasePet.getStatusMessage(),
		IsAlive:       c.IsAlive(),
	}
}

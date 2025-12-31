package pet

import (
	"testing"
)

func TestNewDog(t *testing.T) {
	dog := NewDog("Max", "Golden Retriever")

	if dog.GetName() != "Max" {
		t.Errorf("Expected name 'Max', got '%s'", dog.GetName())
	}

	if dog.breed != "Golden Retriever" {
		t.Errorf("Expected breed 'Golden Retriever', got '%s'", dog.breed)
	}

	// Check initial stats
	if dog.GetHealth() != 100 {
		t.Errorf("Expected initial health 100, got %d", dog.GetHealth())
	}

	if dog.GetHunger() != 100 {
		t.Errorf("Expected initial hunger 100, got %d", dog.GetHunger())
	}
}

func TestDogMakeSound(t *testing.T) {
	dog := NewDog("Max", "Golden Retriever")

	sound := dog.MakeSound()
	expected := "Woof! Woof!"

	if sound != expected {
		t.Errorf("Expected sound '%s', got '%s'", expected, sound)
	}
}

func TestDogFeed(t *testing.T) {
	dog := NewDog("Max", "Golden Retriever")

	// Set hunger to 50
	dog.BasePet.setHunger(50)
	dog.setHappiness(70)
	initialHappiness := dog.GetHappiness()
	// Feed the dog
	dog.Feed()

	// Check hunger increased
	if dog.GetHunger() != 70 { // 50 + 20 from Feed
		t.Errorf("Expected hunger 70, got %d", dog.GetHunger())
	}

	// Check happiness increased
	if dog.GetHappiness() != initialHappiness+5 {
		t.Errorf("Expected happiness %d, got %d", initialHappiness+5, dog.GetHappiness())
	}
}

func TestDogPlay(t *testing.T) {
	dog := NewDog("Max", "Golden Retriever")
	dog.setHunger(60)
	dog.setHappiness(70)
	initialHunger := dog.GetHunger()
	initialHappiness := dog.GetHappiness()

	dog.Play()

	// Check hunger decreased
	if dog.GetHunger() >= initialHunger {
		t.Errorf("Expected hunger to decrease from %d, got %d", initialHunger, dog.GetHunger())
	}

	// Check happiness increased
	if dog.GetHappiness() <= initialHappiness {
		t.Errorf("Expected happiness to increase from %d, got %d", initialHappiness, dog.GetHappiness())
	}
}

func TestDogLoyaltyAbility(t *testing.T) {
	dog := NewDog("Max", "Golden Retriever")

	// Initially should be able to use ability
	if !dog.CanUseAbility() {
		t.Error("Dog should be able to use Loyalty ability initially")
	}

	// Use the ability
	dog.UseSpecialAbility()

	if !dog.loyaltyActive {
		t.Error("Loyalty should be active after using special ability")
	}

	// Should be on Cooldown
	if dog.CanUseAbility() {
		t.Error("Dogs Ability should be on cooldown")
	}
}

func TestDogLoyaltyReducesHappinessDecay(t *testing.T) {
	dog := NewDog("Max", "Golden Retriever")

	// Set stats to trigger happiness decay
	dog.BasePet.setHunger(20) // Below critical threshold
	dog.BasePet.setHappiness(100)

	// Activate loyalty
	dog.UseSpecialAbility()

	// Update for 1 second
	dog.Update(1.0)

	// Happiness should decay slower with loyalty active
	// Normal decay would be: 1.0 * 1.0 * 1.3 (baby multiplier) = 1.3
	// With loyalty: 1.3 * 0.5 = 0.65 (about 1 point)
	if dog.GetHappiness() < 98 {
		t.Errorf("Happiness decayed too much with loyalty active: %d", dog.GetHappiness())
	}
}

func TestDogGetStatus(t *testing.T) {
	dog := NewDog("Max", "Golden Retriever")

	status := dog.GetStatus()

	if status.Name != "Max" {
		t.Errorf("Expected status name 'Max', got '%s'", status.Name)
	}

	if status.Type != "Dog" {
		t.Errorf("Expected type 'Dog', got '%s'", status.Type)
	}

	if status.SpecialAbility != "Loyalty - Maintains happiness longer!" {
		t.Errorf("Unexpected special ability description: '%s'", status.SpecialAbility)
	}
}

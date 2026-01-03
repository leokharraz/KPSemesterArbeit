package pet

import "testing"

func TestBirdGetStatusWithIllness(t *testing.T) {
	bird := NewBird("Tweety", "Canary")

	// Make bird ill
	bird.BasePet.isIll = true
	bird.BasePet.illnessName = "Fleas"

	status := bird.GetStatus()

	if !status.IsIll {
		t.Error("Status should show bird is ill")
	}

	if status.IllnessName != "Fleas" {
		t.Errorf("Expected illness 'Fleas', got '%s'", status.IllnessName)
	}
}

func TestBirdSongDoesNotCureIllness(t *testing.T) {
	bird := NewBird("Tweety", "Canary")

	// Make bird ill
	bird.BasePet.isIll = true
	bird.BasePet.illnessName = "Stomach Bug"

	// Set stats low
	bird.BasePet.setHealth(50)
	bird.BasePet.setHunger(50)

	// Use Song ability
	bird.UseSpecialAbility()

	// Stats should increase
	if bird.GetHealth() <= 50 {
		t.Error("Song should boost health")
	}

	// But should still be ill (Song doesn't cure illness, only Clean does)
	if !bird.IsIll() {
		t.Error("Bird should still be ill after Song (only Clean cures)")
	}
}

func TestBirdCleanCuresIllness(t *testing.T) {
	bird := NewBird("Tweety", "Canary")

	// Make bird ill and set cleanliness low
	bird.BasePet.isIll = true
	bird.BasePet.illnessName = "Infection"
	bird.BasePet.setCleanliness(30)

	// Clean the bird
	bird.Clean()

	// Should be cured (cleanliness will be 70, > 60)
	if bird.IsIll() {
		t.Error("Bird should be cured after cleaning")
	}
}

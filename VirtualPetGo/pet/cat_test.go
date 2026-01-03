package pet

import "testing"

func TestCatGetStatusWithIllness(t *testing.T) {
	cat := NewCat("Whiskers", "Orange")

	// Make cat ill
	cat.BasePet.isIll = true
	cat.BasePet.illnessName = "Cold"

	status := cat.GetStatus()

	if !status.IsIll {
		t.Error("Status should show cat is ill")
	}

	if status.IllnessName != "Cold" {
		t.Errorf("Expected illness 'Cold', got '%s'", status.IllnessName)
	}
}

func TestCatIllnessWithNineLives(t *testing.T) {
	cat := NewCat("Whiskers", "Orange")

	// Make cat ill and set health low
	cat.BasePet.isIll = true
	cat.BasePet.illnessName = "Fever"
	cat.BasePet.setHealth(0)

	initialLives := cat.livesRemaining

	// Update should auto-revive
	cat.Update(0.1)

	// Should be alive
	if !cat.IsAlive() {
		t.Error("Cat should auto-revive with Nine Lives")
	}

	// Should still be ill (Nine Lives doesn't cure illness)
	if !cat.IsIll() {
		t.Error("Cat should still be ill after auto-revive")
	}

	// Lives should decrease
	if cat.livesRemaining != initialLives-1 {
		t.Errorf("Expected lives %d, got %d", initialLives-1, cat.livesRemaining)
	}
}

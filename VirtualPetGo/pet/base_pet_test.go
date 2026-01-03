package pet

import (
	"math/rand"
	"testing"
	"time"
)

func TestNewBasePet(t *testing.T) {
	basePet := newBasePet("TestPet")

	if basePet.GetName() != "TestPet" {
		t.Errorf("Expected name 'TestPet', got '%s'", basePet.GetName())
	}

	// Check initial stats are 100
	if basePet.GetHealth() != 100 {
		t.Errorf("Expected health 100, got %d", basePet.GetHealth())
	}

	if basePet.GetHunger() != 100 {
		t.Errorf("Expected hunger 100, got %d", basePet.GetHunger())
	}

	if basePet.GetHappiness() != 100 {
		t.Errorf("Expected happiness 100, got %d", basePet.GetHappiness())
	}

	if basePet.GetCleanliness() != 100 {
		t.Errorf("Expected cleanliness 100, got %d", basePet.GetCleanliness())
	}

	// Check not ill initially
	if basePet.IsIll() {
		t.Error("Pet should not be ill initially")
	}
}

func TestBasePetFeed(t *testing.T) {
	basePet := newBasePet("TestPet")
	basePet.setHunger(50)
	basePet.setHappiness(50)

	basePet.Feed()

	// Feed adds +20 hunger, +5 happiness
	if basePet.GetHunger() != 70 {
		t.Errorf("Expected hunger 70, got %d", basePet.GetHunger())
	}

	if basePet.GetHappiness() != 55 {
		t.Errorf("Expected happiness 55, got %d", basePet.GetHappiness())
	}
}

func TestBasePetSleep(t *testing.T) {
	basePet := newBasePet("TestPet")
	basePet.setHealth(50)
	basePet.setHunger(50)

	basePet.Sleep()

	// Sleep adds +20 health, -5 hunger
	if basePet.GetHealth() != 70 {
		t.Errorf("Expected health 70, got %d", basePet.GetHealth())
	}

	if basePet.GetHunger() != 45 {
		t.Errorf("Expected hunger 45, got %d", basePet.GetHunger())
	}
}

func TestBasePetPlay(t *testing.T) {
	basePet := newBasePet("TestPet")
	basePet.setHappiness(50)
	basePet.setHunger(50)

	basePet.Play()

	// Play adds +20 happiness, -10 hunger
	if basePet.GetHappiness() != 70 {
		t.Errorf("Expected happiness 70, got %d", basePet.GetHappiness())
	}

	if basePet.GetHunger() != 40 {
		t.Errorf("Expected hunger 40, got %d", basePet.GetHunger())
	}
}

func TestBasePetClean(t *testing.T) {
	basePet := newBasePet("TestPet")
	basePet.setCleanliness(50)
	basePet.setHappiness(50)

	basePet.Clean()

	// Clean adds +40 cleanliness, +10 happiness
	if basePet.GetCleanliness() != 90 {
		t.Errorf("Expected cleanliness 90, got %d", basePet.GetCleanliness())
	}

	if basePet.GetHappiness() != 60 {
		t.Errorf("Expected happiness 60, got %d", basePet.GetHappiness())
	}
}

func TestBasePetStatClamping(t *testing.T) {
	basePet := newBasePet("TestPet")

	// Test upper bound (100)
	basePet.setHealth(150)
	if basePet.GetHealth() != 100 {
		t.Errorf("Health should be clamped to 100, got %d", basePet.GetHealth())
	}

	// Test lower bound (0)
	basePet.setHealth(-10)
	if basePet.GetHealth() != 0 {
		t.Errorf("Health should be clamped to 0, got %d", basePet.GetHealth())
	}
}

func TestBasePetIsAlive(t *testing.T) {
	basePet := newBasePet("TestPet")

	// Should be alive with health > 0
	if !basePet.IsAlive() {
		t.Error("Pet should be alive with positive health")
	}

	// Should be dead with health = 0
	basePet.setHealth(0)
	if basePet.IsAlive() {
		t.Error("Pet should be dead with 0 health")
	}
}

func TestBasePetAgeStages(t *testing.T) {
	basePet := newBasePet("TestPet")

	// Just born - should be Baby
	if basePet.getAgeStage() != Baby {
		t.Errorf("Expected Baby stage, got %s", basePet.getAgeStage())
	}

	// Simulate age by changing birth time
	basePet.birthTime = time.Now().Add(-7 * time.Minute)
	if basePet.getAgeStage() != Adult {
		t.Errorf("Expected Adult stage at 7 minutes, got %s", basePet.getAgeStage())
	}

	basePet.birthTime = time.Now().Add(-20 * time.Minute)
	if basePet.getAgeStage() != Elderly {
		t.Errorf("Expected Elderly stage at 20 minutes, got %s", basePet.getAgeStage())
	}
}

func TestBasePetDecayMultiplier(t *testing.T) {
	basePet := newBasePet("TestPet")

	// Baby multiplier (1.3x)
	if basePet.getDecayMultiplier() != BabyDecayMultiplier {
		t.Errorf("Expected baby multiplier %f, got %f", BabyDecayMultiplier, basePet.getDecayMultiplier())
	}

	// Adult multiplier (1.0x)
	basePet.birthTime = time.Now().Add(-7 * time.Minute)
	if basePet.getDecayMultiplier() != AdultDecayMultiplier {
		t.Errorf("Expected adult multiplier %f, got %f", AdultDecayMultiplier, basePet.getDecayMultiplier())
	}

	// Elderly multiplier (0.7x)
	basePet.birthTime = time.Now().Add(-20 * time.Minute)
	if basePet.getDecayMultiplier() != ElderlyDecayMultiplier {
		t.Errorf("Expected elderly multiplier %f, got %f", ElderlyDecayMultiplier, basePet.getDecayMultiplier())
	}
}

func TestBasePetUpdate(t *testing.T) {
	basePet := newBasePet("TestPet")
	initialHunger := basePet.GetHunger()
	initialCleanliness := basePet.GetCleanliness()

	// Update for 1 second
	basePet.Update(1.0)

	// Hunger should decrease (2 points/sec * 1.3 baby multiplier = ~2.6)
	if basePet.GetHunger() >= initialHunger {
		t.Errorf("Hunger should decrease, was %d, now %d", initialHunger, basePet.GetHunger())
	}

	// Cleanliness should decrease (1.5 points/sec * 1.3 baby multiplier = ~1.95)
	if basePet.GetCleanliness() >= initialCleanliness {
		t.Errorf("Cleanliness should decrease, was %d, now %d", initialCleanliness, basePet.GetCleanliness())
	}
}

func TestBasePetIllness(t *testing.T) {
	rand.Seed(time.Now().UnixNano())
	basePet := newBasePet("TestPet")

	// Should not be ill initially
	if basePet.IsIll() {
		t.Error("Pet should not be ill initially")
	}

	if basePet.GetIllness() != "" {
		t.Error("Pet should have no illness name initially")
	}
}

func TestBasePetIllnessIncreasesWithDirtiness(t *testing.T) {
	rand.Seed(42) // Fixed seed for reproducible test

	// Test with high cleanliness (low chance)
	cleanPet := newBasePet("CleanPet")
	cleanPet.setCleanliness(90)
	illnessCount := 0

	for i := 0; i < 100; i++ {
		cleanPet.isIll = false // Reset
		cleanPet.checkForIllness()
		if cleanPet.IsIll() {
			illnessCount++
		}
	}

	// With high cleanliness, should have low illness rate
	cleanRate := float64(illnessCount) / 100.0

	// Test with low cleanliness (high chance)
	dirtyPet := newBasePet("DirtyPet")
	dirtyPet.setCleanliness(10)
	illnessCount = 0

	for i := 0; i < 100; i++ {
		dirtyPet.isIll = false // Reset
		dirtyPet.checkForIllness()
		if dirtyPet.IsIll() {
			illnessCount++
		}
	}

	dirtyRate := float64(illnessCount) / 100.0

	// Dirty pet should get sick more often than clean pet
	if dirtyRate <= cleanRate {
		t.Errorf("Dirty pet should get sick more often. Clean rate: %f, Dirty rate: %f", cleanRate, dirtyRate)
	}
}

func TestBasePetIllnessCausesHealthDecay(t *testing.T) {
	basePet := newBasePet("TestPet")
	basePet.setHealth(100)

	// Make pet ill
	basePet.isIll = true
	basePet.illnessName = "Cold"

	initialHealth := basePet.GetHealth()

	// Update for 1 second
	basePet.Update(1.0)

	// Health should decrease faster when ill
	if basePet.GetHealth() >= initialHealth {
		t.Errorf("Health should decrease when ill, was %d, now %d", initialHealth, basePet.GetHealth())
	}
}

func TestBasePetRecoverFromIllness(t *testing.T) {
	basePet := newBasePet("TestPet")

	// Make pet ill
	basePet.isIll = true
	basePet.illnessName = "Fleas"

	if !basePet.IsIll() {
		t.Error("Pet should be ill")
	}

	// Recover
	basePet.recoverFromIllness()

	if basePet.IsIll() {
		t.Error("Pet should not be ill after recovery")
	}

	if basePet.GetIllness() != "" {
		t.Error("Illness name should be empty after recovery")
	}
}

func TestBasePetCleanCuresIllness(t *testing.T) {
	basePet := newBasePet("TestPet")

	// Make pet ill and dirty
	basePet.isIll = true
	basePet.illnessName = "Infection"
	basePet.setCleanliness(50)

	// Clean the pet (cleanliness will be 90, > 60)
	message := basePet.Clean()

	// Should cure illness
	if basePet.IsIll() {
		t.Error("Pet should be cured after cleaning above 60")
	}

	// Message should mention cure
	if message == "" {
		t.Error("Clean message should not be empty")
	}
}

func TestBasePetGetStatusMessage(t *testing.T) {
	basePet := newBasePet("TestPet")

	// Full health - "Alive and well!"
	status := basePet.getStatusMessage()
	if status != "Alive and well!" {
		t.Errorf("Expected 'Alive and well!', got '%s'", status)
	}

	// Critical stats
	basePet.setHunger(15)
	basePet.setCleanliness(15)
	basePet.setHappiness(15)
	status = basePet.getStatusMessage()
	if status != "Critical condition! Needs immediate care!" {
		t.Errorf("Expected critical message, got '%s'", status)
	}

	// Dead
	basePet.setHealth(0)
	status = basePet.getStatusMessage()
	if status != "Dead..." {
		t.Errorf("Expected 'Dead...', got '%s'", status)
	}
}

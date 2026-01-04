package pet

import "time"

type Dog struct {
	BasePet
	loyaltyActive  bool
	loyaltyEndTime time.Time
}

func NewDog(name string) *Dog {
	return &Dog{
		BasePet:       newBasePet(name),
		loyaltyActive: false,
	}
}

func (d *Dog) Play() string {
	// Call base play behaviour
	d.BasePet.Play()

	//Add extra happiness for fetch
	d.setHappiness(d.GetHappiness() + 20)
	d.setHunger(d.GetHunger() - 10)

	return d.GetName() + " loves playing fetch! Extra happiness gained"
}

func (d *Dog) MakeSound() string {
	return "Woof! Woof!"
}
func (d *Dog) Interact() string {
	return d.GetName() + " says " + d.MakeSound()
}
func (d *Dog) getHappinessDecayModifier() float64 {
	if d.loyaltyActive {
		return LoyaltyHappinessReduction // 0.5
	}
	return 1.0
}

func (d *Dog) Update(deltaTime float64) {
	// Call base update (which uses our modifier)
	d.BasePet.Update(deltaTime)

	// Check if loyalty has expired
	if d.loyaltyActive && time.Now().After(d.loyaltyEndTime) {
		d.loyaltyActive = false
	}
}
func (d *Dog) UseSpecialAbility() string {
	d.loyaltyActive = true
	d.loyaltyEndTime = time.Now().Add(time.Duration(LoyaltyDuration) * time.Second)

	return d.GetName() + " is feeling extra loyal! Happiness will decay slower for the next 60 seconds."
}
func (d *Dog) CanUseAbility() bool {
	return !d.loyaltyActive // Can only use when loyalty is not currently active
}
func (d *Dog) GetStatus() Status {
	// Determine ability status text
	abilityStatus := "(Inactive)"
	if d.loyaltyActive {
		abilityStatus = "(Active)"
	}

	return Status{
		Name:     d.GetName(),
		Type:     "Dog",
		Age:      d.GetAge(),
		AgeStage: d.BasePet.getAgeStage(),

		// Core stats
		Health:      d.GetHealth(),
		Hunger:      d.GetHunger(),
		Happiness:   d.GetHappiness(),
		Cleanliness: d.GetCleanliness(),

		// Special ability info
		SpecialAbility: "Loyalty - Maintains happiness longer!",
		AbilityStatus:  abilityStatus,

		// Overall status
		StatusMessage: d.BasePet.getStatusMessage(),
		IsAlive:       d.IsAlive(),
		// Illness status
		IsIll:       d.IsIll(),
		IllnessName: d.GetIllness(),
	}
}

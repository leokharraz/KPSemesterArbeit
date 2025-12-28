package pet

import "time"

type BasePet struct {
	name           string
	birthTime      time.Time
	health         int
	hunger         int
	happiness      int
	cleanliness    int
	lastUpdateTime time.Time
	sound          string
}

func newBasePet(name string) BasePet {
	now := time.Now()
	return BasePet{
		name:           name,
		birthTime:      now,
		health:         100,
		hunger:         100,
		happiness:      100,
		cleanliness:    100,
		lastUpdateTime: now,
	}
}
func (bp *BasePet) GetName() string {
	return bp.name
}

func (bp *BasePet) setHealth(value int) {
	bp.health = clampStat(value)
}
func (bp *BasePet) setHunger(value int) {
	bp.hunger = clampStat(value)
}
func (bp *BasePet) setHappiness(value int) {
	bp.happiness = clampStat(value)
}
func (bp *BasePet) setCleanliness(value int) {
	bp.cleanliness = clampStat(value)
}
func (bp *BasePet) GetHealth() int {
	return bp.health
}
func (bp *BasePet) GetHappiness() int {
	return bp.happiness
}
func (bp *BasePet) GetHunger() int {
	return bp.hunger
}
func (bp *BasePet) GetCleanliness() int {
	return bp.cleanliness
}

// GetAge calculates and returns the pet's age in minutes
func (bp *BasePet) GetAge() float64 {
	return time.Since(bp.birthTime).Minutes()
}
func (bp *BasePet) getAgeStage() AgeStage {
	age := bp.GetAge()

	if age < BabyMaxAge {
		return Baby
	} else if age < AdultMaxAge {
		return Adult
	} else {
		return Elderly
	}
}
func (bp *BasePet) getDecayMultiplier() float64 {
	switch bp.getAgeStage() {
	case Baby:
		return BabyDecayMultiplier // 1.3x
	case Adult:
		return AdultDecayMultiplier // 1.0x
	case Elderly:
		return ElderlyDecayMultiplier // 0.7x
	default:
		return AdultDecayMultiplier
	}
}

func (bp *BasePet) Feed() string {
	bp.setHunger(bp.GetHunger() + 20)
	bp.setHappiness(bp.GetHappiness() + 5)
	return bp.GetName() + " enjoyed the meal! Hunger restored"
}
func (bp *BasePet) Sleep() string {
	bp.setHealth(bp.GetHealth() + 20)
	bp.setHunger(bp.GetHunger() - 5)

	return bp.GetName() + " took a nice nap! Health Restored"
}
func (bp *BasePet) Play() string {
	bp.setHappiness(bp.GetHappiness() + 20)
	bp.setHunger(bp.GetHunger() - 10)

	return bp.GetName() + " is playing! Happiness increased, but got a bit hungry."
}

func (bp *BasePet) Clean() string {
	bp.setCleanliness(bp.GetCleanliness() + 40)
	bp.setHappiness(bp.GetHappiness() + 10)
	return bp.GetName() + " is now clean and fresh! Feels much better."
}
func (bp *BasePet) IsAlive() bool {
	return bp.health > 0
}
func (bp *BasePet) getHappinessDecayModifier() float64 {
	//used to change HappinessDecay in Update subclasses
	return 1.0 // Default: no modification
}

func (bp *BasePet) Update(deltaTime float64) {
	multiplier := bp.getDecayMultiplier()

	// Apply hunger decay
	hungerDecay := HungerDecayRate * deltaTime * multiplier
	bp.setHunger(bp.GetHunger() - int(hungerDecay))

	// Apply cleanliness decay
	cleanlinessDecay := CleanlinessDecayRate * deltaTime * multiplier
	bp.setCleanliness(bp.GetCleanliness() - int(cleanlinessDecay))

	// Apply happiness decay with modifier hook
	if bp.GetHunger() < CriticalStatThreshold || bp.GetCleanliness() < CriticalStatThreshold {
		happinessDecay := HappinessDecayRate * deltaTime * multiplier
		happinessDecay *= bp.getHappinessDecayModifier() // Hook for subclasses
		bp.setHappiness(bp.GetHappiness() - int(happinessDecay))
	}

	if bp.getCriticalStatCount() >= 2 {
		healthDecay := HealthDecayRate * deltaTime * multiplier
		bp.setHealth(bp.GetHealth() - int(healthDecay))
	}

	bp.lastUpdateTime = time.Now()
}

func (bp *BasePet) getStatusMessage() string {
	if !bp.IsAlive() {
		return "Dead..."
	}

	// Return appropriate message
	if bp.getCriticalStatCount() >= 3 {
		return "Critical condition! Needs immediate care!"
	} else if bp.getCriticalStatCount() >= 2 {
		return "Needs attention!"
	} else if bp.getCriticalStatCount() == 1 {
		return "Doing okay, but could use some care."
	} else {
		return "Alive and well!"
	}
}
func Interact() {

}

// Helper Function to reduce redundancy
func clampStat(value int) int {
	if value < MinStat {
		return MinStat
	} else if value > MaxStat {
		return MaxStat
	}
	return value
}
func (bp *BasePet) getCriticalStatCount() int {
	lowStatCount := 0
	if bp.GetHunger() < LowStatThreshold {
		lowStatCount++
	}
	if bp.GetCleanliness() < LowStatThreshold {
		lowStatCount++
	}
	if bp.happiness < LowStatThreshold {
		lowStatCount++
	}
	return lowStatCount
}

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

// Helper Function to reduce redundancy
func clampStat(value int) int {
	if value < MinStat {
		return MinStat
	} else if value > MaxStat {
		return MaxStat
	}
	return value
}

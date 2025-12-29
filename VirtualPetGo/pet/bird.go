package pet

import "fmt"

type Bird struct {
	BasePet
	featherColor string
	songCooldown float64
}

func NewBird(name string, featherColor string) *Bird {
	return &Bird{
		BasePet:      newBasePet(name),
		featherColor: featherColor,
		songCooldown: 0,
	}
}

func (b *Bird) MakeSound() string {
	return "Chirp chirp"
}

func (b *Bird) Interact() string {
	return b.GetName() + "is chirping happily! " + b.MakeSound()
}

func (b *Bird) Play() string {
	b.BasePet.Play()
	// Reduced hunger decrease for bird
	b.setHunger(b.GetHunger() + 2)
	return b.GetName() + " performs aerial acrobatics! Happiness increased."
}

func (b *Bird) Update(deltaTime float64) {
	b.BasePet.Update(deltaTime)

	if b.songCooldown > 0 {
		b.songCooldown -= deltaTime
		if b.songCooldown < 0 {
			b.songCooldown = 0
		}
	}
}

func (b *Bird) UseSpecialAbility() string {
	b.setHunger(b.GetHunger() + SongHungerBoost)
	b.setHappiness(b.GetHappiness() + SongHungerBoost)
	b.setHealth(b.GetHealth() + SongHealthBoost)
	b.setCleanliness(b.GetCleanliness() + SongCleanlinessBoost)

	//Set Cooldown
	b.songCooldown = SongCooldown

	return b.GetName() + " sings a beautiful song! All boosted!"
}

func (b *Bird) CanUseAbility() bool {
	return b.songCooldown <= 0
}
func (b *Bird) GetStatus() Status {
	// Determine ability status text
	abilityStatus := "(Ready!)"
	if b.songCooldown > 0 {
		abilityStatus = fmt.Sprintf("(Cooldown: %.0f seconds)", b.songCooldown)
	}

	return Status{
		Name:     b.GetName(),
		Type:     "Bird",
		Age:      b.GetAge(),
		AgeStage: b.BasePet.getAgeStage(),

		// Core stats
		Health:      b.GetHealth(),
		Hunger:      b.GetHunger(),
		Happiness:   b.GetHappiness(),
		Cleanliness: b.GetCleanliness(),

		// Special ability info
		SpecialAbility: "Song - Boosts all stats!",
		AbilityStatus:  abilityStatus,

		// Overall status
		StatusMessage: b.BasePet.getStatusMessage(),
		IsAlive:       b.IsAlive(),
	}
}

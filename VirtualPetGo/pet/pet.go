package pet

// ===== Age Stage Type and Constants =====

type AgeStage string

const (
	Baby    AgeStage = "Baby"
	Adult   AgeStage = "Adult"
	Elderly AgeStage = "Elderly"
)

type Pet interface {
	Feed() string
	Play() string
	Sleep() string
	Clean() string
	Interact() string
	MakeSound() string
	GetStatus() Status
	Update(deltaTime float64)
	IsAlive() bool
	GetAge() float64
}

type SpecialAbility interface {
	UseSpecialAbility() string
	CanUseAbility() bool // Check if ability is available
}

type Status struct {
	Name     string
	Type     string   // "Dog", "Cat", "Bird"
	Age      float64  // Age in minutes
	AgeStage AgeStage // Baby, Adult, or Elderly

	// Core stats (0-100)
	Health      int
	Hunger      int
	Happiness   int
	Cleanliness int

	// Special ability info
	SpecialAbility string // e.g., "Loyalty - Maintains happiness longer!"
	AbilityStatus  string // e.g., "(Active: true)", "(9 lives remaining)", "(Ready!)"

	// Overall status
	StatusMessage string // e.g., "Alive and well!", "Needs attention!", "Critical condition!"
	IsAlive       bool
}

const (
	MinStat = 0
	MaxStat = 100
)

// Age stage thresholds (in minutes)
const (
	BabyMaxAge  = 5.0
	AdultMaxAge = 15.0
	// Elderly starts at 15+ minutes
)

// Age-based decay multipliers
const (
	BabyDecayMultiplier    = 1.3
	AdultDecayMultiplier   = 1.0
	ElderlyDecayMultiplier = 0.7
)

// Stat decay rates (points per second, before age multiplier)
const (
	HungerDecayRate      = 2.0
	CleanlinessDecayRate = 1.0
	HappinessDecayRate   = 1.0 // Only when hunger or cleanliness < 30
	HealthDecayRate      = 0.5 // When multiple stats are critically low
)

// Critical stat thresholds
const (
	CriticalStatThreshold = 30 // Below this, happiness starts decaying
	LowStatThreshold      = 20 // Below this, health may be affected
)

// Action effects
const (
	// Feed
	FeedHungerIncrease    = 30
	FeedHappinessIncrease = 5

	// Play
	PlayHungerDecrease    = 10
	PlayHappinessIncrease = 15 // Base amount, can be overridden by pet type

	// Sleep
	SleepHealthIncrease = 20
	SleepHungerDecrease = 5

	// Clean
	CleanCleanlinessIncrease = 40
	CleanHappinessIncrease   = 10
)

// Special ability parameters
const (
	// LoyaltyDuration Dog - Loyalty
	LoyaltyDuration           = 60.0 // seconds
	LoyaltyHappinessReduction = 0.5  // 50% reduced happiness decay

	// MaxLives Cat - Nine Lives
	MaxLives = 9

	// SongCooldown Bird - Song
	SongCooldown         = 120.0 // seconds
	SongHungerBoost      = 20
	SongHappinessBoost   = 25
	SongHealthBoost      = 15
	SongCleanlinessBoost = 20
)

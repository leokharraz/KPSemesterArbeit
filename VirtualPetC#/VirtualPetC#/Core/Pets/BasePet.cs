using VirtualPetC_.Enums;

namespace VirtualPetC_.Core.Pets;

/// <summary>
/// Abstract base class representing a virtual pet.
/// </summary>
public abstract class BasePet
{
    // Constants - Stat boundaries
    private const int MinStat = 0;
    private const int MaxStat = 100;

    // Age stage thresholds (in minutes)
    private const double BabyMaxAge = 5.0;
    private const double AdultMaxAge = 15.0;

    // Age-based decay multipliers
    private const double BabyDecayMultiplier = 1.3;
    private const double AdultDecayMultiplier = 1.0;
    private const double ElderlyDecayMultiplier = 0.7;

    // Stat decay rates (points per second, before age multiplier)
    private const double HungerDecayRate = 2.0;
    private const double CleanlinessDecayRate = 1.5;
    private const double HappinessDecayRate = 1.0; // Only when hunger or cleanliness < 30
    private const double HealthDecayRate = 0.5; // When multiple stats are critically low

    // Critical stat thresholds
    private const int CriticalStatThreshold = 30; // Below this, happiness starts decaying
    private const int LowStatThreshold = 20; // Below this, health may be affected

    // Special ability parameters
    protected const double LoyaltyDuration = 60.0; // seconds
    protected const double LoyaltyHappinessReduction = 0.5; // 50% reduced happiness decay

    protected const int MaxLives = 9;

    protected const double SongCooldown = 120.0; // seconds
    protected const int SongHungerBoost = 20;
    protected const int SongHappinessBoost = 25;
    protected const int SongHealthBoost = 15;
    protected const int SongCleanlinessBoost = 20;

    // Illness mechanic constants
    protected const double IllnessCheckInterval = 5.0; // Check every 5 seconds
    private const double IllnessHealthDecayMultiplier = 2.5; // 2.5x health decay when ill
    private const int CleanlinessLowThreshold = 30;
    private const int CleanlinessCriticalThreshold = 10;
    private const double IllnessChanceLow = 0.075; // 7.5% at cleanliness < 30
    private const double IllnessChanceCritical = 0.175; // 17.5% at cleanliness < 10

    // Private fields - data hiding
    private int hunger;
    private int happiness;
    private int health;
    private int cleanliness;
    private readonly DateTime birthTime;
    private bool isIll;
    private double illnessCheckTimer;

    // Public properties with controlled access
    public string Name { get; }

    public int Hunger
    {
        get => hunger;
        protected set => hunger = ClampStat(value);
    }

    public int Happiness
    {
        get => happiness;
        protected set => happiness = ClampStat(value);
    }

    public int Health
    {
        get => health;
        protected set => health = ClampStat(value);
    }

    public int Cleanliness
    {
        get => cleanliness;
        protected set => cleanliness = ClampStat(value);
    }

    public int Age => (int)(DateTime.Now - birthTime).TotalMinutes;
    public AgeStage AgeStage => GetAgeStage();
    public bool IsAlive => health > 0;
    public bool IsIll => isIll;

    // Constructor
    protected BasePet(string name)
    {
        Name = name;
        hunger = 100;  // Start full (100 = full, 0 = starving)
        happiness = 100;
        health = 100;
        cleanliness = 100;  // Start clean (100 = clean, 0 = dirty)
        birthTime = DateTime.Now;
        isIll = false;
        illnessCheckTimer = IllnessCheckInterval;
    }

    /// <summary>
    /// Sets the illness state. Protected method for testing purposes.
    /// </summary>
    /// <param name="ill">Whether the pet should be ill</param>
    protected void SetIllnessForTesting(bool ill)
    {
        isIll = ill;
    }

    /// <summary>
    /// Makes the pet produce its characteristic sound.
    /// Each pet type implements this differently
    /// </summary>
    public abstract void MakeSound();


    // Returns a description of the pet's special ability.
    // Each pet type has a unique ability.
    public abstract string GetSpecialAbility();

    // Virtual method - can be overridden by derived classes
    /// <summary>
    /// Play with the pet. Can be overridden for pet-specific behavior.
    /// </summary>
    public virtual string Play()
    {
        Happiness += 20;
        Hunger -= 10;
        return $"{Name} is playing! Happiness increased, but got a bit hungry.";
    }

    // Concrete methods - shared by all pets
    public string Feed()
    {
        Hunger += 20;
        Happiness += 5;
        return $"{Name} enjoyed the meal! Hunger restored";
    }

    public string Sleep()
    {
        Health += 20;
        Hunger -= 5;
        return $"{Name} took a nice nap! Health Restored";
    }

    public string Clean()
    {
        Cleanliness += 40;
        Happiness += 10;

        if (isIll)
        {
            isIll = false;
            return $"{Name} is now clean and fresh! The illness has been cured!";
        }

        return $"{Name} is now clean and fresh! Feels much better.";
    }

    /// <summary>
    /// Updates pet stats based on time elapsed since last update.
    /// Demonstrates time-based decay system.
    /// </summary>
    /// <param name="deltaTime">Time elapsed in seconds since last update</param>
    public virtual void Update(double deltaTime)
    {
        if (deltaTime > 0)
        {
            double multiplier = GetDecayMultiplier();

            // Apply hunger decay
            double hungerDecay = HungerDecayRate * deltaTime * multiplier;
            Hunger -= (int)hungerDecay;

            // Apply cleanliness decay
            double cleanlinessDecay = CleanlinessDecayRate * deltaTime * multiplier;
            Cleanliness -= (int)cleanlinessDecay;

            // Apply happiness decay with modifier hook
            if (hunger < CriticalStatThreshold || cleanliness < CriticalStatThreshold)
            {
                double happinessDecay = HappinessDecayRate * deltaTime * multiplier;
                happinessDecay *= GetHappinessDecayModifier(); // Hook for subclasses
                Happiness -= (int)happinessDecay;
            }

            // Apply health decay when 2+ stats are critically low OR when ill
            if (GetCriticalStatCount() >= 2 || isIll)
            {
                double healthDecay = HealthDecayRate * deltaTime * multiplier;
                healthDecay *= GetHealthDecayModifier(); // Apply illness multiplier
                Health -= (int)healthDecay;
            }

            // Illness check mechanic (every 5 seconds)
            if (!isIll)
            {
                illnessCheckTimer -= deltaTime;

                if (illnessCheckTimer <= 0)
                {
                    illnessCheckTimer = IllnessCheckInterval; // Reset timer

                    // Determine illness chance based on cleanliness
                    double illnessChance = 0.0;
                    if (cleanliness < CleanlinessCriticalThreshold)
                        illnessChance = IllnessChanceCritical;
                    else if (cleanliness < CleanlinessLowThreshold)
                        illnessChance = IllnessChanceLow;

                    // Roll for illness
                    if (illnessChance > 0)
                    {
                        Random random = new Random();
                        if (random.NextDouble() < illnessChance)
                        {
                            isIll = true;
                            Console.WriteLine($"\n⚕️ {Name} has become ill! Health will decay faster until cleaned.\n");
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Displays current pet status with all stats.
    /// </summary>
    public void DisplayStatus()
    {
        Console.WriteLine($"\n=== {Name}'s Status ===");
        Console.WriteLine($"Type: {GetType().Name}");
        Console.WriteLine($"Age: {Age} minutes ({AgeStage})");
        Console.WriteLine($"Health: {Health}/100 {GetHealthBar(Health)}");
        Console.WriteLine($"Hunger: {Hunger}/100 {GetHealthBar(Hunger)}");
        Console.WriteLine($"Happiness: {Happiness}/100 {GetHealthBar(Happiness)}");
        Console.WriteLine($"Cleanliness: {Cleanliness}/100 {GetHealthBar(Cleanliness)}");
        Console.WriteLine($"Special Ability: {GetSpecialAbility()}");
        if (isIll)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"⚕️ ILLNESS: Pet is ILL! Clean to cure.");
            Console.ResetColor();
        }
        Console.WriteLine($"Status: {GetStatusMessage()}");
        Console.WriteLine("===================\n");
    }

    // Private helper methods
    private AgeStage GetAgeStage()
    {
        double age = (DateTime.Now - birthTime).TotalMinutes;
        if (age < BabyMaxAge) return AgeStage.Baby;
        if (age < AdultMaxAge) return AgeStage.Adult;
        return AgeStage.Elderly;
    }

    private double GetDecayMultiplier()
    {
        return AgeStage switch
        {
            AgeStage.Baby => BabyDecayMultiplier,
            AgeStage.Adult => AdultDecayMultiplier,
            AgeStage.Elderly => ElderlyDecayMultiplier,
            _ => AdultDecayMultiplier
        };
    }

    /// <summary>
    /// Hook for subclasses to modify happiness decay rate.
    /// Returns a multiplier for happiness decay (1.0 = normal, 0.5 = half decay).
    /// </summary>
    protected virtual double GetHappinessDecayModifier()
    {
        return 1.0; // Default: no modification
    }

    /// <summary>
    /// Hook for illness mechanic to modify health decay rate.
    /// Returns a multiplier for health decay (1.0 = normal, 2.5 = ill).
    /// </summary>
    protected virtual double GetHealthDecayModifier()
    {
        return isIll ? IllnessHealthDecayMultiplier : 1.0;
    }

    private int GetCriticalStatCount()
    {
        int lowStatCount = 0;
        if (hunger < LowStatThreshold) lowStatCount++;
        if (cleanliness < LowStatThreshold) lowStatCount++;
        if (happiness < LowStatThreshold) lowStatCount++;
        return lowStatCount;
    }

    public string GetStatusMessage()
    {
        if (!IsAlive)
        {
            return "Dead...";
        }

        // Show illness as high-priority status
        if (isIll)
        {
            return "ILL! Health decaying fast - needs cleaning!";
        }

        int criticalCount = GetCriticalStatCount();
        if (criticalCount >= 3)
        {
            return "Critical condition! Needs immediate care!";
        }
        else if (criticalCount >= 2)
        {
            return "Needs attention!";
        }
        else if (criticalCount == 1)
        {
            return "Doing okay, but could use some care.";
        }
        else
        {
            return "Alive and well!";
        }
    }

    private static int ClampStat(int value)
    {
        if (value < MinStat) return MinStat;
        if (value > MaxStat) return MaxStat;
        return value;
    }

    private string GetHealthBar(int value)
    {
        int bars = value / 10;
        return "[" + new string('█', bars) + new string('░', 10 - bars) + "]";
    }
}

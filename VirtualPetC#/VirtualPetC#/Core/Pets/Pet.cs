using VirtualPetC_.Enums;

namespace VirtualPetC_.Core.Pets;

/// <summary>
/// Abstract base class representing a virtual pet.
/// Demonstrates ABSTRACTION and ENCAPSULATION OOP principles.
/// </summary>
public abstract class Pet
{
    // ENCAPSULATION: Private fields - data hiding
    private int hunger;
    private int happiness;
    private int health;
    private int cleanliness;
    private readonly DateTime birthTime;
    private DateTime lastUpdateTime;

    // Accumulated fractional decay (to prevent loss from integer truncation)
    private double accumulatedHungerDecay;
    private double accumulatedHappinessDecay;
    private double accumulatedCleanlinessDecay;

    // ENCAPSULATION: Public properties with controlled access
    public string Name { get; }

    public int Hunger
    {
        get => hunger;
        protected set => hunger = Math.Clamp(value, 0, 100);
    }

    public int Happiness
    {
        get => happiness;
        protected set => happiness = Math.Clamp(value, 0, 100);
    }

    public int Health
    {
        get => health;
        protected set => health = Math.Clamp(value, 0, 100);
    }

    public int Cleanliness
    {
        get => cleanliness;
        protected set => cleanliness = Math.Clamp(value, 0, 100);
    }

    public int Age => (int)(DateTime.Now - birthTime).TotalMinutes;
    public AgeStage AgeStage => GetAgeStage();
    public bool IsAlive => health > 0;

    // Constructor
    protected Pet(string name)
    {
        Name = name;
        hunger = 100;  // Start full (100 = full, 0 = starving)
        happiness = 100;
        health = 100;
        cleanliness = 100;  // Start clean (100 = clean, 0 = dirty)
        birthTime = DateTime.Now;
        lastUpdateTime = DateTime.Now;
    }

    // ABSTRACTION: Abstract methods - must be implemented by derived classes
    /// <summary>
    /// Makes the pet produce its characteristic sound.
    /// Each pet type implements this differently (POLYMORPHISM).
    /// </summary>
    public abstract void MakeSound();

    /// <summary>
    /// Returns a description of the pet's special ability.
    /// Each pet type has a unique ability.
    /// </summary>
    public abstract string GetSpecialAbility();

    // Virtual method - can be overridden by derived classes
    /// <summary>
    /// Play with the pet. Can be overridden for pet-specific behavior.
    /// </summary>
    public virtual void Play()
    {
        Happiness += 20;
        Hunger -= 10;  // Playing makes pet hungry
        Console.WriteLine($"{Name} is playing! Happiness increased, but got a bit hungry.");
    }

    // Concrete methods - shared by all pets
    public void Feed()
    {
        Hunger += 30;  // Feeding restores hunger
        Happiness += 5;
        Console.WriteLine($"{Name} enjoyed the meal! Hunger restored.");
    }

    public void Sleep()
    {
        Health += 20;
        Hunger -= 5;  // Sleeping uses a bit of energy
        Console.WriteLine($"{Name} took a nice nap. Health restored!");
    }

    public void Clean()
    {
        Cleanliness += 40;
        Happiness += 10;
        Console.WriteLine($"{Name} is now clean and fresh! Feels much better!");
    }

    /// <summary>
    /// Updates pet stats based on time elapsed since last update.
    /// Demonstrates time-based decay system.
    /// </summary>
    public void Update()
    {
        TimeSpan elapsed = DateTime.Now - lastUpdateTime;
        double secondsElapsed = elapsed.TotalSeconds;

        if (secondsElapsed > 0)
        {
            // Apply stat decay based on age stage
            double decayMultiplier = GetDecayMultiplier();

            // Accumulate hunger decay (prevents loss from truncation)
            // Rate: 2 points per second
            accumulatedHungerDecay += secondsElapsed * 2.0 * decayMultiplier;
            if (accumulatedHungerDecay >= 1.0)
            {
                int hungerLoss = (int)accumulatedHungerDecay;
                Hunger -= hungerLoss;
                accumulatedHungerDecay -= hungerLoss; // Keep the fractional part
            }

            // Accumulate cleanliness decay
            // Rate: 1 point per second (gets dirty slower than hunger)
            accumulatedCleanlinessDecay += secondsElapsed * 1.0 * decayMultiplier;
            if (accumulatedCleanlinessDecay >= 1.0)
            {
                int cleanlinessLoss = (int)accumulatedCleanlinessDecay;
                Cleanliness -= cleanlinessLoss;
                accumulatedCleanlinessDecay -= cleanlinessLoss;
            }

            // Accumulate happiness decay
            // Rate: 2 points per second, but ONLY if cleanliness OR hunger is low
            if (cleanliness < 60 || hunger < 60)
            {
                accumulatedHappinessDecay += secondsElapsed * 2.0 * decayMultiplier;
            }

            // Apply accumulated happiness decay
            if (accumulatedHappinessDecay >= 1.0)
            {
                int happinessLoss = (int)accumulatedHappinessDecay;
                Happiness -= happinessLoss;
                accumulatedHappinessDecay -= happinessLoss; // Keep the fractional part
            }

            // Update health based on other stats
            UpdateHealth();

            lastUpdateTime = DateTime.Now;
        }
    }

    /// <summary>
    /// Displays current pet status with all stats.
    /// </summary>
    public void DisplayStatus()
    {
        Update();
        Console.WriteLine($"\n=== {Name}'s Status ===");
        Console.WriteLine($"Type: {GetType().Name}");
        Console.WriteLine($"Age: {Age} minutes ({AgeStage})");
        Console.WriteLine($"Health: {Health}/100 {GetHealthBar(Health)}");
        Console.WriteLine($"Hunger: {Hunger}/100 {GetHealthBar(Hunger)}");
        Console.WriteLine($"Happiness: {Happiness}/100 {GetHealthBar(Happiness)}");
        Console.WriteLine($"Cleanliness: {Cleanliness}/100 {GetHealthBar(Cleanliness)}");
        Console.WriteLine($"Special Ability: {GetSpecialAbility()}");
        Console.WriteLine($"Status: {(IsAlive ? "Alive and well!" : "Critical condition!")}");
        Console.WriteLine("===================\n");
    }

    // Private helper methods
    private AgeStage GetAgeStage()
    {
        int age = Age;
        if (age < 5) return AgeStage.Baby;
        if (age < 15) return AgeStage.Adult;
        return AgeStage.Elderly;
    }

    private double GetDecayMultiplier()
    {
        return AgeStage switch
        {
            AgeStage.Baby => 1.3,      // Babies need food more often
            AgeStage.Adult => 1.0,     // Adults normal rate
            AgeStage.Elderly => 0.7,   // Elderly slower decay
            _ => 1.0
        };
    }

    private void UpdateHealth()
    {
        // Poor conditions reduce health (low hunger, happiness, or cleanliness)
        if (hunger < 20 || happiness < 20 || cleanliness < 20)
        {
            Health -= 2;
        }
        // Good conditions slowly restore health (well-fed, happy, and clean)
        else if (hunger > 70 && happiness > 70 && cleanliness > 70)
        {
            Health += 1;
        }
    }

    private string GetHealthBar(int value)
    {
        int bars = value / 10;
        return "[" + new string('█', bars) + new string('░', 10 - bars) + "]";
    }
}

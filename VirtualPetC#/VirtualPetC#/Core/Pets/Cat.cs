namespace VirtualPetC_.Core.Pets;

/// <summary>
/// Cat class - demonstrates INHERITANCE and POLYMORPHISM.
/// Inherits from abstract Pet class and provides unique cat-specific behaviors.
/// Special Ability: Nine Lives - can regenerate health over time.
/// </summary>
public class Cat : Pet
{
    private int livesRemaining;

    // Constructor - calls base Pet constructor
    public Cat(string name) : base(name)
    {
        livesRemaining = 9;
    }

    public int LivesRemaining => livesRemaining;

    // POLYMORPHISM: Override abstract method with cat-specific implementation
    /// <summary>
    /// Cats meow when making sounds.
    /// </summary>
    public override void MakeSound()
    {
        Console.WriteLine($"{Name} says: Meow~ 🐱");
        Happiness += 3;
        Console.WriteLine($"{Name} purrs softly...");
    }

    // POLYMORPHISM: Override abstract method
    /// <summary>
    /// Returns description of cat's special ability.
    /// </summary>
    public override string GetSpecialAbility()
    {
        return $"Nine Lives - Can regenerate health! ({livesRemaining} lives remaining)";
    }

    // POLYMORPHISM: Override virtual method for unique cat behavior
    /// <summary>
    /// Cats are independent and play by themselves with less energy.
    /// </summary>
    public override void Play()
    {
        Update();
        Console.WriteLine($"{Name} is playing independently with a toy mouse! 🐭");
        Console.WriteLine($"{Name} pounces and swats at the toy gracefully!");

        // Cats are more independent - moderate happiness gain, less hunger
        Happiness += 18;  // Slightly less than base Pet (20)
        Hunger += 7;      // Less hunger than dogs (10) - cats conserve energy

        Console.WriteLine($"{Name} seems content with the solo playtime.");
    }

    /// <summary>
    /// Special cat ability: Use one of nine lives to regenerate health.
    /// Cats can recover from poor health conditions.
    /// </summary>
    public void UseNineLives()
    {
        Update();

        if (livesRemaining <= 0)
        {
            Console.WriteLine($"{Name} has no lives remaining to use!");
            return;
        }

        if (Health >= 80)
        {
            Console.WriteLine($"{Name} is already healthy! No need to use a life.");
            return;
        }

        livesRemaining--;
        Console.WriteLine($"{Name} uses the power of nine lives! ✨");
        Console.WriteLine($"Mystical cat energy flows through {Name}...");

        Health += 35;
        Happiness += 10;

        Console.WriteLine($"Health significantly restored! Lives remaining: {livesRemaining}/9");
    }

    /// <summary>
    /// Cats naturally regenerate a small amount of health over time.
    /// This is a passive ability that triggers during updates.
    /// </summary>
    public void PassiveRegeneration()
    {
        if (Health < 100 && livesRemaining > 0)
        {
            Health += 1;
        }
    }
}

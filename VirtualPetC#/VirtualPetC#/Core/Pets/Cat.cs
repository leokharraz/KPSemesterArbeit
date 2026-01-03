namespace VirtualPetC_.Core.Pets;

/// <summary>
/// Inherits from  Pet class and provides unique cat-specific behaviors.
/// Special Ability: Nine Lives - can regenerate health over time.
/// </summary>
public class Cat : Pet
{
    private int livesRemaining;

    // Constructor - calls base Pet constructor
    public Cat(string name) : base(name)
    {
        livesRemaining = MaxLives;
    }

    public int LivesRemaining => livesRemaining;

   
    public override void MakeSound()
    {
        Console.WriteLine($"{Name} says: Meow~ 🐱");
        Happiness += 3;
        Console.WriteLine($"{Name} purrs softly...");
    }

    
    // Returns description of cat's special ability.
    public override string GetSpecialAbility()
    {
        return $"Nine Lives - Auto-revive on death! ({livesRemaining} lives remaining)";
    }

    
    // Implementation of Play. Less Happiness gain  / lose less Hunger
    public override string Play()
    {
        // Cats are more independent - moderate happiness gain, less hunger loss
        Happiness += 18;  // Slightly less than base Pet (20)
        Hunger -= 7;      // Less hunger loss than dogs (10) - cats conserve energy

        return $"{Name} is playing independently with a toy mouse! 🐭\n{Name} pounces and swats at the toy gracefully!\n{Name} seems content with the solo playtime.";
    }

    /// <summary>
    /// Special cat ability: Manually use one of nine lives to restore health.
    /// </summary>
    public string UseSpecialAbility()
    {
        if (!CanUseAbility())
        {
            return $"{Name} has no lives remaining";
        }

        // Restore health to full
        Health = 100;
        livesRemaining--;
        return $"{Name} used Nine Lives! Health restored to 100. ({livesRemaining} lives remaining)";
    }

    
    // Check if the cat can use its special ability.
    public bool CanUseAbility()
    {
        return livesRemaining > 0;
    }

    
    // Update method with auto-revive on death.
    public override void Update(double deltaTime)
    {
        base.Update(deltaTime);

        // Auto-revive if dead and has lives remaining
        if (!IsAlive && CanUseAbility())
        {
            Health = 100;
            livesRemaining--;
            Console.WriteLine($"\n🐱 {Name} used a life! {livesRemaining} lives remaining.\n");
        }
    }
}

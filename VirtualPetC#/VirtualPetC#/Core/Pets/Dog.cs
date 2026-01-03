namespace VirtualPetC_.Core.Pets;

/// <summary>
/// Dog class 
/// Inherits from abstract Pet class and provides unique dog-specific behaviors.
/// Special Ability: Loyalty - maintains happiness longer than other pets.
/// </summary>
public class Dog : BasePet
{
    private bool loyaltyActive;
    private DateTime loyaltyEndTime;

    // Constructor - calls base Pet constructor
    public Dog(string name) : base(name)
    {
        loyaltyActive = false;
        loyaltyEndTime = DateTime.MinValue;
    }

    public bool IsLoyaltyActive => loyaltyActive;

    
    // Implementation of MakeSound Woof Woof
    public override void MakeSound()
    {
        Console.WriteLine($"{Name} says: Woof! Woof!");
        Happiness += 5;
        Console.WriteLine($"{Name} wags tail happily!");
    }

    
    
    // Returns description of dog's special ability.
    public override string GetSpecialAbility()
    {
        string status = loyaltyActive ? "(Active)" : "(Ready)";
        return $"Loyalty - Reduces happiness decay! {status}";
    }

    
    
    // Dogs get extra Happiness from Play
        public override string Play()
    {
        // Dogs get MORE happiness from playing due to their playful nature
        Happiness += 25;  // Base Pet gives 20, dogs get 25
        Hunger -= 10;  // Playing makes pet hungry

        return $"{Name} is playing fetch! \n{Name} brings the ball back with tail wagging!\n{Name} is very happy but getting hungry from all that running!";
    }

    
    // Override to reduce happiness decay when loyalty is active.
    protected override double GetHappinessDecayModifier()
    {
        return loyaltyActive ? LoyaltyHappinessReduction : 1.0;
    }

    
    // Special dog ability: Activate loyalty to reduce happiness decay.
    public string UseSpecialAbility()
    {
        loyaltyActive = true;
        loyaltyEndTime = DateTime.Now.AddSeconds(LoyaltyDuration);

        return $"{Name} is feeling extra loyal! Happiness will decay slower for the next 60 seconds.";
    }

    
    // Check if the dog can use its special ability.
    public bool CanUseAbility()
    {
        return !loyaltyActive; // Can only use when loyalty is not currently active
    }

    
    /// Update method to check if loyalty has expired.
    public override void Update(double deltaTime)
    {
        // Call base update (which uses our modifier)
        base.Update(deltaTime);

        // Check if loyalty has expired
        if (loyaltyActive && DateTime.Now >= loyaltyEndTime)
        {
            loyaltyActive = false;
        }
    }
}

namespace VirtualPetC_.Core.Pets;

/// <summary>
/// Dog class - demonstrates INHERITANCE and POLYMORPHISM.
/// Inherits from abstract Pet class and provides unique dog-specific behaviors.
/// Special Ability: Loyalty - maintains happiness longer than other pets.
/// </summary>
public class Dog : Pet
{
    // Constructor - calls base Pet constructor
    public Dog(string name) : base(name)
    {
    }

    // POLYMORPHISM: Override abstract method with dog-specific implementation
    /// <summary>
    /// Dogs bark when making sounds.
    /// </summary>
    public override void MakeSound()
    {
        Console.WriteLine($"{Name} says: Woof! Woof! 🐕");
        Happiness += 5;
        Console.WriteLine($"{Name} wags tail happily!");
    }

    // POLYMORPHISM: Override abstract method
    /// <summary>
    /// Returns description of dog's special ability.
    /// </summary>
    public override string GetSpecialAbility()
    {
        return "Loyalty - Your faithful companion maintains happiness longer!";
    }

    // POLYMORPHISM: Override virtual method for unique dog behavior
    /// <summary>
    /// Dogs love to play fetch and get extra happiness from playing.
    /// </summary>
    public override void Play()
    {
        Update();
        Console.WriteLine($"{Name} is playing fetch! 🎾");
        Console.WriteLine($"{Name} brings the ball back with tail wagging!");

        // Dogs get MORE happiness from playing due to their playful nature
        Happiness += 25;  // Base Pet gives 20, dogs get 25
        Hunger += 10;

        Console.WriteLine($"{Name} is very happy but getting hungry from all that running!");
    }

    /// <summary>
    /// Special dog ability: Use loyalty to boost happiness.
    /// Dogs can use their loyal nature to cheer themselves up.
    /// </summary>
    public void UseLoyaltyBoost()
    {
        Update();
        Console.WriteLine($"{Name} shows unconditional loyalty and devotion!");
        Console.WriteLine("Your loyal companion's spirits are lifted!");
        Happiness += 15;
        Health += 5;
        Console.WriteLine("Happiness and health increased through the power of loyalty!");
    }
}

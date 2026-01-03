namespace VirtualPetC_.Core.Pets;

/// <summary>
/// Bird class 
/// Special Ability: Song - singing boosts all stats slightly.
/// </summary>
public class Bird : BasePet
{
    private int songsPerformed;
    private double songCooldownRemaining;

    // Constructor - calls base Pet constructor
    public Bird(string name) : base(name)
    {
        songsPerformed = 0;
        songCooldownRemaining = 0;
    }

    public int SongsPerformed => songsPerformed;
    public double SongCooldownRemaining => songCooldownRemaining;

    
    
    public override void MakeSound()
    {
        Console.WriteLine($"{Name} says: Chirp chirp! 🐦");
        Happiness += 4;
        Console.WriteLine($"{Name} flutters wings happily!");
    }

    
    // Returns description of bird's special ability.
    
    public override string GetSpecialAbility()
    {
        string status = songCooldownRemaining > 0
            ? $"(Cooldown: {(int)songCooldownRemaining}s)"
            : "(Ready)";
        return $"Song - Boosts all stats! {status}";
    }

    public override string Play()
    {
        // Birds get balanced stats from flying - between dog and cat
        Happiness += 22;  // Between dog (25) and cat (18)
        Hunger -= 8;      // Between dog (10) and cat (7)

        return $"{Name} is flying around the room! \n{Name} does aerial acrobatics and loops in the air!\n{Name} lands gracefully, chirping with joy!";
    }

    
    // Special bird ability: Sing a beautiful song to boost all stats.
    public string UseSpecialAbility()
    {
        Hunger += SongHungerBoost;
        Happiness += SongHappinessBoost;
        Health += SongHealthBoost;
        Cleanliness += SongCleanlinessBoost;

        // Set cooldown
        songCooldownRemaining = SongCooldown;
        songsPerformed++;

        return $"{Name} sings a beautiful song! All stats boosted!";
    }

    
    // Check if the bird can use its special ability.
    public bool CanUseAbility()
    {
        return songCooldownRemaining <= 0;
    }

    
    // Update method to decrease song cooldown.
    public override void Update(double deltaTime)
    {
        base.Update(deltaTime);

        // Decrease cooldown
        if (songCooldownRemaining > 0)
        {
            songCooldownRemaining -= deltaTime;
            if (songCooldownRemaining < 0)
            {
                songCooldownRemaining = 0;
            }
        }
    }
}

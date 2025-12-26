namespace VirtualPetC_.Core.Pets;

/// <summary>
/// Bird class - demonstrates INHERITANCE and POLYMORPHISM.
/// Inherits from abstract Pet class and provides unique bird-specific behaviors.
/// Special Ability: Song - singing boosts all stats slightly.
/// </summary>
public class Bird : Pet
{
    private int songsPerformed;
    private DateTime lastSongTime;

    // Constructor - calls base Pet constructor
    public Bird(string name) : base(name)
    {
        songsPerformed = 0;
        lastSongTime = DateTime.MinValue;
    }

    public int SongsPerformed => songsPerformed;

    // POLYMORPHISM: Override abstract method with bird-specific implementation
    /// <summary>
    /// Birds chirp when making sounds.
    /// </summary>
    public override void MakeSound()
    {
        Console.WriteLine($"{Name} says: Chirp chirp! 🐦");
        Happiness += 4;
        Console.WriteLine($"{Name} flutters wings happily!");
    }

    // POLYMORPHISM: Override abstract method
    /// <summary>
    /// Returns description of bird's special ability.
    /// </summary>
    public override string GetSpecialAbility()
    {
        return $"Song - Beautiful melodies restore all stats! (Songs performed: {songsPerformed})";
    }

    // POLYMORPHISM: Override virtual method for unique bird behavior
    /// <summary>
    /// Birds love to fly around while playing.
    /// </summary>
    public override void Play()
    {
        Update();
        Console.WriteLine($"{Name} is flying around the room! 🦅");
        Console.WriteLine($"{Name} does aerial acrobatics and loops in the air!");

        // Birds get balanced stats from flying - between dog and cat
        Happiness += 22;  // Between dog (25) and cat (18)
        Hunger += 8;      // Between dog (10) and cat (7)

        Console.WriteLine($"{Name} lands gracefully, chirping with joy!");
    }

    /// <summary>
    /// Special bird ability: Sing a beautiful song to boost all stats.
    /// Birds can use their melodious voice to improve mood and health.
    /// </summary>
    public void SingSong()
    {
        Update();

        // Check if bird sang recently (cooldown of 2 minutes)
        TimeSpan timeSinceLastSong = DateTime.Now - lastSongTime;
        if (timeSinceLastSong.TotalMinutes < 2 && songsPerformed > 0)
        {
            Console.WriteLine($"{Name} needs to rest their voice!");
            Console.WriteLine($"Wait {2 - (int)timeSinceLastSong.TotalMinutes} more minute(s) before singing again.");
            return;
        }

        Console.WriteLine($"\n{Name} begins to sing a beautiful melody! 🎵");
        Console.WriteLine("♪ ♫ ♪ ♫ ♪ ♫");
        Console.WriteLine("The enchanting song fills the air...");

        // Song boosts all stats moderately
        Health += 15;
        Happiness += 20;
        Hunger -= 10;  // Song is calming, reduces perceived hunger

        songsPerformed++;
        lastSongTime = DateTime.Now;

        Console.WriteLine($"\nThe song was wonderful! All stats improved!");
        Console.WriteLine($"Total songs performed: {songsPerformed}");
    }

    /// <summary>
    /// Birds can preen their feathers, which is a self-care activity.
    /// </summary>
    public void Preen()
    {
        Update();
        Console.WriteLine($"{Name} is preening their beautiful feathers! ✨");
        Console.WriteLine($"{Name} carefully smooths each feather...");

        Health += 8;
        Happiness += 6;

        Console.WriteLine("Preening complete! Health and happiness increased slightly.");
    }
}

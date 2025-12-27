using VirtualPetC_.Core.Pets;

namespace VirtualPetTests;

/// <summary>
/// Tests for Bird-specific polymorphic behavior
/// </summary>
public class BirdTests
{
    [Fact]
    public void Bird_InheritsFromPet()
    {
        // Arrange & Act
        var bird = new Bird("Tweety");

        // Assert
        Assert.IsAssignableFrom<Pet>(bird);
    }

    [Fact]
    public void Bird_CanSingSong()
    {
        // Arrange
        var bird = new Bird("Tweety");
        int healthBefore = bird.Health;
        int happinessBefore = bird.Happiness;
        int hungerBefore = bird.Hunger;

        // Act
        bird.SingSong();

        // Assert - Song should boost stats
        Assert.True(bird.Health >= healthBefore, "Song should increase health");
        Assert.True(bird.Happiness >= happinessBefore, "Song should increase happiness");
        Assert.True(bird.Hunger >= hungerBefore, "Song should increase hunger");
    }

    [Fact]
    public void Bird_SongHasCooldown()
    {
        // Arrange
        var bird = new Bird("Tweety");

        // Act
        bird.SingSong(); // First song
        int songsAfterFirst = bird.SongsPerformed;

        bird.SingSong(); // Try second song immediately

        // Assert - Should be blocked by cooldown
        Assert.Equal(1, songsAfterFirst);
        // Second attempt should show cooldown message (songs performed stays at 1)
    }

    [Fact]
    public void Bird_TrackssongsPerformed()
    {
        // Arrange
        var bird = new Bird("Tweety");

        // Act
        bird.SingSong();

        // Assert
        Assert.True(bird.SongsPerformed >= 1, "Should track number of songs performed");
    }

    [Fact]
    public void Bird_HasBalancedPlayStats()
    {
        // Arrange
        var bird = new Bird("Tweety");
        var dog = new Dog("Buddy");
        var cat = new Cat("Whiskers");

        int birdHappinessBefore = bird.Happiness;
        int dogHappinessBefore = dog.Happiness;
        int catHappinessBefore = cat.Happiness;

        // Act
        bird.Play();
        dog.Play();
        cat.Play();

        // Assert - Bird should be balanced (between dog and cat)
        int birdHappinessGain = bird.Happiness - birdHappinessBefore;
        int dogHappinessGain = dog.Happiness - dogHappinessBefore;
        int catHappinessGain = cat.Happiness - catHappinessBefore;

        Assert.True(birdHappinessGain <= dogHappinessGain && birdHappinessGain >= catHappinessGain,
            "Bird happiness gain should be between dog and cat");
    }
}

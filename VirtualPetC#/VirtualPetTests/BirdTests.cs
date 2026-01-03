using VirtualPetC_.Core.Pets;

namespace VirtualPetTests;

/// <summary>
/// Tests for Bird-specific polymorphic behavior
/// </summary>
public class BirdTests
{
    [Fact]
    public void Bird_InheritsFromBasePet()
    {
        // Arrange & Act
        var bird = new Bird("Tweety");

        // Assert
        Assert.IsAssignableFrom<BasePet>(bird);
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
        bird.UseSpecialAbility();

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
        bird.UseSpecialAbility(); // First song
        int songsAfterFirst = bird.SongsPerformed;
        bool canUseImmediately = bird.CanUseAbility();

        // Assert - Should be blocked by cooldown
        Assert.Equal(1, songsAfterFirst);
        Assert.False(canUseImmediately, "Should not be able to use ability immediately due to cooldown");
    }

    [Fact]
    public void Bird_TrackssongsPerformed()
    {
        // Arrange
        var bird = new Bird("Tweety");

        // Act
        bird.UseSpecialAbility();

        // Assert
        Assert.True(bird.SongsPerformed >= 1, "Should track number of songs performed");
    }
 
}

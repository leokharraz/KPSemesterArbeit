using VirtualPetC_.Core.Pets;

namespace VirtualPetTests;

/// <summary>
/// Tests for Cat-specific polymorphic behavior
/// </summary>
public class CatTests
{
    [Fact]
    public void Cat_InheritsFromBasePet()
    {
        // Arrange & Act
        var cat = new Cat("Whiskers");

        // Assert
        Assert.IsAssignableFrom<BasePet>(cat);
    }

    [Fact]
    public void Cat_StartsWithNineLives()
    {
        // Arrange & Act
        var cat = new Cat("Whiskers");

        // Assert
        Assert.Equal(9, cat.LivesRemaining);
    }

    [Fact]
    public void Cat_CanUseNineLives()
    {
        // Arrange
        var cat = new Cat("Whiskers");

        // Damage the cat first by simulating time
        cat.Update(30); // Simulate 30 seconds

        int livesBefore = cat.LivesRemaining;

        // Act
        cat.UseSpecialAbility();

        // Assert - Should use a life and restore health
        Assert.Equal(100, cat.Health);
        Assert.Equal(livesBefore - 1, cat.LivesRemaining);
    }

    [Fact]
    public void Cat_PlayUsesLessHunger()
    {
        // Arrange
        var cat = new Cat("Whiskers");
        var dog = new Dog("Buddy");

        int catHungerBefore = cat.Hunger;
        int dogHungerBefore = dog.Hunger;

        // Act
        cat.Play();
        dog.Play();

        // Assert
        int catHungerLoss = catHungerBefore - cat.Hunger;
        int dogHungerLoss = dogHungerBefore - dog.Hunger;

        Assert.True(catHungerLoss <= dogHungerLoss,
            "Cat should use equal or less hunger when playing (more independent)");
    }
}

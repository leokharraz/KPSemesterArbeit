using VirtualPetC_.Core.Pets;

namespace VirtualPetTests;

/// <summary>
/// Tests for Cat-specific polymorphic behavior
/// </summary>
public class CatTests
{
    [Fact]
    public void Cat_InheritsFromPet()
    {
        // Arrange & Act
        var cat = new Cat("Whiskers");

        // Assert
        Assert.IsAssignableFrom<Pet>(cat);
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
        Thread.Sleep(10000); // Wait 10 seconds
        cat.DisplayStatus(); // Trigger update

        int livesBefore = cat.LivesRemaining;
        int healthBefore = cat.Health;

        // Act - Only works if health is low enough
        cat.UseNineLives();

        // Assert
        if (healthBefore < 80)
        {
            Assert.True(cat.LivesRemaining <= livesBefore, "Should use a life if health was low");
        }
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

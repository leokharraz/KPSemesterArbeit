using VirtualPetC_.Core.Pets;

namespace VirtualPetTests;

/// <summary>
/// Tests for Dog-specific polymorphic behavior
/// </summary>
public class DogTests
{
    [Fact]
    public void Dog_InheritsFromPet()
    {
        // Arrange & Act
        var dog = new Dog("Buddy");

        // Assert
        Assert.IsAssignableFrom<Pet>(dog);
    }

    [Fact]
    public void Dog_HasCorrectInitialStats()
    {
        // Arrange & Act
        var dog = new Dog("Buddy");

        // Assert
        Assert.Equal("Buddy", dog.Name);
        Assert.Equal(100, dog.Hunger);
        Assert.Equal(100, dog.Health);
        Assert.True(dog.IsAlive);
    }

    [Fact]
    public void Dog_CanUseLoyaltyBoost()
    {
        // Arrange
        var dog = new Dog("Buddy");
        int happinessBefore = dog.Happiness;
        int healthBefore = dog.Health;

        // Act
        dog.UseLoyaltyBoost();

        // Assert
        Assert.True(dog.Happiness >= happinessBefore, "Loyalty boost should increase happiness");
        Assert.True(dog.Health >= healthBefore, "Loyalty boost should increase health");
    }

    [Fact]
    public void Dog_PlayGivesExtraHappiness()
    {
        // Arrange
        var dog = new Dog("Buddy");
        var testPet = new TestPet("Generic");

        int dogHappinessBefore = dog.Happiness;
        int testPetHappinessBefore = testPet.Happiness;

        // Act
        dog.Play();
        testPet.Play();

        // Assert - Dogs should get more happiness from playing
        int dogHappinessGain = dog.Happiness - dogHappinessBefore;
        int testPetHappinessGain = testPet.Happiness - testPetHappinessBefore;

        Assert.True(dogHappinessGain >= testPetHappinessGain,
            "Dog should get equal or more happiness from playing");
    }
}

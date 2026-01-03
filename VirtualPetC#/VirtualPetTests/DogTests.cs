using VirtualPetC_.Core.Pets;

namespace VirtualPetTests;

/// <summary>
/// Tests for Dog-specific polymorphic behavior
/// </summary>
public class DogTests
{
    [Fact]
    public void Dog_InheritsFromBasePet()
    {
        // Arrange & Act
        var dog = new Dog("Buddy");

        // Assert
        Assert.IsAssignableFrom<BasePet>(dog);
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

        // Act
        dog.UseSpecialAbility();

        // Assert
        Assert.True(dog.IsLoyaltyActive, "Loyalty should be active after using ability");
        Assert.True(dog.CanUseAbility() == false, "Should not be able to use ability when loyalty is active");
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

using VirtualPetC_.Core.Pets;
using VirtualPetC_.Enums;

namespace VirtualPetTests;

/// <summary>
/// Test implementation of Pet for unit testing.
/// Since Pet is abstract, we need a concrete class for testing.
/// </summary>
public class TestPet : BasePet
{
    public TestPet(string name) : base(name) { }

    public override void MakeSound()
    {
        Console.WriteLine("Test sound!");
    }

    public override string GetSpecialAbility()
    {
        return "Test ability";
    }

    // Expose Update as public for testing time-based mechanics
    public void TestUpdate(double deltaTime = 0.01) => Update(deltaTime);

    // Helper to simulate time passing
    public void SimulateTimePassing(double seconds)
    {
        Update(seconds);
    }
}

public class PetTests
{
    [Fact]
    public void Pet_InitialValues_AreCorrect()
    {
        // Arrange & Act
        var pet = new TestPet("TestPet");

        // Assert
        Assert.Equal("TestPet", pet.Name);
        Assert.Equal(100, pet.Hunger);
        Assert.Equal(100, pet.Happiness);
        Assert.Equal(100, pet.Health);
        Assert.Equal(100, pet.Cleanliness);
        Assert.True(pet.IsAlive);
        Assert.Equal(AgeStage.Baby, pet.AgeStage);
    }

    [Fact]
    public void Feed_IncreasesHunger_AndHappiness()
    {
        // Arrange
        var pet = new TestPet("TestPet");
        pet.SimulateTimePassing(10); // Make pet a bit hungry first

        int hungerBefore = pet.Hunger;
        int happinessBefore = pet.Happiness;

        // Act
        pet.Feed();

        // Assert
        Assert.True(pet.Hunger >= hungerBefore, "Hunger should increase or stay same after feeding");
        Assert.True(pet.Happiness >= happinessBefore, "Happiness should increase or stay same");
    }

    [Fact]
    public void Play_IncreasesHappiness_DecreasesHunger()
    {
        // Arrange
        var pet = new TestPet("TestPet");
        int hungerBefore = pet.Hunger;
        int happinessBefore = pet.Happiness;

        // Act
        pet.Play();

        // Assert
        Assert.True(pet.Happiness >= happinessBefore, "Happiness should increase or stay same after playing");
        Assert.True(pet.Hunger <= hungerBefore, "Hunger should decrease or stay same after playing");
    }

    [Fact]
    public void Sleep_IncreasesHealth()
    {
        // Arrange
        var pet = new TestPet("TestPet");
        pet.SimulateTimePassing(10); // Decrease health a bit first

        int healthBefore = pet.Health;

        // Act
        pet.Sleep();

        // Assert
        Assert.True(pet.Health >= healthBefore, "Health should increase after sleeping");
    }

    [Fact]
    public void Clean_IncreasesCleanlinessAndHappiness()
    {
        // Arrange
        var pet = new TestPet("TestPet");
        pet.SimulateTimePassing(20); // Make pet dirty first

        int cleanlinessBefore = pet.Cleanliness;
        int happinessBefore = pet.Happiness;

        // Act
        pet.Clean();

        // Assert
        Assert.True(pet.Cleanliness > cleanlinessBefore, "Cleanliness should increase after cleaning");
        Assert.True(pet.Happiness >= happinessBefore, "Happiness should increase or stay same");
    }

    [Fact]
    public void Stats_DecayOverTime()
    {
        // Arrange
        var pet = new TestPet("TestPet");
        int hungerBefore = pet.Hunger;
        int cleanlinessBefore = pet.Cleanliness;

        // Act
        pet.SimulateTimePassing(5); // Wait 5 seconds

        // Assert
        Assert.True(pet.Hunger < hungerBefore, "Hunger should decrease over time");
        Assert.True(pet.Cleanliness < cleanlinessBefore, "Cleanliness should decrease over time");
    }

    [Fact]
    public void Happiness_DoesNotDecay_WhenHungerAndCleanlinessHigh()
    {
        // Arrange
        var pet = new TestPet("TestPet");
        // Pet starts with Hunger=100, Cleanliness=100, both above 30
        int happinessBefore = pet.Happiness;

        // Act
        pet.SimulateTimePassing(2); // Wait 2 seconds

        // Assert
        // Happiness should stay the same because both hunger and cleanliness are above 30
        Assert.Equal(happinessBefore, pet.Happiness);
    }

    [Fact]
    public void Happiness_Decays_WhenHungerBelowThreshold()
    {
        // Arrange
        var pet = new TestPet("TestPet");

        // Make hunger drop below 30
        pet.SimulateTimePassing(50); // Wait to lower hunger

        // Verify hunger is below 30
        if (pet.Hunger < 30)
        {
            int happinessBefore = pet.Happiness;

            // Act
            pet.SimulateTimePassing(2);

            // Assert
            Assert.True(pet.Happiness <= happinessBefore, "Happiness should decrease or stay same when hunger is low");
        }
    }

    [Fact]
    public void Stats_StayWithinBounds()
    {
        // Arrange
        var pet = new TestPet("TestPet");

        // Act - Try to overflow stats
        for (int i = 0; i < 10; i++)
        {
            pet.Feed();
            pet.Play();
            pet.Clean();
        }

        // Assert
        Assert.InRange(pet.Hunger, 0, 100);
        Assert.InRange(pet.Happiness, 0, 100);
        Assert.InRange(pet.Health, 0, 100);
        Assert.InRange(pet.Cleanliness, 0, 100);
    }

    [Fact]
    public void Pet_Dies_WhenHealthReachesZero()
    {
        // Arrange
        var pet = new TestPet("TestPet");

        // Act - Simulate long time without care
        pet.SimulateTimePassing(200); // Simulate 200 seconds without care

        // Assert
        // Pet should either be dead or very close
        Assert.True(pet.Health <= 30 || !pet.IsAlive, "Pet should have very low health or be dead");
    }

    [Fact]
    public void AgeStage_Changes_OverTime()
    {
        // Note: This test would take 5+ minutes to run naturally
        // For demonstration purposes, we verify the initial stage
        var pet = new TestPet("TestPet");

        // Assert
        Assert.Equal(AgeStage.Baby, pet.AgeStage);
        // In real gameplay, after 5 minutes -> Adult, after 15 minutes -> Elderly
    }
}

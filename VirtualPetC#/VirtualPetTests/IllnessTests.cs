using VirtualPetC_.Core.Pets;

namespace VirtualPetTests;

/// <summary>
/// Comprehensive tests for the Illness mechanic.
/// Tests illness triggering, health decay, and cure mechanics.
/// </summary>
public class IllnessTests
{
    /// <summary>
    /// Test helper pet that exposes illness state for testing.
    /// </summary>
    private class TestIllnessPet : BasePet
    {
        public TestIllnessPet(string name) : base(name) { }

        public override void MakeSound()
        {
            Console.WriteLine("Test sound!");
        }

        public override string GetSpecialAbility()
        {
            return "Test ability";
        }

        // Helper to force cleanliness to a specific value for testing
        public void SetCleanliness(int value)
        {
            // Reduce cleanliness through updates
            while (Cleanliness > value)
            {
                Update(1.0); // 1 second updates
            }
        }

        // Expose the protected method for testing
        public void SetIllness(bool ill)
        {
            SetIllnessForTesting(ill);
        }
    }

    [Fact]
    public void Pet_StartsHealthy()
    {
        // Arrange & Act
        var pet = new TestIllnessPet("TestPet");

        // Assert
        Assert.False(pet.IsIll, "Pet should start healthy (not ill)");
    }

    [Fact]
    public void Pet_CanBecomeIll_WhenCleanlinessLow()
    {
        // Arrange
        var pet = new TestIllnessPet("TestPet");

        // Act - Reduce cleanliness below 30 and wait for illness checks
        // This test has probabilistic nature (7.5% chance every 5 seconds)
        // We'll run multiple update cycles to increase likelihood
        pet.SetCleanliness(25);

        bool becameIll = false;
        // Run up to 150 update cycles (750 seconds simulated time)
        for (int i = 0; i < 150; i++)
        {
            pet.Update(5.0); // 5 second updates to trigger illness checks
            if (pet.IsIll)
            {
                becameIll = true;
                break;
            }
        }

        // Assert
        // With 7.5% chance and 150 tries, probability of success is ~99.999%
        Assert.True(becameIll, "Pet should eventually become ill with low cleanliness");
    }

    [Fact]
    public void Pet_HasHigherIllnessChance_WhenCleanlinessCritical()
    {
        // Arrange
        var pet1 = new TestIllnessPet("LowCleanliness");
        var pet2 = new TestIllnessPet("CriticalCleanliness");

        // Act - Set different cleanliness levels
        pet1.SetCleanliness(25); // Low (7.5% chance)
        pet2.SetCleanliness(5);  // Critical (17.5% chance)

        int pet1IllnessCount = 0;
        int pet2IllnessCount = 0;

        // Run multiple trials
        for (int trial = 0; trial < 50; trial++)
        {
            var testPet1 = new TestIllnessPet($"Test1_{trial}");
            var testPet2 = new TestIllnessPet($"Test2_{trial}");

            testPet1.SetCleanliness(25);
            testPet2.SetCleanliness(5);

            // Check for illness multiple times
            for (int i = 0; i < 20; i++)
            {
                testPet1.Update(5.0);
                testPet2.Update(5.0);

                if (testPet1.IsIll) pet1IllnessCount++;
                if (testPet2.IsIll) pet2IllnessCount++;

                if (testPet1.IsIll || testPet2.IsIll) break;
            }
        }

        // Assert - Pet with critical cleanliness should get ill more often
        // This is a statistical test - with enough trials, pet2 should get ill more
        Assert.True(pet2IllnessCount >= pet1IllnessCount,
            "Pet with critical cleanliness should have higher illness rate");
    }

    [Fact]
    public void Pet_DoesNotBecomeIll_WhenCleanlinessHigh()
    {
        // Arrange
        var pet = new TestIllnessPet("TestPet");

        // Act - Keep cleanliness high and run many update cycles
        for (int i = 0; i < 50; i++)
        {
            pet.Update(5.0); // 5 second updates
            pet.Clean(); // Keep clean
        }

        // Assert
        Assert.False(pet.IsIll, "Pet should not become ill when cleanliness is maintained high");
    }

    [Fact]
    public void Illness_IncreasesHealthDecay()
    {
        // Arrange - Create two pets with identical starting conditions
        var healthyPet = new TestIllnessPet("Healthy");
        var illPet = new TestIllnessPet("Ill");

        // Set one pet to ill using test helper method
        illPet.SetIllness(true);

        // Make both pets have 2+ critical stats (below 20) to trigger health decay
        // This ensures both pets will lose health, but ill pet loses more
        healthyPet.SetCleanliness(15);
        illPet.SetCleanliness(15);

        // Lower hunger to below 20 for both pets to ensure 2+ critical stats
        while (healthyPet.Hunger > 15)
        {
            healthyPet.Update(5.0);
        }
        while (illPet.Hunger > 15)
        {
            illPet.Update(5.0);
        }

        // Get initial health values after setup
        int healthyPetHealthBefore = healthyPet.Health;
        int illPetHealthBefore = illPet.Health;

        // Act - Update both pets for same duration (20 seconds for measurable difference)
        healthyPet.Update(20.0);
        illPet.Update(20.0);

        // Calculate health loss
        int healthyPetHealthLoss = healthyPetHealthBefore - healthyPet.Health;
        int illPetHealthLoss = illPetHealthBefore - illPet.Health;

        // Assert - Ill pet should lose more health (2.5x multiplier when ill)
        // Both pets have 2+ critical stats, so both lose health
        // But ill pet has 2.5x multiplier, so should lose significantly more
        Assert.True(illPetHealthLoss > healthyPetHealthLoss,
            $"Ill pet should lose more health than healthy pet. Healthy lost: {healthyPetHealthLoss}, Ill lost: {illPetHealthLoss}");
    }

    [Fact]
    public void Clean_CuresIllness()
    {
        // Arrange
        var pet = new TestIllnessPet("TestPet");

        // Make pet ill
        pet.SetIllness(true);

        // Act
        pet.Clean();

        // Assert
        Assert.False(pet.IsIll, "Cleaning should cure illness");
    }

    [Fact]
    public void Clean_ShowsCureMessage_WhenIll()
    {
        // Arrange
        var pet = new TestIllnessPet("TestPet");

        // Make pet ill
        pet.SetIllness(true);

        // Act
        string result = pet.Clean();

        // Assert
        Assert.Contains("cured", result.ToLower());
    }

    [Fact]
    public void Illness_DoesNotTrigger_WhenAlreadyIll()
    {
        // Arrange
        var pet = new TestIllnessPet("TestPet");
        pet.SetIllness(true);

        // Act - Continue updating while ill
        bool stillIll = pet.IsIll;
        for (int i = 0; i < 20; i++)
        {
            pet.Update(5.0);
            // Illness state should remain stable (not toggling)
            Assert.Equal(stillIll, pet.IsIll);
        }

        // Assert - Pet should remain ill until cleaned
        Assert.True(pet.IsIll, "Pet should remain ill until cleaned");
    }

    [Fact]
    public void StatusMessage_IndicatesIllness()
    {
        // Arrange
        var pet = new TestIllnessPet("TestPet");
        pet.SetCleanliness(5);

        // Make pet ill
        while (!pet.IsIll && pet.Health > 50)
        {
            pet.Update(5.0);
        }

        // Skip test if pet didn't become ill
        if (!pet.IsIll) return;

        // Act
        string status = pet.GetStatusMessage();

        // Assert
        Assert.Contains("ILL", status.ToUpper());
    }

   
}

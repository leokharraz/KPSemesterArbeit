namespace VirtualPetC_.Enums;

/// <summary>
/// Represents the different life stages of a pet
/// </summary>
public enum AgeStage
{
    /// <summary>
    /// Baby stage: 0-5 minutes old, higher hunger rate
    /// </summary>
    Baby,

    /// <summary>
    /// Adult stage: 5-15 minutes old, peak performance
    /// </summary>
    Adult,

    /// <summary>
    /// Elderly stage: 15+ minutes old, slower decay but lower max health
    /// </summary>
    Elderly
}

/// <summary>
/// Represents the different types of pets available
/// </summary>
public enum PetType
{
    Dog,
    Cat,
    Bird
}

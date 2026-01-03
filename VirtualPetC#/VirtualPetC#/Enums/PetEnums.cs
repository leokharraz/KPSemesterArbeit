namespace VirtualPetC_.Enums;

/// <summary>
/// Represents the different life stages of a pet
/// </summary>
public enum AgeStage
{
    
    // Baby stage: 0-5 minutes old, higher hunger rate
    Baby,

    
    // Adult stage: 5-15 minutes old, peak performance
    Adult,

    
    // Elderly stage: 15+ minutes old, slower decay but lower max health
    Elderly
}


// Represents the different types of pets available
public enum PetType
{
    Dog,
    Cat,
    Bird
}

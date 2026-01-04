# Virtual Pet Simulator

A console-based virtual pet simulator built in C# that demonstrates core Object-Oriented Programming principles. This project showcases abstraction, inheritance, polymorphism, encapsulation, and dependency inversion through an interactive pet care game.

## Overview

Virtual Pet Simulator allows users to adopt and care for a virtual pet (Dog, Cat, or Bird). Players must maintain their pet's health, hunger, happiness, and cleanliness through various interactions. Each pet type has unique behaviors and special abilities, demonstrating polymorphic design.

## Key Features

### Pet Types & Special Abilities

- **Dog**
  - Special Ability: Loyalty - maintains happiness longer
  - Behavior: Extra happiness from playing fetch
  - Sound: Woof! Woof!

- **Cat**
  - Special Ability: Nine Lives - regenerate health (9 uses)
  - Behavior: Independent play with lower energy consumption
  - Sound: Meow~

- **Bird**
  - Special Ability: Song - singing boosts all stats
  - Behavior: Aerial acrobatics while playing
  - Sound: Chirp chirp!

### Game Mechanics

- **Time-Based Decay System**: Stats decrease over time based on age stage
  - Baby (0-5 minutes): 1.3x decay rate - needs more frequent care
  - Adult (5-15 minutes): 1.0x decay rate - normal maintenance
  - Elderly (15+ minutes): 0.7x decay rate - slower stat decline

- **Four Core Stats**:
  - **Health** (0-100): Affected by other stats; pet dies if health reaches 0
  - **Hunger** (0-100): Decays at 2 points/second; restored by feeding
  - **Happiness** (0-100): Decays when hunger or cleanliness is low
  - **Cleanliness** (0-100): Decays at 1 point/second; restored by cleaning

- **Available Actions**:
  - Feed: Restores hunger (+30) and happiness (+5)
  - Play: Increases happiness, decreases hunger (pet-specific behavior)
  - Sleep: Restores health (+20), slightly decreases hunger (-5)
  - Clean: Increases cleanliness (+40) and happiness (+10)
  - Interact: Pet makes its characteristic sound
  - Use Special Ability: Unique ability per pet type



```
VirtualPetC#/
├── Core/
│   ├── Pets/
│   │   ├── Pet.cs           # Abstract base class
│   │   ├── Dog.cs           # Dog implementation
│   │   ├── Cat.cs           # Cat implementation
│   │   └── Bird.cs          # Bird implementation
│   └── GameManager.cs       # Game orchestration
├── UI/
│   ├── IUserInterface.cs    # UI abstraction
│   └── MenuSystem.cs        # Console UI implementation
├── Enums/
│   └── PetEnums.cs          # AgeStage and PetType enums
└── Program.cs               # Entry point

VirtualPetTests/
├── PetTests.cs              # Base Pet class tests
├── DogTests.cs              # Dog-specific tests
├── CatTests.cs              # Cat-specific tests
└── BirdTests.cs             # Bird-specific tests
```

## Technical Details

- **Framework**: .NET 8.0
- **Language**: C# with nullable reference types enabled
- **Testing**: Unit tests using test framework
- **Architecture**: Clean separation of concerns with dependency injection

## How to Run

1. Clone the repository
2. Navigate to the project directory
3. Build the solution:
   ```bash
   dotnet build
   ```
4. Run the application:
   ```bash
   dotnet run --project VirtualPetC#
   ```

## How to Test

Run the unit tests with:
```bash
dotnet test
```

## Game Tips

- Monitor all stats regularly - neglecting any stat can lead to health decline
- Baby pets require more frequent care due to higher decay rates
- Each pet type has strengths: Dogs get more happiness from play, Cats have health regeneration, Birds can boost all stats with songs
- Use special abilities strategically when stats are low
- Keep cleanliness and hunger above 60 to prevent happiness decay

## Educational Purpose

This project was created as a semester assignment to demonstrate mastery of object-oriented programming concepts in C#. It serves as a practical example of how OOP principles can be applied to create a maintainable and extensible application.

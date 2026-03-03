# Flappy Bird — C# WinForms

Weekly assignment for **YMT-218 Object-Oriented Programming**.

![Gameplay](screenshot.png)

## About

A 2D Flappy Bird clone written in C# using WinForms and GDI+ for rendering. The bird flaps with spacebar or mouse click, pipes scroll from right to left, and each gap cleared earns a point. The game features day/night backgrounds picked at random, sprite-based score display, and original sound effects.

## Features

- 60 FPS game loop with double-buffered rendering
- 2x pixel scaling for crisp retro visuals
- Wing animation (3-frame cycle), rotation based on velocity
- Randomized pipe gaps and day/night backgrounds
- Collision detection with reduced hitbox for fair gameplay
- High score tracking across rounds
- Sound effects: flap, score, hit, die, swoosh

## Project Structure

```
FlappyBird/
├── Program.cs              → Entry point
├── GameForm.cs             → Window, input events, rendering
├── GameEngine.cs           → Game loop logic, scoring, collisions
├── Entity.cs               → Abstract base (implements IRenderable + IUpdatable)
├── Bird.cs                 → Player entity — physics, animation
├── PipePair.cs             → Top + bottom pipe as a single unit
├── ScrollingGround.cs      → Looping ground tile
├── AudioManager.cs         → Sound loading and playback
├── IRenderable.cs          → Draw contract
├── IUpdatable.cs           → Update contract
└── Assets/
    ├── sprites/            → Bird, pipe, background, digit sprites
    └── audio/              → wav sound effects
```

## Design Decisions

The game is split into three layers. **GameForm** only handles the window, user input, and drawing. It delegates all logic to **GameEngine**, which manages state transitions, pipe spawning, and collision checks. The actual game objects (**Bird**, **PipePair**, **ScrollingGround**) each inherit from **Entity** and know how to update and draw themselves.

**AudioManager** doesn't inherit from Entity because it has no position or visual — it sits inside GameEngine as a regular field. This is a deliberate choice: sound management is a "has-a" relationship, not an "is-a".

Bird keeps its velocity and rotation private. Outside code can only call `Flap()` — everything else (gravity, terminal velocity, ceiling clamp, ground detection) is handled internally.

## Built With

- C# / .NET 8
- WinForms + GDI+ (System.Drawing)
- System.Media for audio

## How to Run

Requires [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) on Windows.

```
git clone https://github.com/LordAlis/FlappyBird.git
cd FlappyBird
dotnet run
```

Controls: **Space** / **Up** / **Mouse click** to flap. Click to restart after game over.

## Credits

Sprites and audio: [samuelcust/flappy-bird-assets](https://github.com/samuelcust/flappy-bird-assets) (MIT)

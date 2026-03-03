# Flappy Bird

A Flappy Bird clone built with C# and WinForms, developed as a weekly assignment for the **YMT-218 Object-Oriented Programming** course.

![Gameplay Screenshot](screenshot.png)

---

## Technologies

| Technology             | Purpose                          |
| ---------------------- | -------------------------------- |
| C# (.NET 8)            | Core programming language        |
| WinForms               | Game window and event handling   |
| GDI+ (System.Drawing)  | 2D rendering and sprite drawing  |
| System.Media           | Sound effect playback            |
| Git                    | Version control                  |

---

## OOP Structure

The project follows a layered OOP architecture where every game object inherits from a shared abstract base, and non-visual systems are composed rather than inherited.

### Class Hierarchy

```
IRenderable       IUpdatable
    └──────┬──────────┘
        Entity  (abstract)
          ├── Bird
          ├── PipePair
          └── ScrollingGround

AudioManager      (standalone — composed inside GameEngine)
GameEngine        (standalone — orchestrates game logic)
GameState         (enum — Ready, Playing, GameOver)
GameForm          (window, rendering, input)
```

### OOP Principles Used

**Abstraction & Inheritance**
`Entity` is an abstract base class holding common properties (`X`, `Y`, `Width`, `Height`) and defining abstract `Update()` and `Draw()` methods. `Bird`, `PipePair`, and `ScrollingGround` each extend it with their own behavior.

**Encapsulation**
Internal state is kept private. For instance, `Bird._velocity` and `Bird._rotation` cannot be accessed from outside — the only public action is `Flap()`. Ground collision and ceiling clamping are also handled internally by the Bird class itself (`HitsGround()`, `ClampToTop()`).

**Polymorphism**
`GameForm` iterates over a collection of pipes and calls `Draw()` on each one. Every `Entity` subclass responds to the same interface but renders itself differently.

**Interfaces**
`IRenderable` and `IUpdatable` define independent contracts. An object can be drawable without needing update logic, or vice versa — keeping things flexible.

**Composition over Inheritance**
`AudioManager` is not an `Entity` — it has no position or sprite. Instead, it lives inside `GameEngine` as a field (has-a relationship), handling all sound effects without being part of the visual hierarchy.

**Separation of Concerns**
Each class has a single responsibility:

- `Bird` — gravity, flapping, rotation, animation
- `PipePair` — manages top and bottom pipes as a single unit
- `ScrollingGround` — infinite horizontal scrolling
- `AudioManager` — loads and plays sound effects
- `GameEngine` — state management, scoring, collision detection, pipe spawning
- `GameForm` — window setup, user input, rendering loop

---

## How to Play

- Press **Space** / **Up Arrow** or **click** to flap
- Navigate through the gaps between pipes
- Each pipe passed = +1 point
- Avoid hitting pipes and the ground

---

## Running the Project

Make sure you have [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) installed.

```bash
git clone https://github.com/LordAlis/FlappyBird.git
cd FlappyBird
dotnet run
```

---

## Assets

Sprites and audio from [samuelcust/flappy-bird-assets](https://github.com/samuelcust/flappy-bird-assets) (MIT License).

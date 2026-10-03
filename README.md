# Retro Space Invaders — A TUI Arcade Game in C#

A responsive, cross-platform **Space Invaders clone** built entirely inside the system terminal using ASCII/ANSI character graphics. 

This project was built as an architectural exercise to demonstrate how modern enterprise design patterns—such as **Vertical Slice Architecture**, **Dependency Injection (DI)**, and **SOLID principles**—can be applied cleanly to a lightweight, real-time interactive game loop without falling into the trap of over-engineering.

---

## 🚀 Key Features
*   **Rich Text User Interface (TUI):** Powered by `Spectre.Console` utilizing 24-bit TrueColor rendering, asynchronous non-blocking input, and a high-performance double-buffered canvas to guarantee zero screen flickering at a stable **30 FPS**.
*   **Fully Responsive Alien Grid:** The total number, spacing, and distribution of the alien invaders are parameterized dynamically at startup based on the current dimensions (`Console.WindowWidth`/`Height`) of the user's terminal window.
*   **Live Window Resizing:** Handles runtime terminal window stretching or shrinking gracefully by dynamically scaling coordinate bounds and entity positioning mid-game without application crashes.

---

## 🏗️ Architectural Blueprint: Vertical Slice Architecture

Unlike traditional N-Tier or Clean/Hexagonal architectures that split code horizontally by technical layers (e.g., UI, Core Business Logic, Data Access), this solution is structured around **Vertical Slices**. 

Every feature is treated as an independent vertical column that encapsulates everything it needs to perform its behavior (Commands, Domain Extensions, Renderers).

### Folder Structure
```text
TUInvaders/
│
├── Assets/                         # Sound effects (.wav files)
├── Common/                         # Cross-cutting concerns / Infrastructure
│   ├── IGameEngine.cs              # Core deterministic Game Loop coordinator
│   └── InputProvider.cs            # Asynchronous keyboard input handler
│
├── Domain/                         # Global Anemic Shared State
│   └── GameState.cs                # Global parameters (Screen size, Score, Entities)
│
├── Features/                       # THE VERTICAL SLICES
│   ├── Initialization/             # Setup and adaptive responsive alien grid calculation
│   ├── PlayerMovement/             # Tank inputs, updates, and side boundary clipping
│   ├── AlienAI/                    # Flotta behavior, rhythmic movements, and downward march
│   ├── Shooting/                   # Fire logic, collision detection, and explosion triggers
│   ├── GameOverDisplay/            # TUI ASCII Art Win/Loss presentation screens
│   └── WindowResizer/              # Runtime dynamic window scaling coordinator
│
├── Program.cs                      # Application Entry Point & Native DI Bootstrapper
└── TUInvaders.csproj
```

**Why this approach?**  
High cohesion, low coupling. If we want to modify or refactor how lasers operate, we only touch the `Features/Shooting/` folder. Slices interact solely via the thin, shared global `GameState` singleton, vastly reducing cognitive load.

---

## 🧩 Deep Dive: Respecting SOLID Principles

### 1. Single Responsibility Principle (SRP)
Every command class within a vertical slice is responsible for **exactly one single behavior**. 
*   `MovePlayerCommand` is solely responsible for parsing directional inputs and modifying the player's X coordinate.
*   `DrawFrameCommand` only cares about projecting the internal entities onto the Spectre `Canvas`.
*   The `GameEngine` does not know how an alien moves or what color a laser is; it acts exclusively as a traffic controller orchestrating the game frames and sleep limits.

### 2. Open/Closed Principle (OCP)
The application architecture is highly extensible without requiring modifications to existing code. For instance, when adding the feature *"Aliens shooting lasers downwards"*, we did not modify the player movement or core engine code. We simply introduced `AlienFireLaserCommand` inside the `Shooting` slice and registered it into the DI Container.

### 3. Liskov Substitution Principle (LSP)
Abstractions are kept concise and tailored. Slices expose atomized command interfaces (e.g., `IMovePlayerCommand`, `IUpdateLasersCommand`). Any implementation of these interfaces acts exactly as the caller (`GameEngine`) expects, without throwing `NotImplementedException` or breaking the predictable game-loop sequence.

### 4. Interface Segregation Principle (ISP)
Instead of forcing a monolithic `IGameService` interface containing `Update()`, `Render()`, and `Input()` methods onto every subsystem, interfaces are decoupled into atomic, behavior-specific structures generally exposing a single `Execute()` method. Components only depend on the narrow contract they actually execute.

### 5. Dependency Inversion Principle (DIP)
High-level modules do not depend on low-level modules; both depend on abstractions. Crucially, the system decouples third-party presentation frameworks. By injecting `IAnsiConsole` (Spectre) into our commands via constructor injection, the game rules are shielded against infrastructural changes, making unit testing and potential framework replacement straightforward.

---

## 🛠️ Prerequisites & Compilation

This application is built using modern **.NET** and targets cross-platform terminal environments.

### 🏃‍♂️ Running locally
```bash
# Clone the repository
git clone https://github.com
cd tui-invaders

# Run the project
dotnet run --project TUInvaders
```

### 📦 Self-Contained Native Publishing
To distribute this game as a single executable without requiring the end-user to install the .NET Runtime, use the following native compilation targets:

**Windows (64-bit):**
```bash
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:PublishTrimmed=false
```
**Linux (64-bit):**
```bash
dotnet publish -c Release -r linux-x64 --self-contained true -p:PublishSingleFile=true -p:PublishTrimmed=false
```
**macOS (Apple Silicon M1/M2/M3):**
```bash
dotnet publish -c Release -r osx-arm64 --self-contained true -p:PublishSingleFile=true -p:PublishTrimmed=false
```

# SkyHigh 🎮
### Master your weapon. Hit the target. Don't look down.

---

## 🎯 Game Concept

Somewhere above the clouds, the world kept going.

Buildings stretch into the sky. Platforms connect rooftops. Guards 
patrol the walkways between them. Nobody knows how it got here or 
why it's floating. Nobody asks. They just keep moving.

You are a player dropped into this sky world with one goal: get 
through. The catch is that the path forward is locked behind 
bullseye targets hidden across the environment. Hit them in the 
right order and the world opens up — platforms extend, doors slide 
open, timers start counting down.

Your weapon changes depending on the level. Right now it is a 
wooden toy dagger. Later it will be something else. The weapon 
does not matter as much as the skill. Whatever you are equipped 
with, learn how it moves, learn how it travels, and put it on 
the target.

SkyHigh is a first-person platformer-puzzle game built around one 
principle: accuracy wins. Movement gets you there. Accuracy gets 
you through.

---

## 🕹️ Controls

| Input | Action |
|---|---|
| WASD | Move |
| Mouse | Look |
| Space | Jump |
| Left Click | Throw weapon |
| Escape | Pause |
| R (win screen) | Restart |
| M (win screen) | Main Menu |

---

## 🌤️ The World

SkyHigh takes place entirely above the clouds.

The level is built across rooftops and floating platforms connected 
by narrow walkways and guarded by patrol enemies. The terrain below 
is sky — fall off and you respawn at the last checkpoint. Nothing 
down there will catch you.

The environment is urban and vertical. Buildings from the Kenney 
prototype kit fill the skyline. Bullseye targets are mounted on 
walls, tucked behind props, placed above doorways, and hidden in 
the spaces players instinctively overlook. Finding them is half 
the challenge. Hitting them accurately is the other half.

The story is still forming. Something about why you are up here. 
Why the targets are placed where they are. Why the guards patrol 
but never stop. The world has answers. Future levels will surface 
them.

---

## 📐 SOLID Principles

Every script in this project was written with SOLID in mind:

**S — Single Responsibility**
Each script has exactly one job.
- `PuzzleButton` only handles button activation logic
- `Door` only handles door sliding logic
- `MovingPlatform` only handles platform movement
- `CheckpointManager` only handles player respawning
- `AudioManager` only handles sound playback
- `EnemyPatrol` only handles enemy movement and player detection

**O — Open/Closed**
The puzzle system is open for extension, closed for modification.
Adding a new puzzle object requires zero changes to existing scripts
— it simply implements `IActivatable` and `IButtonReaction`.
Adding a new weapon type requires zero changes to the core shooting
system — it simply implements `ICommand`.

**L — Liskov Substitution**
Every `IButtonReaction` implementor (Door, MovingPlatform, FuseTimer)
can be swapped for another without breaking the system.
Every `IResettable` implementor (PuzzleButton, Door, MovingPlatform,
FuseTimer, EnemyPatrol) can be reset without the CheckpointManager
knowing what type of object it is resetting.

**I — Interface Segregation**
Three small focused interfaces instead of one large one:
- `IActivatable` — for anything a weapon can hit
- `IButtonReaction` — for anything that reacts to a target being hit
- `IResettable` — for anything that resets on player death

**D — Dependency Inversion**
The weapon system never references `Door`, `MovingPlatform`, or any
concrete type. It only calls `IActivatable.Activate()` — it depends
on the abstraction, never the implementation. The weapon never knows
what it hit.

---

## 🧩 Design Patterns

This project consciously applies 4 design patterns, each mapped to
a specific system:

### 1. Observer Pattern
**Where:** `PuzzleButton.cs` using `UnityEvent`
**Why:** The button (Subject) notifies subscribers (Observers) when
activated. Door, MovingPlatform, and FuseTimer all subscribe via the
Inspector. The button never knows what it is notifying — zero coupling
between cause and effect. Adding new reactions requires zero changes
to PuzzleButton.

### 2. Command Pattern
**Where:** `ShootCommand.cs` implementing `ICommand`
**Why:** Each action is encapsulated as an object with an `Execute()`
method. The weapon system creates the command, the command handles
instantiation and launching. This decouples the weapon from the
projectile — the gun never touches the dagger directly.

### 3. Strategy Pattern
**Where:** `IButtonReaction` implemented by `Door`, `MovingPlatform`,
`FuseTimer`
**Why:** Each object defines its own reaction behavior independently.
PuzzleButton fires one event — each subscriber responds differently
based on its own Strategy. Swapping behaviors requires no changes
to the button.

### 4. Factory Pattern
**Where:** `ToyGun.cs` creating projectiles via `ShootCommand`
**Why:** Projectile instantiation is handled through ShootCommand,
not directly by the weapon script. The weapon depends only on
ICommand — it never calls Instantiate directly, keeping creation
logic separate from firing logic.

---
## 🔧 Known Improvements

**Unity Timeline / Playable Director**
The win cutscene currently uses coroutines for sequencing
(WaitForSeconds, fade loops). A future iteration will replace this
with Unity's Timeline and Playable Director package. This would
eliminate the manual yield chains, make the cutscene easier to
extend, and open the door for more complex cinematic sequences in
future levels. This was a deliberate scope decision — replacing a
working system 12 hours before a deadline introduces more risk than
value. It is documented here as the next technical improvement for
version 2.

**AI Pathfinding**
Current enemies use a simple back-and-forth patrol script. A future
iteration will replace this with Unity NavMesh pathfinding and a
behavioral state machine — enemies that actively chase the player,
react to sound, and work together to create real pressure during
puzzle sequences.

**Multiple Weapons**
The weapon system is built on ICommand and ShootCommand, making it
architecturally ready for multiple weapon types. Future levels will
introduce different projectiles — each with unique travel physics
and throw mechanics — while the core accuracy mechanic stays the
same.


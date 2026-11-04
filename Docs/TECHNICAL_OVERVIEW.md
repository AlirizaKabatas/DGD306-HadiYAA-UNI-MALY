# UNI-MALY — Technical Overview

**Project type:** Unity 2D arcade / local co-op shooter  
**Status:** Student project / playable prototype; code review, not a new runtime certification  
**Editor version in source:** Unity `6000.0.41f1`

This document is a **source-code map** for developers reviewing UNI-MALY. Features below reflect scripts found in this repository. The shipped Windows build was not executed during this portfolio update.

## Architecture at a glance

### Two-player gameplay

- [GameManager.cs](../Assets/Ensar%201/Scripts/GameManager.cs) stores the two selected character prefabs, instantiates them at designated spawn points, activates their player-specific movement/shooting components, and connects them to a shared camera.
- [Player1Movement.cs](../Assets/Ali/AScripts/Player/Player1Movement.cs) and [Player2Movement.cs](../Assets/Ali/AScripts/Player/Player2Movement.cs) use Unity's Input System for movement and variable-height jumps, with separate bindings for each player.
- [Player1Shooting.cs](../Assets/Ali/AScripts/Player/Player1Shooting.cs) and [Player2Shooting.cs](../Assets/Ali/AScripts/Player/Player2Shooting.cs) implement directional fire points, magazine and reload handling, shooting rate, animation/sprite aiming and sound effects.
- [DualTargetCamera.cs](../Assets/Ensar%201/Scripts/DualTargetCamera.cs) provides another two-target framing implementation; a multi-target camera is referenced from the game manager.

**Design intent:** Keep both players visible and make combat readable on a single arcade display.

### Character differentiation

The four characters share common player mechanics but use character prefabs and individual tuning values. Sprite variants and weapon artwork are stored under:

- [Four character bases](../Assets/ARDA/4Char_Silahsız/)
- [Weapon artwork](../Assets/ARDA/Char_weapons_PNG/)
- [Character animations](../Assets/Ali/Animations/Characters/)

Specific health, movement and combat values may be configured in prefabs and scene references; this overview intentionally does not invent an exact stat table.

### Enemy behavior

| Script | Design pattern |
| --- | --- |
| [SlimeEnemy.cs](../Assets/Ali/AScripts/Enemies/SlimeEnemy.cs) | Ground pursuit, attack range, animations and hit feedback |
| [FlyingEnemy.cs](../Assets/Ali/AScripts/Enemies/FlyingEnemy.cs) | Flying pursuit with randomized directional variation |
| [RangedEnemy.cs](../Assets/Ali/AScripts/Enemies/RangedEnemy.cs) | Target acquisition and projectile fire inside a configured angle |
| [EnemyBullet.cs](../Assets/Ali/AScripts/EnemyBullet.cs) | Projectile logic |

The itch.io page describes **four enemy types**. The table names representative code paths, not the exhaustive enemy roster.

### Boss encounters

**Spider Boss**
- [SpiderBoss.cs](../Assets/Ali/AScripts/Bosses/SpiderBoss.cs)
- Finds the nearest player, fires from configured points, teleports at intervals, plays attack/teleport/death feedback and can spawn a portal after defeat.

**Dottopus**
- [DottopusBoss.cs](../Assets/Ali/AScripts/Bosses/DottopusBoss.cs)
- [DottopusMove.cs](../Assets/Ali/AScripts/Bosses/DottopusMove.cs)
- Supports projectile spawns for **laser**, **orb** and **AoE** attacks, configurable cooldowns, movement within defined bounds, health and hit response.

Boss health-bar scripts are stored alongside the controllers under [`Bosses/`](../Assets/Ali/AScripts/Bosses/).

### Health, progression and scenes

- [PlayerHealth.cs](../Assets/Ali/AScripts/Player/PlayerHealth.cs) manages HP, feedback, healing pickups and death events.
- [DeathHandler.cs](../Assets/ARDA/DeathHandler.cs) contains a two-player failure handler.
- Build-settings entries include a menu, intro, tutorial, two stage scenes, win screen and credits. See [`ProjectSettings/EditorBuildSettings.asset`](../ProjectSettings/EditorBuildSettings.asset).

## Input & arcade context

The game is **intended for a dual-gamepad arcade setup**. Gameplay uses Unity's Input System and separate player action maps in the source. Some legacy/editor code also contains keyboard key checks: a code snapshot does not guarantee every menu pathway has identical controller support. The published user-facing build is described as gamepad-only, and that is the intended way to evaluate the game.

## Portfolio scope

- This is **real project source**, not a reconstruction from screenshots.
- It is **collaborative work**; code/art folders and filenames document development organization but should not be treated as exclusive ownership claims.
- The existing game code was **not rewritten** for portfolio purposes.
- No new claim is made about performance, bug-free operation, exact character numbers or hardware compatibility.

**For the original design framing, see [UNI-MALY_GDD.pdf](../UNI-MALY_GDD.pdf).**

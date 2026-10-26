# Architecture map — OmegaV AI and interaction systems

[Portfolio](../README.md) · [Decisions](decisions.md) · [Coverage](coverage.md)

**Evidence boundary:** This describes responsibilities found through production **source inspection** at revision `82a4e9142287dcf4a8c4e3b6e751aea747b95e93`. The diagram is conceptual; it does not prove Unity scene execution or claim the exact C# demonstration abstractions shipped in the game.

## System relationships

```text
Observed world / evidence
        |
        v
[Shared perception geometry + immutable snapshots]
        |       (sensing results, remembered positions)
        +----------------------+----------------------+
                               |                      |
                               v                      v
                     [Combat autonomy]       [Objective brain]
                     threat interpretation   search / acquire /
                     approach / hold /       deliver / engage
                     retreat decisions             |
                               |                    |
                               +----------+---------+
                                          |
                                          v
                            [Champion locomotion agent]
                            movement authority / physics /
                            animation / NavMesh integration

[World Soul Shard state] -- collection/steal/drop --> [Shard holder]
                                                    |
                                                    v
                                            [Objective selection]

[Health/shield depletion] --> [Death/respawn controller]
                                      |
                                      v
                          reset transient combat / movement /
                          character state and restart lifecycle
```

Arrows describe responsibility boundaries and key references/events, **not** a verified complete call graph. The source includes additional guidance, HUD, stats, health, enemy, pickup and networking dependencies that are intentionally omitted from this portfolio view.

## Ownership contracts

| Boundary | Production responsibility inspected | Failure or integration question |
| --- | --- | --- |
| Perception | Eye-origin fallback, horizontal FOV, range and first non-self physics hit; immutable observed samples | Does occlusion correspond to actual ray/physics geometry? |
| Objective selection | Evaluate search/acquire/deliver/engage goals, objective commitment and threat importance | Does combat interrupt only under legitimate conditions? Does pursuit resume? |
| Combat autonomy | Track visible/known hostiles, tactical approach/hold/retreat and action permission | Can stale target or movement authority persist after interruption? |
| Locomotion | Accept selected input source and execute movement/path requests | Are path corners, physics boundaries and Animator transitions reliable? |
| Health/death | Observe depletion, reset transient state, disable/re-enable actor components, choose respawn | Are UnityEvents, serialized references and repeated depletion safe? |
| Shard ownership | Observe collection/steal/drop and keep holder/AI state in sync | Does the scene permit duplicate pickups or unexpected stale event ordering? |

## Important design distinction

The [independent models](../samples/independent-models/README.md) intentionally reduce engine-dependent interactions to small pure contracts. For example:

- `ObservationMemory` demonstrates frozen historical observations; it does not perform raycasts or embody combat's exact memory implementation.
- `ObjectiveArbiter` demonstrates permission, score, commitment and urgency; it does not reproduce the production brain's scores, policies or navigation.
- `CharacterLifecycle` uses generation tokens as an **illustrative proposed safeguard**, not a claim that the Unity controller implements those tokens.
- `RangeSteering` computes a desired command and never moves a Transform.
- `ShardCustody` uses expected revisions to illustrate consistency; it is not production netcode.

The corresponding model checks validate these declared rules in isolation. They do **not** certify scene wiring, Unity compilation, on-device performance, training convergence, physics behavior or the whole game's quality.

## What an implementation review should inspect next

A reviewer can start at the [case-study index](../case-studies/README.md), inspect each independent C# model and its [executable checks](../samples/independent-models/Program.cs), then compare precise source claims with the [verification matrix](verification-matrix.md). The actual production repository and its Git history are deliberately not distributed.

# Character death and respawn lifecycle

[Portfolio](../README.md) · [Independent lifecycle example](../samples/independent-models/CharacterLifecycle.cs) · [Verification matrix](../docs/verification-matrix.md)

## Engineering problem

When a character dies, a health value alone is not enough to keep the simulation consistent. Its AI may have an active target, a locomotion request, a pending attack or an interaction; those transient intentions must be cleared before it returns to play. Repeated depletion signals must not create competing respawn operations.

## Production architecture inspected

`CharacterHealthShield` implements the damage interface and exposes depletion, vital-change and real-damage signals. `CharacterDeathRespawnController` listens for health depletion, blocks duplicate respawn entry, marks the character dead, resets transient champion state, clears autonomous movement requests and temporarily disables configured behaviours, colliders and renderers. It then progresses through respawn events and spawn selection.

The controller uses Unity component references and coroutine timing; correct behavior depends on serialized configuration and the scene. The source supports describing these *responsibilities*, not certifying a complete death–respawn round trip.

**Reviewed code basis:** Production revision `82a4e9142287dcf4a8c4e3b6e751aea747b95e93`, `CharacterHealthShield`, `CharacterDeathRespawnController`. Evidence: **Source inspected**.

## Source-level flow and failure boundaries

The inspected death coroutine first enters a guarded respawning state, marks the actor dead, invokes transient-state resets and clears autonomous locomotion. It disables configured scene components and raises a death-started event before waiting the serialized delay.

On respawn it marks the intermediate state, chooses an eligible nearest `ResonanceSpire` when configured, otherwise a home spawn, otherwise its own transform; then it moves the actor, restores current stats and fills health/shield vitals. Importantly, it invokes **transient resets again after restoration and before enabling components**, clears locomotion again, marks the actor respawned and publishes completion. These ordering choices are visible in the source; their effects still depend on actual Unity event subscriptions and serialized references.

Reset ownership is explicit: the objective brain, combat harness, shard pursuit and projectile attack components each clear their previous-life intentions. The source comment distinguishes transient intent from shard ownership and world state; whether dropped shards or existing holders transition on death is a **separate interaction-system concern**, not something this lifecycle sample proves.

## Independent demonstration

[CharacterLifecycle.cs](../samples/independent-models/CharacterLifecycle.cs) provides a small Alive → Dead → Respawning → Alive contract. Duplicate death requests are ignored, transient movement/target state is cleared and a generation token prevents an outdated completion from advancing a later lifecycle. **Generation fencing is a demonstration design decision**, not a claim that the production controller uses that exact technique.

The checks cover idempotence, dead-state input rejection, stale tokens, completion ordering and the absence of carried-over transient state.

## Trade-offs and known limits

- Coordinated reset keeps transient AI state separate from persistent character identity and progression.
- Guards reduce accidental double starts, but coroutine lifetime, destroyed objects and event subscription ordering still require Unity testing.
- A token is a useful way to illustrate stale-event rejection; it is not a replacement for scene-valid prefab references or health/shield reset.
- This sample has no GameObjects, network replication, resurrection visual effects, shield values or UnityEvents.

## Gameplay evidence to capture

Capture a controlled depletion event, movement/target cleanup, temporary inactive state, chosen respawn and resumed movement. Record scene/build and visible diagnostics. **No Unity runtime test or gameplay recording is claimed here.**

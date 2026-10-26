# Combat locomotion and tactical range

[Portfolio](../README.md) · [Independent range example](../samples/independent-models/RangeSteering.cs) · [Architecture](../docs/architecture.md)

## Engineering problem

An autonomous character needs to move smoothly, maintain useful spacing from an opponent, avoid blind edge/ground assumptions and follow routes without letting several unrelated decision systems directly compete for its physics controls.

## Production architecture inspected

`ChampionLocomotionAgent` is an ML-Agents-derived Unity component with movement, acceleration/deceleration, jump buffering, coyote time, air control, animator integration and autonomous NavMesh pathing parameters. It exposes an input-source distinction between ML-Agents policy and deterministic combat autonomy. The combat harness supplies tactical intent and defines states for approach, range holding and retreat.

These code paths show **intended separation between decision input and locomotion execution**. Their presence does not establish that an ML policy has been trained, that long or multi-corner paths always succeed, or that these controls have been benchmarked.

**Reviewed code basis:** Production revision `82a4e9142287dcf4a8c4e3b6e751aea747b95e93`, `ChampionLocomotionAgent` and `VictorCombatAutonomyHarness`. Evidence: **Source inspected**.

## Source-level flow and failure boundaries

For deterministic autonomy, movement and destination requests are issued with an **owner**, **priority** and expiration. The reviewed code maintains separate combat and Soul Shard pursuit requests, selects combat as the winning active request when one exists, and re-evaluates the winner as requests change or expire. That is a concrete input-ownership mechanism, not simply a blend of two vectors.

For destination requests, the locomotion agent maintains path-corner buffers and path-generation/repath diagnostics. It computes or refreshes an autonomous path, validates cached usability, advances reached corners and derives horizontal steering toward the active corner. Blocked or degenerate path cases can return zero steering and neutralize planar motion. This source control flow does **not** guarantee success in every baked NavMesh or scene.

The independent range model demonstrates only the tactical **desired** approach/hold/retreat command. It deliberately does not copy owner arbitration, path-replanning, rigidbody application, animation transitions or ML-Agent policy actions; those require the production Unity integration to inspect.

## Independent demonstration

[RangeSteering.cs](../samples/independent-models/RangeSteering.cs) isolates an approach/hold/retreat command based on distance, preferred/cautious range, health and tolerance. It validates inputs and makes exact band boundaries inspectable. This is **a small sample policy**, not a Unity movement controller or reproduction of the production combat state machine.

Executable checks cover far/near/within-band decisions, low-health spacing, inclusive boundaries and invalid values.

## Trade-offs and known limits

- Isolating movement *intent* from *execution* supports different control sources and clearer ownership.
- A range band avoids constant oscillation near one target distance; its tolerances require in-game tuning.
- Geometry-only steering cannot account for walls, physics, navigation reachability, recoil, facing/aim or latency.
- Actual NavMesh corner traversal, input authority switches, animation response and fall safety require scene-level verification.

## Gameplay evidence to capture

Observe path traversal around an obstacle, approach to attack range, range holding and low-health retreat/recovery. Label what was *seen* versus what would require instrumented state traces. **Not recorded; no performance measurement claimed.**

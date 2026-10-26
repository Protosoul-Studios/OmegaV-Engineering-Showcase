# Soul Shard collection and custody

[Portfolio](../README.md) · [Independent custody example](../samples/independent-models/ShardCustody.cs) · [Evidence](../docs/coverage.md)

## Engineering problem

A collectible can move through states: available in the world, carried by one character, stolen by another or dropped. Its authoritative holder must stay consistent with the UI and AI's plans; stale events should not silently award the same shard to multiple actors.

## Production architecture inspected

`SoulShardHolder` stores the held-shard reference, observes shard-collected, shard-stolen and shard-dropped events, and announces holder-local changes. On collection or acquisition through theft it can update the AI character state to holding a shard. `ChampionObjectiveBrain` distinguishes search, acquisition and delivery objectives and references the shard pursuit component.

This source review confirms the event-handling and ownership boundaries represented in code. It does **not** prove every pickup radius, drop, steal, death or reacquisition path works in the Unity scene.

**Reviewed code basis:** Production revision `82a4e9142287dcf4a8c4e3b6e751aea747b95e93`, `SoulShardHolder`, objective brain and inspected shard-related dependencies. Evidence: **Source inspected**.

## Source-level flow and failure boundaries

The shard behavior has a delayed pickup coroutine that rechecks **collection operation identity** and revalidates the candidate after the delay, before assigning a holder. A successful collection attaches to the holder, clears pending selection state and emits `OnShardCollected`. The holder component listens for that event only when the matching GameObject is involved.

The inspected steal coroutine changes the holder after its configured delay and emits either a stolen or collected event based on previous ownership. Dropping clears held/target/pending state, detaches the object, establishes a world drop anchor and publishes `OnShardDropped` for the previous holder. These are source-level transitions; animation, player distance, collision and event ordering remain scene-dependent.

A key distinction: the **production pickup operation ID** prevents a stale delayed pickup from completing; the independent `ShardCustody` example instead uses an expected revision across collection/steal/drop operations. The concepts are related but **not the same algorithm or evidence**. The model's revision fence is a deliberately simplified illustration rather than a claimed production feature.

## Independent demonstration

[ShardCustody.cs](../samples/independent-models/ShardCustody.cs) supplies a compact, synchronous single-holder ledger. Collection, theft and drop require an expected revision and the appropriate current holder. Out-of-date or conflicting requests cannot mutate the demo state, and an unheld shard cannot be stolen by supplying a missing previous holder. **Revision-checked ownership is a portfolio model choice**, not a claim about production's event system or network authority.

Executable checks cover collection, exclusivity, transfer, stale or wrong-owner rejection, drop and recollection.

## Trade-offs and known limits

- Event-driven holders avoid having every objective component rewrite shard ownership.
- A single-owner invariant is easy to state but harder to enforce across all scene and networking boundaries.
- Version checks model stale operations in one process; they do not solve distributed races, network replication or rollback.
- The example omits Unity collisions, proximity, shard visuals, scoring, return-to-spawn behavior and save persistence.

## Gameplay evidence to capture

Show a shard becoming collectible, collection by one character, a theft or controlled drop, and the next legitimate acquisition or recovery. Preserve clear build and scene metadata. **Video and runtime verification pending.**

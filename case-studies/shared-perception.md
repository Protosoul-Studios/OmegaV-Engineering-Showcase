# AI perception and last-known position

[Portfolio](../README.md) · [Standalone memory example](../samples/independent-models/ObservationMemory.cs) · [Evidence](../docs/coverage.md)

## Engineering problem

A character that briefly sees an opponent may need to remember the last observed location after line of sight is blocked. That memory must not magically follow a moving actor behind a wall. Meanwhile, eye-height selection, range checks and raycast handling should be reusable without imposing one character's threat policy on every AI consumer.

## Production architecture inspected

The production `PerceptionGeometry` helper resolves an observation origin from a configured transform, humanoid head, upper collider bounds or fallback root height. Range uses 3D distance while the field-of-view angle is computed on the horizontal plane. For combat rays, it sorts hits by distance and skips colliders belonging to the observer; the consuming system still interprets whether the first remaining hit represents the intended target.

`PerceptionSnapshot` holds an immutable world position and observation timestamp rather than a live `Transform`. The combat consumer is responsible for separate visibility flags, memory duration, explicit threat evidence, candidate selection and action policy. Strategic shard sight and close pursuit retain different linecast/sphere-cast behavior; this foundation does **not** pretend they all share identical sensing.

**Reviewed code basis:** Production revision `82a4e9142287dcf4a8c4e3b6e751aea747b95e93`, `PerceptionGeometry`, `PerceptionSnapshot`, combat and objective consumers. Evidence label: **Source inspected**.

## Source-level flow and failure boundaries

1. The combat consumer performs periodic awareness refresh and resolves eligible hostile candidates; the geometry helper answers whether the sample lies within range/FOV and what the first non-self ray hit was. The consumer, not the helper, decides whether that hit belongs to a hostile actor.
2. Last-known position and timestamp are stored separately from live visibility. Expiration, explicit evidence and whether an objective may investigate a remembered position are higher-level decisions. A snapshot does not itself grant movement or attack permission.
3. Sharing eye-origin fallback does **not** collapse three distinct sensing contracts: combat rays include trigger hits, strategic shard view uses linecasts and close shard pursuit uses sphere-casts. The differing filters are deliberate compatibility boundaries.
4. The reviewed engineering notes describe a previously identified case where objective assignment refreshed hidden-position memory from a live Transform; the source-level fix avoids refresh on mere assignment. They **do not** establish the physical reason for a separate reported wall-occlusion result. A real wall/raycast retest remains the correct evidence gate.

A reviewer should ask for a captured ray origin, sampled point, collider/layer and visibility transition before accepting an occlusion claim. The model tests cannot answer that physical question.

## Independent demonstration

[ObservationMemory.cs](../samples/independent-models/ObservationMemory.cs) models the observable *knowledge contract*: an accepted observation records position/time; losing sight changes visibility but does not update either sample; reacquisition replaces them; age-based reads can expire; death/reset can clear them. Its non-finite and out-of-order guards are **portfolio model choices**, not claims about the game's exact validation code.

[Executable checks](../samples/independent-models/Program.cs) cover sampling, loss-of-sight freeze, expiry boundary, reacquisition, clearing and invalid inputs.

## Trade-offs and known limits

- A shared geometry utility reduces duplicated mechanics, but choosing enemies or valuing threats remains the consumer's responsibility.
- A remembered position can be useful for investigation, but is not evidence of current visibility or hostility.
- Production combat still uses array-producing physics queries; the geometry extraction is **not** a measured zero-allocation sensing system.
- Prior engineering notes identified a separate hidden-target refresh issue and requested additional wall-occlusion verification. Source inspection cannot establish that a particular wall configuration blocks the actual test ray.

## Gameplay evidence to capture

Show hostile acquisition, wall occlusion, a target moving while hidden, and reacquisition with meaningful on-screen status/debug output. Identify the build and scene, and document any failed or ambiguous observation. **Not recorded; no Unity test pass claimed.**

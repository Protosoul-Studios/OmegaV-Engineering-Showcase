# Combat autonomy and objective selection

[Portfolio](../README.md) · [Independent objective example](../samples/independent-models/ObjectiveArbiter.cs) · [Architecture](../docs/architecture.md)

## Engineering problem

A self-directed character can pursue a collectible while also responding to a nearby enemy. A sustainable design must distinguish **what the actor is trying to accomplish** from **how combat movement and attacks are executed**, and distinguish an actual urgent threat from mere awareness of an opponent.

## Production architecture inspected

`ChampionObjectiveBrain` defines search, acquire, deliver and engage objectives, references the shard pursuit system and the combat harness, and includes scoring/commitment parameters. `VictorCombatAutonomyHarness` manages target/perception information and tactical states such as approaching, holding attack range, retreating and holding retreat. It also integrates health thresholds, guidance inputs and projectile attack status.

The source divides responsibilities: the objective brain selects a strategic direction; combat handles engagement decisions; locomotion receives movement requests; sensing supplies evidence rather than unconditional permission to interrupt an objective. This is a **source-level architecture description**, not a claim that every switch or animation has been observed at runtime.

**Reviewed code basis:** Production revision `82a4e9142287dcf4a8c4e3b6e751aea747b95e93`, `ChampionObjectiveBrain` and `VictorCombatAutonomyHarness`. Evidence: **Source inspected**.

## Source-level flow and failure boundaries

The objective brain's update path checks missing dependencies and dead/respawning availability before asking the actor to pursue a goal. Strategic shard sensing and objective scoring run at separate configured intervals, while the selected objective is executed through an explicit dispatch path. Scoring weighs shard visibility/memory, current possession, search and combat context. Carrying a shard can redirect the actor toward delivery unless legitimate urgency changes the choice.

The combat harness exposes tactical states and methods for known-hostile visibility, external hostile evidence, urgent-threat selection and objective permission. This division is significant: awareness of an actor, remembered knowledge and an actionable urgent threat are different concepts. The objective brain uses the combat harness instead of implementing the entire projectile, targeting and locomotion stack itself.

Two integration checks remain important: (1) an urgent interruption must not hand movement to competing owners at once, and (2) when combat is over, permissions and intent should return to the still-valid objective. Source methods such as `ClearExecutorPermissions` and transient-life resets make these responsibilities inspectable, but do not independently prove the Unity sequence.

## Independent demonstration

[ObjectiveArbiter.cs](../samples/independent-models/ObjectiveArbiter.cs) demonstrates explicit eligibility, ordered scoring, minimum commitment and urgent preemption. Its simplified inputs are: carrying a shard, observing/remembering a shard, confirmed/urgent threat and whether combat is allowed. The exact numbers and simplified ranking belong to **this sample**, not to OmegaV's shipped or current production tuning.

The companion checks exercise search/acquire/deliver decisions, permission-controlled combat, urgent interruption, commitment time, invalidated targets and monotonic evaluation time.

## Trade-offs and known limits

- Separating strategic and tactical control avoids a single script trying to own every decision, but introduces an explicit handoff contract.
- Commitment protects against constant goal-switching, but can delay a worthwhile nonurgent change.
- Urgent interruption should come from legitimate evidence; passive visibility should not automatically imply aggression.
- This model does not evaluate physics, animation, enemy factions, melee range, actual projectiles, ML policy output or multiplayer arbitration.
- Existing production development notes called for targeted verification of wall perception, combat cancellation/resumption and enemy death interactions.

## Gameplay evidence to capture

Record shard pursuit, a genuinely threatening enemy, engagement and eventual return to the objective. Explain any assistance/debug overlays and record the exact observed outcome. **Not recorded; runtime correctness pending.**

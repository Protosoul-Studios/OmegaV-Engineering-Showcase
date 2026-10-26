# Engineering decisions, alternatives and trade-offs

[Portfolio](../README.md) · [Architecture](architecture.md) · [Evidence](coverage.md)

**Provenance:** The production-boundary descriptions below are retrospective interpretations of inspected source. The portfolio model designs are independent examples, **not** statements that identical patterns were deployed. This record avoids attributing undocumented design discussions or performance figures to the original game.

| ID | Decision or inspected boundary | Reason / advantage | Alternative and cost | Evidence |
| --- | --- | --- | --- | --- |
| DEC-01 | Shared perception geometry, consumer-owned target policy | Reuse observation mechanics without binding them to one character's hostility rules | Monolithic perception manager may centralize policy but risks overcoupling | [Perception](../case-studies/shared-perception.md), source inspected |
| DEC-02 | Immutable sampled position/time instead of live hidden-position reads | Keeps known history distinct from current visibility | Persistent live Transform is convenient but risks omniscient pursuit | [Perception](../case-studies/shared-perception.md), source inspected |
| DEC-03 | Strategic objective brain separate from tactical combat harness | One owner for *what to pursue*, another for *how to engage* | One all-purpose brain reduces component boundaries but increases competing responsibilities | [Combat](../case-studies/combat-autonomy.md), source inspected |
| DEC-04 | Locomotion accepts different movement input sources | Allows policy-driven or deterministic movement intent with common execution | Independent controllers may conflict or duplicate Animator/physics code | [Locomotion](../case-studies/combat-locomotion.md), source inspected |
| DEC-05 | Explicit death/respawn coordinator reset | Clears transient decisions across a lifecycle boundary | Ad hoc component-by-component cleanup is difficult to audit | [Lifecycle](../case-studies/character-lifecycle.md), source inspected |
| DEC-06 | Shard holder listens to ownership events | Local reference and UI/AI signals follow collectible changes | Direct polling is simpler in some scenes but may cause redundant checks | [Shard](../case-studies/soul-shard-interaction.md), source inspected |
| DEC-07 | Independent .NET models for portfolio review | Runnable, minimal dependency, no private Unity code disclosure | Raw production code shows more exact integration but exposes implementation and assets | [Models](../samples/independent-models/README.md), showcase-only |
| DEC-08 | Revision-checked demo custody ledger | Shows stale-operation rejection plainly | Simple mutable holder field is shorter but cannot identify stale requests | [Shard model](../samples/independent-models/ShardCustody.cs), showcase-only |
| DEC-09 | Respawn-generation token in demo | Makes obsolete completion rejection explicit | Only guarding a running coroutine is simpler but hides delayed-callback failure mode | [Lifecycle model](../samples/independent-models/CharacterLifecycle.cs), showcase-only |

## Known non-goals

This portfolio does not promise zero-allocation perception, guaranteed navigation, a proven ML training result, deterministic multiplayer combat, network-authoritative shard custody, crash-safe persistence, or performance measurements. Those would require additional evidence and, where appropriate, separate engineering work.

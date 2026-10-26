# Claim-to-evidence coverage

[Portfolio](../README.md) · [Verification](verification-matrix.md) · [Gameplay evidence](../media/videos/README.md)

**Production source baseline inspected:** `82a4e9142287dcf4a8c4e3b6e751aea747b95e93`. File names identify reviewed responsibilities, **not** public links to the private project. **No code from that production revision is published here.**

| ID | Supported production claim | Inspected source responsibility | Status | Public explanation | Limit |
| --- | --- | --- | --- | --- | --- |
| ENG-01 | Reusable origin/FOV/ray geometry and immutable sampled knowledge | `PerceptionGeometry`; `PerceptionSnapshot` | **Source inspected** | [Perception](../case-studies/shared-perception.md) | Wall-occlusion and sensing runtime unverified |
| ENG-02 | Separate strategic objective selection and combat tactical states | `ChampionObjectiveBrain`; `VictorCombatAutonomyHarness` | **Source inspected** | [Combat](../case-studies/combat-autonomy.md) | No fresh runtime objective transition evidence |
| ENG-03 | Health-depletion-triggered reset and respawn coordination | `CharacterHealthShield`; `CharacterDeathRespawnController` | **Source inspected** | [Lifecycle](../case-studies/character-lifecycle.md) | Prefab, events and scene lifecycle not tested here |
| ENG-04 | Locomotion component separates input source and movement settings | `ChampionLocomotionAgent`; combat harness | **Source inspected** | [Locomotion](../case-studies/combat-locomotion.md) | No actual path, physics or trained-policy proof |
| ENG-05 | Shard holder responds to collection, theft and drop events | `SoulShardHolder`; objective/shard references | **Source inspected** | [Shard](../case-studies/soul-shard-interaction.md) | Pickup/drop/steal runtime paths not exercised here |

## Separately verified independent portfolio examples

These claims apply **only** to independently written .NET 10 LTS models. They are not elevated into production claims.

| ID | Model | Checks | Status and evidence |
| --- | --- | --- | --- |
| MOD-01 | [Observation memory](../samples/independent-models/ObservationMemory.cs) | 9 | **Pending new-repository CI** — [workflow](https://github.com/Protosoul-Studios/OmegaV-Engineering-Showcase-Rebuild/actions/workflows/showcase-model-checks.yml) |
| MOD-02 | [Objective arbitration](../samples/independent-models/ObjectiveArbiter.cs) | 10 | **Pending new-repository CI** — same workflow |
| MOD-03 | [Character lifecycle](../samples/independent-models/CharacterLifecycle.cs) | 7 | **Pending new-repository CI** — same workflow |
| MOD-04 | [Range steering](../samples/independent-models/RangeSteering.cs) | 7 | **Pending new-repository CI** — same workflow |
| MOD-05 | [Shard custody](../samples/independent-models/ShardCustody.cs) | 10 | **Pending new-repository CI** — same workflow |

**Rebuild verification:** The independent C# examples retain the same 43-check suite. A successful run of the new repository's .NET 10 workflow must be recorded here before publication. Historical workflow URLs and commit IDs belong to the old repository and are intentionally not reused. Neither Unity production compilation nor gameplay is exercised by these checks.

## Planned evidence, not claimed

| ID | Claim to establish by actual observation | Status | Capture guide |
| --- | --- | --- | --- |
| VID-01 | LOS loss freezes remembered hostile position; reacquisition updates it | **Pending** | [Gameplay gallery](../media/videos/README.md) |
| VID-02 | Threat interruption and objective resumption | **Pending** | [Gameplay gallery](../media/videos/README.md) |
| VID-03 | Death/respawn restores actor and clears transient behavior | **Pending** | [Gameplay gallery](../media/videos/README.md) |
| VID-04 | Navigation and tactical spacing in a representative scene | **Pending** | [Gameplay gallery](../media/videos/README.md) |
| VID-05 | Shard collection, transfer/drop and follow-up ownership | **Pending** | [Gameplay gallery](../media/videos/README.md) |

A recording can establish only visible behavior under its recorded conditions; deeper policies, timing and robustness require instrumented or additional targeted tests. Do not convert a pending row to a pass without named, retained evidence.

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
| MOD-01 | [Observation memory](../samples/independent-models/ObservationMemory.cs) | 9 | **Automated model checks passed** — [GitHub Actions run #40262196307](https://github.com/Protosoul-Studios/OmegaV-Engineering-Showcase/actions/runs/40262196307) |
| MOD-02 | [Objective arbitration](../samples/independent-models/ObjectiveArbiter.cs) | 10 | **Automated model checks passed** — same run |
| MOD-03 | [Character lifecycle](../samples/independent-models/CharacterLifecycle.cs) | 7 | **Automated model checks passed** — same run |
| MOD-04 | [Range steering](../samples/independent-models/RangeSteering.cs) | 7 | **Automated model checks passed** — same run |
| MOD-05 | [Shard custody](../samples/independent-models/ShardCustody.cs) | 10 | **Automated model checks passed** — same run |

**Current model verification:** All **43/43** model checks passed on a GitHub-hosted Ubuntu runner using .NET SDK 10.0.14, [GitHub Actions run #40262196307](https://github.com/Protosoul-Studios/OmegaV-Engineering-Showcase/actions/runs/40262196307), commit `acf7d8097c9f3182a8faade98cd5ff15467fde7d`; the 16-document Markdown/path audit passed as well. This verification belongs to the current official showcase repository; prior archive workflow URLs are not reused. Neither production Unity compilation nor gameplay was exercised.

## Planned evidence, not claimed

| ID | Claim to establish by actual observation | Status | Capture guide |
| --- | --- | --- | --- |
| VID-01 | LOS loss freezes remembered hostile position; reacquisition updates it | **Pending** | [Gameplay gallery](../media/videos/README.md) |
| VID-02 | Threat interruption and objective resumption | **Pending** | [Gameplay gallery](../media/videos/README.md) |
| VID-03 | Death/respawn restores actor and clears transient behavior | **Pending** | [Gameplay gallery](../media/videos/README.md) |
| VID-04 | Navigation and tactical spacing in a representative scene | **Pending** | [Gameplay gallery](../media/videos/README.md) |
| VID-05 | Shard collection, transfer/drop and follow-up ownership | **Pending** | [Gameplay gallery](../media/videos/README.md) |

A recording can establish only visible behavior under its recorded conditions; deeper policies, timing and robustness require instrumented or additional targeted tests. Do not convert a pending row to a pass without named, retained evidence.

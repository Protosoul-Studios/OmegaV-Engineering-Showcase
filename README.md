# OmegaV: AI Battle Champions

**Unity/C# gameplay systems and autonomous-character engineering — Protosoul Studios**

[![Independent C# model checks](https://github.com/Protosoul-Studios/OmegaV-Engineering-Showcase/actions/workflows/showcase-model-checks.yml/badge.svg?branch=main)](https://github.com/Protosoul-Studios/OmegaV-Engineering-Showcase/actions/workflows/showcase-model-checks.yml)

*Badge status covers the independent C# portfolio models and repository link checks, not the Unity production game's tests.*

OmegaV is an in-development arena-oriented game in which autonomous characters pursue objectives, engage opponents, navigate combat spaces and interact with collectible Soul Shards. This repository presents **five focused engineering case studies** and **five standalone, executable C# examples** of the design ideas behind them.

**For prospective clients:** inspect state ownership, defensive contracts, evidence boundaries and the runnable code examples. **For potential collaborators:** follow the dependency map and trade-offs to understand how the systems fit together. **For players and fans:** discover the kinds of AI decisions that shape moment-to-moment behavior, without revealing the unreleased game project.

> **Portfolio, not a game download.** The C# examples are independently authored `.NET 10 LTS` review models, **not** copied Unity production scripts, executable gameplay builds, or claims that these exact abstractions shipped. The underlying production game and assets remain private.

## Explore the engineering

| System | Why it matters | Read the case study | Inspect independent C# example |
| --- | --- | --- | --- |
| **AI perception and last-known position** | Distinguish *seen now* from *known before*, without silently tracking actors through walls | [Perception](case-studies/shared-perception.md) | [ObservationMemory.cs](samples/independent-models/ObservationMemory.cs) |
| **Combat autonomy and objective selection** | Coordinate shard objectives with legitimate hostile interruptions and tactical decisions | [Combat and objectives](case-studies/combat-autonomy.md) | [ObjectiveArbiter.cs](samples/independent-models/ObjectiveArbiter.cs) |
| **Character death and respawn lifecycle** | Reset transient state at death boundaries to avoid stale targets and movement | [Lifecycle](case-studies/character-lifecycle.md) | [CharacterLifecycle.cs](samples/independent-models/CharacterLifecycle.cs) |
| **Combat locomotion and range** | Separate movement intent from its execution and maintain tactical spacing | [Locomotion](case-studies/combat-locomotion.md) | [RangeSteering.cs](samples/independent-models/RangeSteering.cs) |
| **Soul Shard ownership and interaction** | Track collection, theft and release without competing holders | [Shard interactions](case-studies/soul-shard-interaction.md) | [ShardCustody.cs](samples/independent-models/ShardCustody.cs) |

Each case study describes the **production responsibilities inspected**, the **simplified independent model** that illustrates an engineering principle, important **design trade-offs**, and **what still needs gameplay evidence**.

## Executable C# demonstration

The independent examples have no Unity, ML-Agents or NuGet package dependency. The independently authored C# models have **43 executable contract checks**, all passed under .NET 10 LTS in [GitHub Actions run #40262196307](https://github.com/Protosoul-Studios/OmegaV-Engineering-Showcase/actions/runs/40262196307) at commit `acf7d8097c9f3182a8faade98cd5ff15467fde7d`. These are independently verified portfolio-model checks, **not** production Unity tests. The [verification matrix](docs/verification-matrix.md) separates this independent model evidence from production Unity claims. Run them yourself using .NET SDK 10:

```bash
dotnet run --project samples/independent-models/IndependentModels.csproj --configuration Release
```

The [sample documentation](samples/independent-models/README.md) explains each model and limitation; the [verification matrix](docs/verification-matrix.md) records what was actually executed, not simply what was written. A GitHub Actions workflow is provided for reproducibility.

## Implementation context and evidence

In the inspected OmegaV code, a shared perception helper and immutable snapshots serve consumer-owned knowledge; an objective brain coordinates search/acquisition/delivery/engagement; the combat harness requests tactical behavior from locomotion; health depletion enters a character respawn routine; and shard ownership reacts to collection, theft and drop events. These are **source-inspected facts**, not proof of runtime performance, comprehensive AI reliability, or completed game QA.

The showcase intentionally does **not** contain the original Unity project, its C# scripts, production Git history, packages, prefabs, characters, or private telemetry. Gameplay demonstrations have not yet been added to this public code-first portfolio. When approved footage is ready, it will appear in the [gameplay evidence gallery](media/videos/README.md).

## Engineering documentation

[Architecture map](docs/architecture.md) · [Decisions and trade-offs](docs/decisions.md) · [Claim/evidence coverage](docs/coverage.md) · [Verification matrix](docs/verification-matrix.md) · [Gameplay evidence](media/videos/README.md) · [Changelog](CHANGELOG.md)

## Viewing and reuse

**© 2026 Protosoul Studios and applicable rights holders. All rights reserved.** This is a professional **view-for-review portfolio**, **not open source**. No general permission to copy, modify, redistribute, incorporate or commercially reuse code, writing, media or assets is granted. GitHub platform terms and applicable law still apply; see the [code review and reuse policy](docs/code-review-and-reuse.md).

**Release scope:** Curated engineering case studies and independently tested C# models. Game footage, if supplied, is documented separately as observed gameplay, not a substitute for production tests.

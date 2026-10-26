# Independent OmegaV engineering models

Five compact, runnable C#/.NET 10 LTS demonstrations illustrate architectural decisions. They were **authored for this portfolio**, not copied from the private Unity repository, and are not plug-in-ready game components. Read alongside [the five case studies](../../case-studies/README.md).

| Model | Demonstrates | Does not establish |
| --- | --- | --- |
| `ObservationMemory.cs` | Frozen last-observed coordinates, explicit reacquisition, expiry | Real Unity physics or line-of-sight correctness |
| `ObjectiveArbiter.cs` | Objective eligibility, priority, commitment and urgent preemption | The production AI scoring policy |
| `CharacterLifecycle.cs` | Idempotent death, transient cleanup, respawn-generation fencing | Actual prefab, animation, coroutine or scene recovery |
| `RangeSteering.cs` | Pure approach/hold/retreat standoff logic | NavMesh traversal, collision or actual actor movement |
| `ShardCustody.cs` | Single-holder, revision-checked collection, theft and drop; invalid ownership rejection | Multiplayer synchronization, production events or persistence |

Run all checks with .NET SDK 10:

```bash
dotnet run --project samples/independent-models/IndependentModels.csproj --configuration Release
```

`Program.cs` contains **43 executable checks**, including rejection of invalid ownership-transfer requests. The same **43 checks** are retained and independently verified on .NET 10 LTS: all passed in [GitHub Actions run #40261815750](https://github.com/Protosoul-Studios/OmegaV-Engineering-Showcase-Rebuild/actions/runs/40261815750) at `1d945f51bffa89fd9f25d7a081fefcb66eff7b46`. See the [verification matrix](../../docs/verification-matrix.md) for the precise scope and limitations. These verify the standalone models only, not OmegaV gameplay. Uses only base .NET libraries and no NuGet test framework. Treat warnings as errors. No game assets or production code are distributed.

**All rights reserved. No general code reuse license is granted.** See [review/reuse policy](../../docs/code-review-and-reuse.md).

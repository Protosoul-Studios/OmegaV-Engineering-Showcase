# Verification matrix

[Portfolio](../README.md) · [Claim coverage](coverage.md) · [Run instructions](../samples/independent-models/README.md)

## Recorded results

| Layer | Method and revision | Actual outcome | What the outcome means |
| --- | --- | --- | --- |
| Independent C# contract models | .NET 10 LTS Release run at `acf7d8097c9f3182a8faade98cd5ff15467fde7d` | **43/43 checks passed** in new repository CI | Independent model contracts compiled and passed; Unity production was not executed |
| Production source inspection | Read identified Unity C# modules at `82a4e9142287dcf4a8c4e3b6e751aea747b95e93` | **Source inspected** | Described ownership, fields, branches and events exist in reviewed source |
| Fresh Unity Editor compilation | Not executed during showcase preparation | **Pending / not run** | Cannot claim Unity compile or EditMode/PlayMode test pass |
| Fresh gameplay scene behavior | Not independently executed or recorded | **Pending / not observed** | Cannot claim perception, movement, death or shard scenarios passed in game |
| Dedicated hardware/device testing | Not performed | **Pending / not tested** | No platform performance, frame rate or device certification claim |

**Current independently verified result:** [GitHub Actions run #40262196307](https://github.com/Protosoul-Studios/OmegaV-Engineering-Showcase/actions/runs/40262196307) at `acf7d8097c9f3182a8faade98cd5ff15467fde7d`, .NET SDK 10.0.14, reported `PASS 01` through `PASS 43` and `ALL 43 INDEPENDENT MODEL CHECKS PASSED`. The repository guard also passed all 16 Markdown files, links, and distribution paths. Earlier archive CI runs are not presented as verification of this repository. There is no external NuGet test framework, Unity Editor run, or production gameplay test in this workflow.

## Executable check groups

| Model check group | Checks | Contract |
| --- | ---: | --- |
| Observation and last-known position | 9 | Snapshot receipt, LOS loss, stale memory, time order, reacquisition and clear |
| Strategic objective arbitration | 10 | Candidate selection, urgent interruption, permission, commitment and invalidation |
| Lifecycle cleanup | 7 | Death guard, transient reset, stale-generation rejection and respawn |
| Tactical range steering | 7 | Approach/hold/retreat bands, health threshold, input validation |
| Soul Shard custody | 10 | Exclusive holder, revision checks, collection, transfer and drop |
| **Total** | **43** | **Independent model checks only** |

## Reproduce

```bash
dotnet run --project samples/independent-models/IndependentModels.csproj --configuration Release
```

Inspect [the test runner](../samples/independent-models/Program.cs) and the [CI workflow](../.github/workflows/showcase-model-checks.yml). The project targets `net10.0`, enables nullable analysis and treats compiler warnings as errors.

## Evidence limitations

- Standalone `.NET 10` logic is not executed by Unity, even where it illustrates a similar responsibility.
- A passing example test does not establish the production implementation adopted its stronger guard or exact algorithm.
- Static source review cannot confirm serialized scene references, physics hit order in a particular scene, animation behavior, authoritative network state or ML training.
- No new build, video, profiler trace, device log, performance measurement or Unity test output was obtained for the production game in this showcase workflow.
- If later footage contradicts a source-derived hypothesis, record the mismatch and correct the claim rather than hiding it.

## Targeted Unity checks — only when strengthening gameplay claims

A future authorized Unity review should log source revision, Unity version, scene, expected behavior, observed behavior and evidence for: LOS wall and moving-hidden-target test; urgent combat interruption/resume; lethal damage and clean respawn; path/attack band traversal; and shard collect/steal/drop transitions. These are planned actions, **not passes**.

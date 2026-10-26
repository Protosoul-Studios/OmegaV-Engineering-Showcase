# Testing and verification

[Portfolio](../README.md) · [Verification matrix](../docs/verification-matrix.md) · [Executable checks](../samples/independent-models/Program.cs)

**Recorded automated result (.NET 10 LTS):** All **43/43 independent model checks passed** in [GitHub Actions run #40262196307](https://github.com/Protosoul-Studios/OmegaV-Engineering-Showcase/actions/runs/40262196307) at `acf7d8097c9f3182a8faade98cd5ff15467fde7d`, using .NET SDK 10.0.14; the same run passed the 16-document repository link/file audit. These checks exercise only standalone models, **not the Unity production game**.

Reproduce:

```bash
dotnet run --project samples/independent-models/IndependentModels.csproj --configuration Release
```

| Check area | Count | Result |
| --- | ---: | --- |
| Observation memory | 9 | Pass |
| Objective arbitration | 10 | Pass |
| Character lifecycle | 7 | Pass |
| Tactical range | 7 | Pass |
| Shard custody | 10 | Pass |
| **Total** | **43** | **Pass** |

**Pending:** Unity Editor compilation, targeted gameplay scene checks, hardware/device checks and filmed demonstrations. See the [evidence matrix](../docs/coverage.md) for claim-specific limitations.

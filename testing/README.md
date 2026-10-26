# Testing and verification

[Portfolio](../README.md) · [Verification matrix](../docs/verification-matrix.md) · [Executable checks](../samples/independent-models/Program.cs)

**Rebuild verification:** The same five independently authored models and 43 checks have been restored. A fresh .NET 10 GitHub Actions result for this repository will be recorded before publication. These checks exercise only the standalone models, **not the Unity production game**.

Reproduce:

```bash
dotnet run --project samples/independent-models/IndependentModels.csproj --configuration Release
```

| Check area | Count | Result |
| --- | ---: | --- |
| Observation memory | 9 | Pending new CI |
| Objective arbitration | 10 | Pending new CI |
| Character lifecycle | 7 | Pending new CI |
| Tactical range | 7 | Pending new CI |
| Shard custody | 10 | Pending new CI |
| **Total** | **43** | **Pending new CI** |

**Pending:** Unity Editor compilation, targeted gameplay scene checks, hardware/device checks and filmed demonstrations. See the [evidence matrix](../docs/coverage.md) for claim-specific limitations.

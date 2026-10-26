// Copyright © 2026 Protosoul Studios. All rights reserved.
using System;

namespace OmegaV.Showcase.Models;

// Pure range policy. No physics, NavMesh, pathfinding, frame timing or ML training.
public enum RangeCommand { Approach, Hold, Retreat }

public static class RangeSteering
{
    public static RangeCommand Decide(double distance, double healthFraction, double preferredRange,
        double cautiousRange, double lowHealthThreshold, double tolerance)
    {
        if (!double.IsFinite(distance) || distance < 0 ||
            !double.IsFinite(healthFraction) || healthFraction < 0 || healthFraction > 1 ||
            !double.IsFinite(preferredRange) || preferredRange <= 0 ||
            !double.IsFinite(cautiousRange) || cautiousRange <= 0 ||
            !double.IsFinite(lowHealthThreshold) || lowHealthThreshold < 0 || lowHealthThreshold > 1 ||
            !double.IsFinite(tolerance) || tolerance < 0)
            throw new ArgumentOutOfRangeException(nameof(distance), "Ranges and health inputs must be finite and valid.");

        double desired = healthFraction <= lowHealthThreshold ? cautiousRange : preferredRange;
        if (distance > desired + tolerance) return RangeCommand.Approach;
        if (distance < Math.Max(0, desired - tolerance)) return RangeCommand.Retreat;
        return RangeCommand.Hold;
    }
}

// Copyright © 2026 Protosoul Studios. All rights reserved.
using System;

namespace OmegaV.Showcase.Models;

// Standalone portfolio model. NOT an OmegaV Unity production component.
public readonly record struct SamplePoint(double X, double Y, double Z);
public readonly record struct TargetObservation(SamplePoint Position, double AtSeconds);

public sealed class ObservationMemory
{
    private TargetObservation? last;
    public bool IsVisible { get; private set; }
    public bool HasObservation => last.HasValue;

    public void Observe(SamplePoint position, double nowSeconds)
    {
        if (!double.IsFinite(position.X) || !double.IsFinite(position.Y) ||
            !double.IsFinite(position.Z) || !double.IsFinite(nowSeconds) ||
            (last is { } previous && nowSeconds < previous.AtSeconds))
            throw new ArgumentOutOfRangeException(nameof(nowSeconds), "Invalid position, time or observation order.");
        last = new TargetObservation(position, nowSeconds);
        IsVisible = true;
    }

    // A lost raycast is not a new observation. Do not chase a hidden live Transform.
    public void LoseSight() => IsVisible = false;

    public TargetObservation? ReadFresh(double nowSeconds, double maximumAgeSeconds)
    {
        if (!double.IsFinite(nowSeconds) || !double.IsFinite(maximumAgeSeconds) || maximumAgeSeconds < 0)
            throw new ArgumentOutOfRangeException(nameof(nowSeconds));
        return last is { } value && nowSeconds >= value.AtSeconds &&
               nowSeconds - value.AtSeconds <= maximumAgeSeconds ? value : null;
    }

    public void Clear()
    {
        last = null;
        IsVisible = false;
    }
}

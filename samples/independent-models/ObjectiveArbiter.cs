// Copyright © 2026 Protosoul Studios. All rights reserved.
using System;

namespace OmegaV.Showcase.Models;

// Simplified independent policy example, not a replica of the production objective brain.
public enum Objective { Search, Acquire, Deliver, Engage }
public readonly record struct ObjectiveContext(bool HoldingShard, bool VisibleShard,
    bool RememberedShard, bool ConfirmedThreat, bool UrgentThreat, bool CombatAllowed);
public readonly record struct ObjectiveChoice(Objective Objective, int Score);

public sealed class ObjectiveArbiter
{
    private readonly double minimumCommitmentSeconds;
    private readonly int switchScoreAdvantage;
    private ObjectiveChoice current;
    private double chosenAt;
    private bool hasChoice;
    private double previousTime = double.NegativeInfinity;

    public ObjectiveArbiter(double minimumCommitmentSeconds = 1.0, int switchScoreAdvantage = 10)
    {
        if (!double.IsFinite(minimumCommitmentSeconds) || minimumCommitmentSeconds < 0 ||
            switchScoreAdvantage < 0)
            throw new ArgumentOutOfRangeException(nameof(minimumCommitmentSeconds));
        this.minimumCommitmentSeconds = minimumCommitmentSeconds;
        this.switchScoreAdvantage = switchScoreAdvantage;
    }

    public ObjectiveChoice Choose(ObjectiveContext context, double nowSeconds)
    {
        if (!double.IsFinite(nowSeconds) || nowSeconds < previousTime)
            throw new ArgumentOutOfRangeException(nameof(nowSeconds));
        previousTime = nowSeconds;
        var proposed = Rank(context);

        bool currentStillValid = hasChoice && IsEligible(current.Objective, context);
        if (!currentStillValid || (proposed.Objective == Objective.Engage &&
            context.UrgentThreat && context.CombatAllowed) ||
            (proposed.Objective != current.Objective &&
             nowSeconds - chosenAt >= minimumCommitmentSeconds &&
             proposed.Score >= Score(current.Objective, context) + switchScoreAdvantage))
        {
            current = proposed;
            chosenAt = nowSeconds;
            hasChoice = true;
        }
        // Reflect the latest score for the still-selected objective.
        current = new ObjectiveChoice(current.Objective, Score(current.Objective, context));
        return current;
    }

    public void Reset()
    {
        hasChoice = false;
        previousTime = double.NegativeInfinity;
        chosenAt = 0;
        current = default;
    }

    private static ObjectiveChoice Rank(ObjectiveContext c)
    {
        if (c.CombatAllowed && c.UrgentThreat) return new(Objective.Engage, 130);
        if (c.HoldingShard) return new(Objective.Deliver, 95);
        if (c.CombatAllowed && c.ConfirmedThreat) return new(Objective.Engage, 85);
        if (c.VisibleShard) return new(Objective.Acquire, 75);
        if (c.RememberedShard) return new(Objective.Acquire, 55);
        return new(Objective.Search, 30);
    }

    private static bool IsEligible(Objective o, ObjectiveContext c) => o switch
    {
        Objective.Search => true,
        Objective.Acquire => !c.HoldingShard && (c.VisibleShard || c.RememberedShard),
        Objective.Deliver => c.HoldingShard,
        Objective.Engage => c.CombatAllowed && (c.ConfirmedThreat || c.UrgentThreat),
        _ => false
    };

    private static int Score(Objective o, ObjectiveContext c) => o switch
    {
        Objective.Search => 30,
        Objective.Acquire => c.VisibleShard ? 75 : c.RememberedShard ? 55 : 0,
        Objective.Deliver => c.HoldingShard ? 95 : 0,
        Objective.Engage => c.UrgentThreat ? 130 : c.ConfirmedThreat ? 85 : 0,
        _ => 0
    };
}

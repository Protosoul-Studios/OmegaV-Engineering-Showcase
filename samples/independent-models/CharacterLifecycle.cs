// Copyright © 2026 Protosoul Studios. All rights reserved.
using System;

namespace OmegaV.Showcase.Models;

// Tiny deterministic lifecycle, not Unity GameObject, coroutine or animation integration.
public enum ActorPhase { Alive, Dead, Respawning }

public sealed class CharacterLifecycle
{
    public ActorPhase Phase { get; private set; } = ActorPhase.Alive;
    public int Generation { get; private set; }
    public bool MovementRequested { get; private set; }
    public string? TargetId { get; private set; }

    public bool TryRequestMovement()
    {
        if (Phase != ActorPhase.Alive) return false;
        MovementRequested = true;
        return true;
    }

    public bool TrySelectTarget(string targetId)
    {
        if (Phase != ActorPhase.Alive || string.IsNullOrWhiteSpace(targetId)) return false;
        TargetId = targetId;
        return true;
    }

    public bool TryBeginDeath()
    {
        if (Phase != ActorPhase.Alive) return false;
        Generation++;
        Phase = ActorPhase.Dead;
        MovementRequested = false;
        TargetId = null;
        return true;
    }

    public bool TryBeginRespawn(int generation)
    {
        if (Phase != ActorPhase.Dead || generation != Generation) return false;
        Phase = ActorPhase.Respawning;
        return true;
    }

    public bool TryFinishRespawn(int generation)
    {
        if (Phase != ActorPhase.Respawning || generation != Generation) return false;
        Phase = ActorPhase.Alive;
        return true;
    }
}

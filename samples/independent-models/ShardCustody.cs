// Copyright © 2026 Protosoul Studios. All rights reserved.
using System;

namespace OmegaV.Showcase.Models;

// Independent state-transition ledger; not a network authority or production shard component.
public sealed class ShardCustody
{
    public string? HolderId { get; private set; }
    public int Revision { get; private set; }

    public bool TryCollect(string newHolderId, int expectedRevision)
    {
        if (expectedRevision != Revision || HolderId != null || string.IsNullOrWhiteSpace(newHolderId))
            return false;
        HolderId = newHolderId;
        Revision++;
        return true;
    }

    public bool TrySteal(string? previousHolderId, string? newHolderId, int expectedRevision)
    {
        if (expectedRevision != Revision || HolderId == null ||
            string.IsNullOrWhiteSpace(previousHolderId) || HolderId != previousHolderId ||
            string.IsNullOrWhiteSpace(newHolderId) || previousHolderId == newHolderId)
            return false;
        HolderId = newHolderId;
        Revision++;
        return true;
    }

    public bool TryDrop(string holderId, int expectedRevision)
    {
        if (expectedRevision != Revision || HolderId == null || HolderId != holderId) return false;
        HolderId = null;
        Revision++;
        return true;
    }
}

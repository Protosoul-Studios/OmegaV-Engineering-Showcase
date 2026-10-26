// Copyright © 2026 Protosoul Studios. All rights reserved.
// Dependency-free executable contract checks for five independently authored models.
// These checks are NOT Unity tests and do NOT verify the production OmegaV game.
using OmegaV.Showcase.Models;

var total = 0;
void Check(bool condition, string explanation)
{
    if (!condition) throw new InvalidOperationException(explanation);
}
void Test(string label, Action body)
{
    body();
    total++;
    Console.WriteLine($"PASS {total:00} {label}");
}
void Throws<T>(Action body) where T : Exception
{
    try { body(); }
    catch (T) { return; }
    throw new InvalidOperationException($"Expected {typeof(T).Name}");
}

var p = new SamplePoint(1, 2, 3);
var q = new SamplePoint(7, 8, 9);
Test("memory starts without an observation", () => Check(!new ObservationMemory().HasObservation, "start"));
Test("recording observation marks visible", () => { var m = new ObservationMemory(); m.Observe(p, 10); Check(m.IsVisible && m.HasObservation, "observe"); });
Test("LOS loss retains immutable position and time", () => { var m = new ObservationMemory(); m.Observe(p, 10); m.LoseSight(); Check(!m.IsVisible && m.ReadFresh(11, 5)?.Position == p && m.ReadFresh(11, 5)?.AtSeconds == 10, "freeze"); });
Test("new observation replaces prior sample", () => { var m = new ObservationMemory(); m.Observe(p, 10); m.LoseSight(); m.Observe(q, 12); Check(m.IsVisible && m.ReadFresh(13, 5)?.Position == q, "reacquire"); });
Test("memory expiry boundary inclusive", () => { var m = new ObservationMemory(); m.Observe(p, 10); Check(m.ReadFresh(15, 5).HasValue && !m.ReadFresh(15.01, 5).HasValue, "expiry"); });
Test("future reading does not pretend old observation exists", () => { var m = new ObservationMemory(); m.Observe(p, 10); Check(!m.ReadFresh(9, 5).HasValue, "time order"); });
Test("backward observation timestamp rejected", () => { var m = new ObservationMemory(); m.Observe(p, 10); Throws<ArgumentOutOfRangeException>(() => m.Observe(q, 9)); Check(m.ReadFresh(10, 1)?.Position == p, "unchanged"); });
Test("nonfinite observation rejected", () => Throws<ArgumentOutOfRangeException>(() => new ObservationMemory().Observe(new SamplePoint(double.NaN, 0, 0), 1)));
Test("clearing removes prior knowledge", () => { var m = new ObservationMemory(); m.Observe(p, 1); m.Clear(); Check(!m.HasObservation && !m.IsVisible, "clear"); });

var search = new ObjectiveContext(false, false, false, false, false, true);
var visible = new ObjectiveContext(false, true, false, false, false, true);
var remembered = new ObjectiveContext(false, false, true, false, false, true);
var carrying = new ObjectiveContext(true, false, false, false, false, true);
var urgent = new ObjectiveContext(true, false, false, true, true, true);
Test("no candidate chooses search", () => Check(new ObjectiveArbiter().Choose(search, 1).Objective == Objective.Search, "search"));
Test("visible shard chooses acquisition initially", () => Check(new ObjectiveArbiter().Choose(visible, 1).Objective == Objective.Acquire, "acquire"));
Test("remembered shard remains an acquisition candidate", () => Check(new ObjectiveArbiter().Choose(remembered, 1).Objective == Objective.Acquire, "memory"));
Test("carrying shard prioritizes delivery", () => Check(new ObjectiveArbiter().Choose(carrying, 1).Objective == Objective.Deliver, "deliver"));
Test("urgent threat interrupts delivery", () => { var a = new ObjectiveArbiter(10); a.Choose(carrying, 1); Check(a.Choose(urgent, 1.1).Objective == Objective.Engage, "urgent"); });
Test("combat disallowed blocks threat interruption", () => { var c = urgent with { CombatAllowed = false }; Check(new ObjectiveArbiter().Choose(c, 1).Objective == Objective.Deliver, "permission"); });
Test("objective retains commitment against ordinary upgrade", () => { var a = new ObjectiveArbiter(2); a.Choose(search, 1); Check(a.Choose(visible, 2).Objective == Objective.Search, "commitment"); });
Test("objective may switch after commitment", () => { var a = new ObjectiveArbiter(2); a.Choose(search, 1); Check(a.Choose(visible, 3).Objective == Objective.Acquire, "switch"); });
Test("invalid current objective is discarded immediately", () => { var a = new ObjectiveArbiter(10); a.Choose(visible, 1); Check(a.Choose(search, 1.1).Objective == Objective.Search, "invalidated"); });
Test("nonmonotonic policy time rejected", () => { var a = new ObjectiveArbiter(); a.Choose(search, 2); Throws<ArgumentOutOfRangeException>(() => a.Choose(search, 1)); });

Test("new actor is alive", () => Check(new CharacterLifecycle().Phase == ActorPhase.Alive, "alive"));
Test("death stops movement and clears target", () => { var l = new CharacterLifecycle(); l.TryRequestMovement(); l.TrySelectTarget("A"); Check(l.TryBeginDeath() && !l.MovementRequested && l.TargetId == null && l.Phase == ActorPhase.Dead, "cleanup"); });
Test("duplicate death request is ignored", () => { var l = new CharacterLifecycle(); l.TryBeginDeath(); Check(!l.TryBeginDeath() && l.Generation == 1, "dedupe"); });
Test("dead actor rejects movement and targeting", () => { var l = new CharacterLifecycle(); l.TryBeginDeath(); Check(!l.TryRequestMovement() && !l.TrySelectTarget("A"), "reject"); });
Test("stale respawn generation rejected", () => { var l = new CharacterLifecycle(); l.TryBeginDeath(); Check(!l.TryBeginRespawn(0) && l.Phase == ActorPhase.Dead, "stale"); });
Test("respawn phase requires matching completion", () => { var l = new CharacterLifecycle(); l.TryBeginDeath(); Check(l.TryBeginRespawn(1) && !l.TryFinishRespawn(0) && l.TryFinishRespawn(1) && l.Phase == ActorPhase.Alive, "resume"); });
Test("post-respawn transient state remains clear", () => { var l = new CharacterLifecycle(); l.TrySelectTarget("A"); l.TryRequestMovement(); l.TryBeginDeath(); l.TryBeginRespawn(1); l.TryFinishRespawn(1); Check(l.TargetId == null && !l.MovementRequested && l.Generation == 1, "fresh"); });

Test("healthy far target requests approach", () => Check(RangeSteering.Decide(20, 1, 8, 12, .35, 1) == RangeCommand.Approach, "approach"));
Test("healthy close target requests retreat", () => Check(RangeSteering.Decide(4, 1, 8, 12, .35, 1) == RangeCommand.Retreat, "retreat"));
Test("healthy target in tolerance holds", () => Check(RangeSteering.Decide(8.5, 1, 8, 12, .35, 1) == RangeCommand.Hold, "hold"));
Test("low health uses cautious standoff distance", () => Check(RangeSteering.Decide(10, .2, 8, 12, .35, 1) == RangeCommand.Retreat, "caution"));
Test("range band boundaries hold", () => Check(RangeSteering.Decide(9, 1, 8, 12, .35, 1) == RangeCommand.Hold && RangeSteering.Decide(7, 1, 8, 12, .35, 1) == RangeCommand.Hold, "boundaries"));
Test("nonfinite range rejected", () => Throws<ArgumentOutOfRangeException>(() => RangeSteering.Decide(double.NaN, 1, 8, 12, .35, 1)));
Test("invalid health fraction rejected", () => Throws<ArgumentOutOfRangeException>(() => RangeSteering.Decide(8, 1.1, 8, 12, .35, 1)));

Test("empty shard has no holder", () => Check(new ShardCustody().HolderId == null, "empty"));
Test("collect sets holder and increments revision", () => { var s = new ShardCustody(); Check(s.TryCollect("A", 0) && s.HolderId == "A" && s.Revision == 1, "collect"); });
Test("second collector cannot replace current holder", () => { var s = new ShardCustody(); s.TryCollect("A", 0); Check(!s.TryCollect("B", 1) && s.HolderId == "A", "exclusive"); });
Test("steal transfers to another holder", () => { var s = new ShardCustody(); s.TryCollect("A", 0); Check(s.TrySteal("A", "B", 1) && s.HolderId == "B" && s.Revision == 2, "steal"); });
Test("stale steal attempt cannot change custody", () => { var s = new ShardCustody(); s.TryCollect("A", 0); Check(!s.TrySteal("A", "B", 0) && s.HolderId == "A", "stale"); });
Test("invalid prior holder and self-steal are rejected", () => { var s = new ShardCustody(); Check(!s.TrySteal(null, "B", 0) && !s.TrySteal(" ", "B", 0) && s.Revision == 0 && s.HolderId == null, "unheld"); s.TryCollect("A", 0); Check(!s.TrySteal("C", "B", 1) && !s.TrySteal("A", "A", 1) && !s.TrySteal("A", null, 1) && s.Revision == 1 && s.HolderId == "A", "invalid transfer"); });
Test("holder can drop to unowned state", () => { var s = new ShardCustody(); s.TryCollect("A", 0); Check(s.TryDrop("A", 1) && s.HolderId == null && s.Revision == 2, "drop"); });
Test("wrong holder cannot drop shard", () => { var s = new ShardCustody(); s.TryCollect("A", 0); Check(!s.TryDrop("B", 1) && s.HolderId == "A", "wrong drop"); });
Test("recollection requires updated revision", () => { var s = new ShardCustody(); s.TryCollect("A", 0); s.TryDrop("A", 1); Check(!s.TryCollect("B", 1) && s.TryCollect("B", 2) && s.Revision == 3, "revision"); });
Test("blank holder is rejected", () => Check(!new ShardCustody().TryCollect(" ", 0), "identity"));

Console.WriteLine($"ALL {total} INDEPENDENT MODEL CHECKS PASSED");

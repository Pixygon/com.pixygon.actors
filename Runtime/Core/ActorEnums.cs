namespace Pixygon.Actors {
    // ── Engine-portable core (no UnityEngine). See Pixygon.Actors.Core.asmdef. ──

    /// <summary>Combat/social alignment used for targeting + reactions. Game-specific factions can
    /// layer on top via a faction id; this is the coarse universal axis.</summary>
    public enum Allegiance {
        Neutral,
        Friendly,
        Hostile,
    }

    /// <summary>Lifecycle of an actor.</summary>
    public enum ActorState {
        Inactive, // created, not yet spawned
        Alive,
        Downed,   // incapacitated but not gone (revivable)
        Dead,
    }

    /// <summary>Who drives an actor — the literal "skeleton + brain" seam.
    /// Player = a real human soul; Ai = an NPC; Remote = a networked peer.</summary>
    public enum BrainKind {
        None,
        Player,
        Ai,
        Remote,
    }

    /// <summary>What kind of body an actor wears. Humanoids add a com.pixygon.avatar body;
    /// creatures add a com.pixygon.anima body.</summary>
    public enum BodyKind {
        None,
        Humanoid,
        Creature,
    }
}

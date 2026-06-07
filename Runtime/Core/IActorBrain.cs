namespace Pixygon.Actors {
    /// <summary>
    /// The seam that decides <b>who drives an actor</b>. Swapping the brain swaps a body between a
    /// player-character (a real soul), an NPC (AI), or a networked peer — without the actor's stats,
    /// body, or world presence changing. Engine-portable (no UnityEngine).
    /// </summary>
    public interface IActorBrain {
        BrainKind Kind { get; }

        /// <summary>Called when this brain takes control of an actor.</summary>
        void Attach(ActorCore actor);

        /// <summary>Called when this brain releases control.</summary>
        void Detach();

        /// <summary>Per-frame think step (movement intent, AI decisions, input read…).</summary>
        void Tick(float deltaTime);
    }
}

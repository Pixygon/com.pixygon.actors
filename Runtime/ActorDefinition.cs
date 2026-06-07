using System.Collections.Generic;
using Pixygon.ID;
using Pixygon.Stats;
using UnityEngine;

namespace Pixygon.Actors {
    /// <summary>
    /// Authoring template for an actor: its base stats, allegiance, default brain, and body kind.
    /// <see cref="CreateCore"/> builds the engine-portable <see cref="ActorCore"/> a spawned actor runs on.
    /// </summary>
    [CreateAssetMenu(menuName = "Pixygon/Actors/Actor Definition", fileName = "Actor")]
    public class ActorDefinition : IdObject {
        public string _displayName;
        public Allegiance _allegiance = Allegiance.Neutral;
        public BrainKind _defaultBrain = BrainKind.Ai;
        public BodyKind _bodyKind = BodyKind.Humanoid;

        [System.Serializable]
        public struct BaseStat {
            public StatDefinition stat;
            public float baseValue;
        }

        [Tooltip("The stats this actor has, and their starting base values. Only the stats listed here exist on the actor.")]
        public List<BaseStat> _baseStats = new();

        /// <summary>Build a fresh runtime core from this template.</summary>
        public ActorCore CreateCore() {
            var block = new StatBlock();
            foreach (var b in _baseStats)
                if (b.stat != null) block.Add(b.stat.Def, b.baseValue);

            return new ActorCore(block) {
                DefinitionId = GetFullID,
                DisplayName = _displayName,
                Allegiance = _allegiance,
            };
        }
    }
}

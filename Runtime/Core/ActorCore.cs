using System;
using Pixygon.Stats;

namespace Pixygon.Actors {
    /// <summary>
    /// The engine-portable heart of a living entity: identity, allegiance, lifecycle, a
    /// <see cref="StatBlock"/>, and the vitals logic (damage / heal / death). No UnityEngine — this
    /// runs unchanged on the future Pixygon Engine. The Unity <c>Actor</c> MonoBehaviour is just a
    /// shell around one of these.
    /// </summary>
    public sealed class ActorCore {
        public string Id;            // instance id (unique per spawned actor)
        public int DefinitionId;     // the ActorDefinition's stable GetFullID
        public string DisplayName;
        public Allegiance Allegiance;

        public ActorState State { get; private set; } = ActorState.Inactive;
        public readonly StatBlock Stats;

        public event Action<ActorCore> Spawned;
        public event Action<ActorCore, DamageInfo> Damaged;
        public event Action<ActorCore, float> Healed;
        public event Action<ActorCore> Died;

        public ActorCore(StatBlock stats) {
            Stats = stats ?? new StatBlock();
        }

        public bool IsAlive => State == ActorState.Alive || State == ActorState.Downed;

        /// <summary>Current / max HP, read from the shared stat catalog (Stat.Health).</summary>
        public float Health => Stats.Current(Stat.Health);
        public float MaxHealth => Stats.Value(Stat.Health);

        /// <summary>Fill vitals and enter the world.</summary>
        public void Spawn() {
            Stats.FillVitals();
            State = ActorState.Alive;
            Spawned?.Invoke(this);
        }

        public void ApplyDamage(in DamageInfo dmg) {
            if (!IsAlive) return;
            var hp = Stats.Get(Stat.Health);
            if (hp == null) return;
            hp.Current = Math.Max(0f, hp.Current - Math.Max(0f, dmg.Amount));
            Damaged?.Invoke(this, dmg);
            if (hp.Current <= 0f) Kill(dmg.Source);
        }

        public void Heal(float amount) {
            if (!IsAlive || amount <= 0f) return;
            var hp = Stats.Get(Stat.Health);
            if (hp == null) return;
            hp.Current = Math.Min(hp.Value, hp.Current + amount);
            Healed?.Invoke(this, amount);
        }

        public void Kill(ActorCore killer = null) {
            if (State == ActorState.Dead) return;
            State = ActorState.Dead;
            var hp = Stats.Get(Stat.Health);
            if (hp != null) hp.Current = 0f;
            Died?.Invoke(this);
        }
    }

    /// <summary>A unit of damage. <see cref="Element"/> is a stat id from the ElementalPower category
    /// (or 0 for Normal); mitigation/affinity is applied by the game before/within ApplyDamage.</summary>
    public struct DamageInfo {
        public float Amount;
        public int Element;
        public ActorCore Source;
    }
}

using UnityEngine;
using Pixygon.Stats;

namespace Pixygon.Actors {
    /// <summary>
    /// Unity host for a living entity. Owns the engine-portable <see cref="ActorCore"/> (all gameplay
    /// logic), a swappable <see cref="IActorBrain"/> (player / AI / remote), and an optional
    /// <see cref="IBody"/> (Avatar or Anima). This MonoBehaviour is the thin shell — when we move to
    /// the Pixygon Engine, the Core stays and only this wrapper is replaced.
    /// </summary>
    public class Actor : MonoBehaviour {
        [SerializeField] private ActorDefinition _definition;

        public ActorCore Core { get; private set; }
        public IActorBrain Brain { get; private set; }
        public IBody Body { get; private set; }

        private void Awake() {
            Core = _definition != null ? _definition.CreateCore() : new ActorCore(new StatBlock());
            Body = GetComponentInChildren<IBody>(true);
            Body?.OnActorAttached(this);
        }

        /// <summary>Attach (or swap) the brain driving this actor.</summary>
        public void SetBrain(IActorBrain brain) {
            Brain?.Detach();
            Brain = brain;
            Brain?.Attach(Core);
        }

        private void Start() {
            if (Core.State == ActorState.Inactive) Core.Spawn();
        }

        private void Update() {
            // TODO: gate on com.pixygon.core PauseManager once its API is confirmed (avoid guessing here).
            Brain?.Tick(Time.deltaTime);
        }
    }
}

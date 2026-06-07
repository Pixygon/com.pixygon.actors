# Pixygon — Actors

The base for **all living entities** — players, NPCs, enemies, animas. The "**Pixiel
skeleton + brain**" in code: every living thing is an Actor; humanoids add an Avatar
body, creatures add an Anima body.

> **Rule:** all living = **Actor**. Humanoids = Actor + **Avatar**. Other creatures =
> Actor + **Anima**.

## Why it's shaped this way

- **Engine-portable core.** `Pixygon.Actors.Core` (`noEngineReferences: true`) holds
  the whole model + vitals logic — it can't import UnityEngine, so it lifts onto the
  future Pixygon Engine untouched.
- **Thin Unity adapter.** `Pixygon.Actors` adds the `Actor` MonoBehaviour and the
  `ActorDefinition` authoring asset. Swap these off-Unity; the Core stays.
- **Name-based asmdefs, acyclic deps:** `idsystem` + `stats(core)` → `Actors.Core` →
  `Actors`.

```
com.pixygon.actors/
└── Runtime/
    ├── Core/   →  Pixygon.Actors.Core   ⚙️ engine-free
    │   ├── ActorEnums.cs   Allegiance / ActorState / BrainKind / BodyKind
    │   ├── IActorBrain.cs  the player/AI/remote seam
    │   └── ActorCore.cs    identity + StatBlock + vitals (damage/heal/death + events)
    └── (root) →  Pixygon.Actors          Unity adapter
        ├── Actor.cs           MonoBehaviour shell (owns Core + Brain + Body)
        ├── ActorDefinition.cs : IdObject — base stats / allegiance / brain / body
        └── IBody.cs           the Avatar/Anima body seam
```

## Key types

| Type | What it is |
|---|---|
| **`ActorCore`** | The portable model: `Stats` (a `StatBlock`), `Allegiance`, `State`, `Spawn/ApplyDamage/Heal/Kill` + events. Vitals read `Stat.Health`. |
| **`IActorBrain`** | Who drives it: `Player` (a soul), `Ai` (NPC), `Remote` (networked). Swap the brain to swap player↔NPC. |
| **`IBody`** | The visual body — an Avatar (humanoid) or Anima (creature) implements it. |
| **`Actor`** | The Unity MonoBehaviour shell. |
| **`ActorDefinition`** | `: IdObject` template → `CreateCore()` builds the runtime `ActorCore`. |

## Dependencies

`com.pixygon.idsystem`, `com.pixygon.stats`.

## Usage

```csharp
var actor = GetComponent<Actor>();         // Awake built its ActorCore from the definition
actor.SetBrain(new PlayerBrain());         // or AiBrain / RemoteBrain
actor.Core.ApplyDamage(new DamageInfo { Amount = 12, Element = Stat.FirePower });
actor.Core.Died += a => /* drop loot, lineage handoff, recycler… */;
```

## Status

`0.1.0` — MVP scaffold. **Next:** brain implementations (PlayerBrain/AiBrain), pause
integration (via `core.PauseManager`), the `Avatar`/`Anima` bodies that plug into
`IBody`, and migrating a Veilwalkers character type onto it as the proof. **Note:**
`.meta` files generate on first Unity import.

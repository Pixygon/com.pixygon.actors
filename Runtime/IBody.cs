namespace Pixygon.Actors {
    /// <summary>
    /// The visual body an actor wears. A humanoid <c>Avatar</c> (com.pixygon.avatar) or a creature
    /// <c>Anima</c> (com.pixygon.anima) implements this. The Actor never knows which — it just has a
    /// body. (Kept in the Unity layer because bodies are rendered; the actor's *logic* doesn't need it.)
    /// </summary>
    public interface IBody {
        BodyKind Kind { get; }
        void OnActorAttached(Actor actor);
    }
}

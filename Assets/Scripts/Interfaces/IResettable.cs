// SOLID — Single Responsibility + Open/Closed:
// Small focused interface for anything that resets on player death.
// CheckpointManager calls ResetState() on every IResettable in the scene
// without knowing what type of object it is resetting.
// Adding a new resettable object requires zero changes to CheckpointManager.

public interface IResettable
{
    void ResetState();
}
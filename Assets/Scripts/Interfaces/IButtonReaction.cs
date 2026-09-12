// DESIGN PATTERN: Strategy
// Small focused interface for anything that reacts to a button press.
// PuzzleButton fires its UnityEvent — any object implementing this
// defines its own independent reaction without PuzzleButton knowing what it is.

public interface IButtonReaction
{
    void OnButtonActivated();
}
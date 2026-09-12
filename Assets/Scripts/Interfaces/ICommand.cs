// DESIGN PATTERN: Command
// Base interface for all commands in the game.
// Every action that can be encapsulated, queued, or replayed implements this.
// ToyGun creates commands — commands execute themselves.
// The weapon never knows what the command does internally.

public interface ICommand
{
    void Execute();
}
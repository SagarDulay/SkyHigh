// SOLID — Interface Segregation + Dependency Inversion:
// Small focused interface for anything the projectile can hit.
// Dart depends only on this abstraction — never on concrete types like Door or MovingPlatform.
// Any object that should respond to a projectile hit implements this interface.

public interface IActivatable
{
    void Activate();
}
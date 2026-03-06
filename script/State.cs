using Godot;

public partial class State : Node
{
    // node references
    public StateMachine stateMachine;
    public Player player;

    // state lifecycle functions
    public virtual void Enter()
    {
    }

    public virtual void Exit()
    {
    }

    // process functions
    public virtual void Update(double delta)
    {
    }

    public virtual void PhysicsUpdate(double delta)
    {
    }
}
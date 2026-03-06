using Godot;

public partial class StateMachine : Node
{
    // state variables
    [Export] public State initialState;
    public State currentState;

    // initialization functions
    public override void _Ready()
    {
        foreach (Node child in GetChildren())
        {
            if (child is State stateChild)
            {
                stateChild.stateMachine = this;
                stateChild.player = Owner as Player;
            }
        }

        if (initialState != null)
        {
            initialState.Enter();
            currentState = initialState;
        }
    }

    // process functions
    public override void _Process(double delta)
    {
        if (currentState != null)
        {
            currentState.Update(delta);
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        if (currentState != null)
        {
            currentState.PhysicsUpdate(delta);
        }
    }

    // state transition functions
    public void TransitionTo(string stateName)
    {
        State nextState = GetNodeOrNull<State>(stateName);
        if (nextState != null)
        {
            currentState.Exit();
            nextState.Enter();
            currentState = nextState;
        }
    }
}
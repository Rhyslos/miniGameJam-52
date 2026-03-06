using Godot;

public partial class Idle : State
{
    // physics update functions
    // physics update functions
    public override void PhysicsUpdate(double delta)
    {
        if (!player.IsOnFloor())
        {
            stateMachine.TransitionTo("Air");
            return;
        }

        if (player.JumpBufferTimer > 0)
        {
            player.JumpBufferTimer = 0;
            player.Velocity = new Vector2(player.Velocity.X, player.JumpVelocity);
            stateMachine.TransitionTo("Air");
            return;
        }

        if (Mathf.Abs(player.MoveDirection) > 0.01f)
        {
            stateMachine.TransitionTo("Run");
            return;
        }

        player.Velocity = new Vector2(Mathf.MoveToward(player.Velocity.X, 0, player.Speed), player.Velocity.Y);
        player.MoveAndSlide();
    }
}
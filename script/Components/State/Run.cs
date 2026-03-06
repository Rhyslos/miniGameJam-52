using Godot;

public partial class Run : State
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

        if (Mathf.IsZeroApprox(player.MoveDirection))
        {
            stateMachine.TransitionTo("Idle");
            return;
        }

        player.Velocity = new Vector2(player.MoveDirection * player.Speed, player.Velocity.Y);
        player.MoveAndSlide();
    }
}
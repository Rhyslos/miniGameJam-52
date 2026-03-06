using Godot;

public partial class Air : State
{
    // state lifecycle functions
    public override void Enter()
    {
        if (player.CanParkour)
        {
            player.SetColor(Colors.White);
        }
        else
        {
            player.SetColor(Colors.DarkGray);
        }
    }

    // physics update functions
    public override void PhysicsUpdate(double delta)
    {
        if (player.IsOnFloor())
        {
            player.CanParkour = true;
            
            if (player.JumpBufferTimer > 0)
            {
                player.JumpBufferTimer = 0;
                player.Velocity = new Vector2(player.Velocity.X, player.JumpVelocity);
            }
            else if (Mathf.IsZeroApprox(player.MoveDirection))
            {
                stateMachine.TransitionTo("Idle");
            }
            else
            {
                stateMachine.TransitionTo("Run");
            }
            return;
        }

        if (player.CanParkour && player.IsOnWall() && player.MoveDirection != 0 && player.MoveDirection == -player.GetWallNormal().X)
        {
            GodotObject currentWall = player.GetWallCollider();
            if (currentWall != null && currentWall != player.LastWallCollider)
            {
                stateMachine.TransitionTo("WallInteract");
                return;
            }
        }

        if (player.CanParkour && player.CoyoteTimer > 0 && player.JumpBufferTimer > 0)
        {
            player.CoyoteTimer = 0;
            player.JumpBufferTimer = 0;
            player.Velocity = new Vector2(player.Velocity.X, player.JumpVelocity);
        }

        player.Velocity = new Vector2(player.Velocity.X, player.Velocity.Y + player.Gravity * (float)delta);

        if (player.WallJumpLockTimer <= 0)
        {
            if (player.MoveDirection != 0)
            {
                player.Velocity = new Vector2(Mathf.MoveToward(player.Velocity.X, player.MoveDirection * player.Speed, player.AirAcceleration * (float)delta), player.Velocity.Y);
            }
            else
            {
                player.Velocity = new Vector2(Mathf.MoveToward(player.Velocity.X, 0, player.AirFriction * (float)delta), player.Velocity.Y);
            }
        }

        player.MoveAndSlide();
    }
}
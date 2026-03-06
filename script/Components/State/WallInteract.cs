using Godot;

public partial class WallInteract : State
{
    // state variables
    private enum SubState { Impact, WallRun, LedgeClimb }
    private SubState _currentSubState;
    private Vector2 _wallNormal;
    private double _impactTimer;
    private double _wallRunTimer;
    private GodotObject _currentWallCollider;

    // state lifecycle functions
    public override void Enter()
    {
        _currentSubState = SubState.Impact;
        _wallNormal = player.GetWallNormal();
        _impactTimer = 0.5;
        _currentWallCollider = player.GetWallCollider();

        player.SetColor(Colors.Orange);
        player.Velocity = new Vector2(0, player.WallImpactUpwardSpeed);

        UpdateRaycasts();
    }

    public override void Exit()
    {
        player.LastWallCollider = _currentWallCollider;
    }

    // physics update functions
    public override void PhysicsUpdate(double delta)
    {
        switch (_currentSubState)
        {
            case SubState.Impact:
                ProcessImpactState(delta);
                break;
            case SubState.WallRun:
                ProcessWallRunState(delta);
                break;
            case SubState.LedgeClimb:
                ProcessLedgeClimbState(delta);
                break;
        }

        player.MoveAndSlide();
    }

    // substate processing functions
    private void ProcessImpactState(double delta)
    {
        if (Input.IsActionJustPressed("jump") || player.JumpBufferTimer > 0)
        {
            player.JumpBufferTimer = 0;
            ExecuteWallJump();
            return;
        }

        _impactTimer -= delta;

        if (_impactTimer <= 0)
        {
            if (player.MoveDirection == -_wallNormal.X)
            {
                _currentSubState = SubState.WallRun;
                _wallRunTimer = player.WallRunDuration;
                player.SetColor(Colors.Red);
            }
            else
            {
                player.CanParkour = false;
                stateMachine.TransitionTo("Air");
            }
        }
        else
        {
            player.Velocity = new Vector2(0, Mathf.MoveToward(player.Velocity.Y, 0, player.Gravity * (float)delta));
        }
    }

    private void ProcessWallRunState(double delta)
    {
        if (Input.IsActionJustPressed("jump") || player.JumpBufferTimer > 0)
        {
            player.JumpBufferTimer = 0;
            ExecuteWallJump();
            return;
        }

        _wallRunTimer -= delta;
        
        float runRatio = (float)(_wallRunTimer / player.WallRunDuration);
        player.Velocity = new Vector2(-_wallNormal.X * 10.0f, player.WallRunSpeed * runRatio);

        UpdateRaycasts();

        if (!player.ShoulderRay.IsColliding() && player.WaistRay.IsColliding())
        {
            _currentSubState = SubState.LedgeClimb;
            player.SetColor(Colors.DarkRed); 
        }
        else if (_wallRunTimer <= 0 || !player.IsOnWall() || player.MoveDirection != -_wallNormal.X)
        {
            player.CanParkour = false;
            stateMachine.TransitionTo("Air");
        }
    }

    private void ProcessLedgeClimbState(double delta)
    {
        player.Velocity = new Vector2(-_wallNormal.X * 200.0f, player.JumpVelocity * 0.8f);

        UpdateRaycasts();

        if (!player.WaistRay.IsColliding())
        {
            stateMachine.TransitionTo("Air");
        }
    }

    // execution functions
    private void ExecuteWallJump()
    {
        player.SetColor(Colors.Yellow);
        player.WallJumpLockTimer = player.WallJumpLockDuration;
        player.Velocity = new Vector2(_wallNormal.X * player.WallJumpPushback, player.JumpVelocity);
        stateMachine.TransitionTo("Air");
    }

    // helper functions
    private void UpdateRaycasts()
    {
        float direction = -_wallNormal.X;
        player.ShoulderRay.TargetPosition = new Vector2(direction * 20.0f, 0);
        player.WaistRay.TargetPosition = new Vector2(direction * 20.0f, 0);
    }
}
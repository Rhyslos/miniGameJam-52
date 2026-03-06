using Godot;

public partial class Player : CharacterBody2D
{
    // physics variables
    [Export] public float Speed = 300.0f;
    [Export] public float JumpVelocity = -400.0f;
    [Export] public float Gravity = 980.0f;
    [Export] public float WallImpactUpwardSpeed = -150.0f;
    [Export] public float WallJumpPushback = 400.0f;
    [Export] public float WallRunSpeed = -400.0f;
    [Export] public double WallRunDuration = 1.5;
    [Export] public float AirAcceleration = 1200.0f;
    [Export] public float AirFriction = 600.0f;

    // timing variables
    [Export] public double CoyoteTimeDuration = 0.15;
    [Export] public double JumpBufferDuration = 0.15;
    [Export] public double WallJumpLockDuration = 0.2;
    public double CoyoteTimer = 0.0;
    public double JumpBufferTimer = 0.0;
    public double WallJumpLockTimer = 0.0;

    // state variables
    public bool CanParkour = true;
    public GodotObject LastWallCollider;

    // input variables
    public float MoveDirection = 0.0f;

    // node references
    [Export] public ColorRect VisualRect;
    [Export] public RayCast2D ShoulderRay;
    [Export] public RayCast2D WaistRay;

    // process functions
    public override void _PhysicsProcess(double delta)
    {
        MoveDirection = Input.GetAxis("walk_left", "walk_right");

        if (Input.IsActionJustPressed("jump"))
        {
            JumpBufferTimer = JumpBufferDuration;
        }
        else if (JumpBufferTimer > 0)
        {
            JumpBufferTimer -= delta;
        }

        if (IsOnFloor())
        {
            CoyoteTimer = CoyoteTimeDuration;
            LastWallCollider = null;
        }
        else if (CoyoteTimer > 0)
        {
            CoyoteTimer -= delta;
        }

        if (WallJumpLockTimer > 0)
        {
            WallJumpLockTimer -= delta;
        }
    }

    // helper functions
    public GodotObject GetWallCollider()
    {
        for (int i = 0; i < GetSlideCollisionCount(); i++)
        {
            KinematicCollision2D collision = GetSlideCollision(i);
            if (Mathf.Abs(collision.GetNormal().X) > 0.9f)
            {
                return collision.GetCollider();
            }
        }
        return null;
    }

    // visual functions
    public void SetColor(Color color)
    {
        if (VisualRect != null)
        {
            VisualRect.Color = color;
        }
    }
}
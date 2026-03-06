using Godot;

[Tool]
public partial class Greybox : Node2D
{
    // node references
    private ColorRect _colorRect;
    private CollisionShape2D _collisionShape;

    // backing fields
    private Vector2 _boxSize = new Vector2(64.0f, 64.0f);
    private Color _boxColor = Colors.Gray;
    private bool _isBreakable = false;

    // exported variables
    [Export]
    public Vector2 BoxSize
    {
        get => _boxSize;
        set
        {
            _boxSize = value;
            UpdateComponents();
        }
    }

    [Export]
    public Color BoxColor
    {
        get => _boxColor;
        set
        {
            _boxColor = value;
            UpdateComponents();
        }
    }

    [Export]
    public bool IsBreakable
    {
        get => _isBreakable;
        set => _isBreakable = value;
    }

    // lifecycle functions
    public override void _Ready()
    {
        _colorRect = GetNodeOrNull<ColorRect>("ColorRect");
        _collisionShape = GetNodeOrNull<CollisionShape2D>("StaticBody2D/CollisionShape2D");

        UpdateComponents();
    }

    // update functions
    private void UpdateComponents()
    {
        if (!IsNodeReady()) return;

        if (_colorRect != null)
        {
            _colorRect.Size = _boxSize;
            _colorRect.Position = -_boxSize / 2.0f;
            _colorRect.Color = _boxColor;
        }

        if (_collisionShape != null)
        {
            if (_collisionShape.Shape is not RectangleShape2D rectShape)
            {
                rectShape = new RectangleShape2D();
                _collisionShape.Shape = rectShape;
            }
            rectShape.Size = _boxSize;
        }
    }
}
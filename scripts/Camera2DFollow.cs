using Godot;

public partial class Camera2DFollow : Camera2D
{
	[Export]
	private NodePath _followNode;

	private CharacterBody2D _probe;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_probe = GetNode<CharacterBody2D>(_followNode);

		if (_probe == null)
		{
			GD.PushError($"Cannot find node at path: {_followNode}");
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		Position = _probe.Position;
	}
}

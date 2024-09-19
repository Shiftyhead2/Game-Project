using Godot;

public partial class Camera2DFollow : Camera2D
{

	[Export]
	private Probe _probe;

	public override void _Ready()
	{
		if (_probe == null)
		{
			_probe = GameManager.instance?.GetProbe();
		}
	}

	public override void _Process(double delta)
	{
		if (_probe != null)
		{
			Position = _probe.GlobalPosition;
		}
	}
}

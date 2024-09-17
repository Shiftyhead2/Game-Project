using Godot;

public partial class GameManager : Node
{
	public static GameManager instance { get; private set; }

	private static Probe _probe;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (instance != null)
		{
			QueueFree();
			return;
		}
		instance = this;
	}


	public void SetProbe(Probe node)
	{
		_probe = node;
	}

	public Probe GetProbe()
	{
		return _probe;
	}
}

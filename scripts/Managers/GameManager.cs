using Godot;

public partial class GameManager : Node
{
	public static GameManager Instance { get; private set; }

	private static Probe _probe;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (Instance != null)
		{
			QueueFree();
			return;
		}
		Instance = this;
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

using Godot;

public partial class GameManager : Node
{
	public static GameManager instance { get; private set; }

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

	public override void _ExitTree()
	{
	}


}

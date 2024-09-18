using System.Collections.Generic;
using Godot;

public partial class SignalSpawner : Node
{
	//I am using Godot arrays instead of C# lists here because C# lists won't show up in the Godot editor when exported.
	//Godot arrays are very similar to C# lists.
	[Export]
	private Godot.Collections.Array<SignalResource> _signals = new Godot.Collections.Array<SignalResource>();
	[Export]
	private PackedScene _signalScene;

	[Export]
	private int min_X = -5000;
	[Export]
	private int max_X = 5000;

	[Export]
	private int min_Y = -5000;

	[Export]
	private int max_Y = 5000;

	private List<Vector2I> _Positions;

	private RandomNumberGenerator _randomNumberGenerator = new RandomNumberGenerator();

	private Signals spawnedSignal;



	// Called when the node enters the scene tree for the first time.
	public override async void _Ready()
	{
		//Waits until the owner of this node which is the Level Node2D is ready
		//This is to ensure that the signals are actual spawned as children of the Level node.
		await Owner.ToSignal(Owner, Node.SignalName.Ready);
		_randomNumberGenerator.Randomize(); // Ensure random numbers are seeded properly

		SetPositions();
		CleanPositions();
	}

	public override void _EnterTree()
	{
		EventManager.OnDetectSignals += SpawnSignal;
	}

	public override void _ExitTree()
	{
		EventManager.OnDetectSignals -= SpawnSignal;
	}


	private void SetPositions()
	{
		_Positions = new List<Vector2I>();

		for (int x = min_X; x <= max_X; x += 500)
		{
			for (int y = min_Y; y <= max_Y; y += 500)
			{
				_Positions.Add(new Vector2I(x, y));
			}
		}
	}

	private void CleanPositions()
	{
		for (int i = _Positions.Count - 1; i >= 0; i--)
		{
			Vector2I position = _Positions[i];
			if (position.X == 0 && position.Y == 0)
			{
				GD.Print("Cleaning position: " + position + " because it's where the player spawns");
				_Positions.RemoveAt(i);
			}
		}
	}

	private void SpawnSignal()
	{
		if (spawnedSignal == null)
		{
			Signals signalToSpawn = ResourceLoader.Load<PackedScene>(_signalScene.ResourcePath).Instantiate() as Signals;
			Owner.AddChild(signalToSpawn);
			signalToSpawn.SetUpSignal(_signals[0], getSignalSpawnPosition());
			spawnedSignal = signalToSpawn;
		}
		else
		{
			Signals signalToReset = spawnedSignal;
			signalToReset.SetUpSignal(_signals[0], getSignalSpawnPosition());
		}

	}

	public override void _UnhandledInput(InputEvent @event)
	{
#if DEBUG
		if (@event.IsActionPressed("debug_spawn_signals"))
		{
			SpawnSignal();
		}
#endif
	}

	private Vector2I getSignalSpawnPosition()
	{
		if (_Positions.Count == 0)
		{
			GD.PrintErr("No available positions to spawn a signal.");
			return Vector2I.Zero;
		}


		int randomIndex = _randomNumberGenerator.RandiRange(0, _Positions.Count - 1);
		Vector2I spawnPosition = _Positions[randomIndex];

		GD.Print("Spawned position: " + spawnPosition);

		return spawnPosition;
	}
}

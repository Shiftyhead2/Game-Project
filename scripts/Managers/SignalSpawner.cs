using System.Collections.Generic;
using Godot;

public partial class SignalSpawner : Node
{
	//I am using Godot/Redot arrays instead of C# lists here because C# lists won't show up in the Godot/Redot editor when exported.
	//Godot/Redot arrays are very similar to C# lists.
	[Export]
	private Godot.Collections.Array<SignalResource> _signals = new Godot.Collections.Array<SignalResource>();
	[Export]
	private PackedScene _signalScene;

	[Export]
	private int _minX = -5000;
	[Export]
	private int _maxX = 5000;

	[Export]
	private int _minY = -5000;

	[Export]
	private int _maxY = 5000;

	private List<Vector2I> _positions;
	

	private Signals _spawnedSignal;



	// Called when the node enters the scene tree for the first time.
	public override async void _Ready()
	{
		//Waits until the owner of this node which is the Level Node2D is ready
		//This is to ensure that the signals are actual spawned as children of the Level node.
		await Owner.ToSignal(Owner, Node.SignalName.Ready);

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
		_positions = new List<Vector2I>();

		for (int x = _minX; x <= _maxX; x += 500)
		{
			for (int y = _minY; y <= _maxY; y += 500)
			{
				_positions.Add(new Vector2I(x, y));
			}
		}
	}

	private void CleanPositions()
	{
		for (int i = _positions.Count - 1; i >= 0; i--)
		{
			Vector2I position = _positions[i];
			if (position is { X: 0, Y: 0 })
			{
				Logger.LogMessage(Name,$"Removing position (0,0) because it's where the player spawns");
				_positions.RemoveAt(i);
			}
		}
	}

	private void SpawnSignal()
	{
		if (_spawnedSignal == null)
		{
			var signalToSpawn = ResourceLoader.Load<PackedScene>(_signalScene.ResourcePath).Instantiate() as Signals;
			Owner.AddChild(signalToSpawn);
			signalToSpawn?.SetUpSignal(_signals[0], GetSignalSpawnPosition());
			_spawnedSignal = signalToSpawn;
		}
		else
		{
			var signalToReset = _spawnedSignal;
			signalToReset.SetUpSignal(_signals[0], GetSignalSpawnPosition());
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
	
	
	private Vector2I GetSignalSpawnPosition()
	{
		if (_positions.Count == 0)
		{
			Logger.LogError(Name,$"No positions could be found!");
			return Vector2I.Zero;
		}


		int randomIndex = RandomNumberGeneratorManager.Instance.GetRandomNumber(0,_positions.Count - 1);
		Vector2I spawnPosition = _positions[randomIndex];

		Logger.LogMessage(Name,$"Spawning signal at {spawnPosition}");

		return spawnPosition;
	}
}

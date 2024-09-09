using Godot;
using System.Collections.Generic;

public partial class SignalSpawner : Node
{
	[Export]
	private Godot.Collections.Array<SignalResource> _signals = new Godot.Collections.Array<SignalResource>();
	[Export]
	private PackedScene _signalScene;

	private const int MAX_ATTEMPTS = 100;
	private const float MIN_DISTANCE_BETWEEN_SIGNALS = 50.0f;
	private const float MIN_DISTANCE_FROM_POINT = 100.0f;
	private List<Vector2> spawnedPositions = new List<Vector2>();
	private Vector2 exclusionPoint = new Vector2(-500, 500);

	private int numberOfSignalsToSpawn = 10;



	// Called when the node enters the scene tree for the first time.
	public override async void _Ready()
	{
		await Owner.ToSignal(Owner, Node.SignalName.Ready);
		SpawnSignals();
	}

	private void SpawnSignals()
	{
		for (int i = 0; i < numberOfSignalsToSpawn; i++)
		{
			Signals signalToSpawn = ResourceLoader.Load<PackedScene>(_signalScene.ResourcePath).Instantiate() as Signals;
			Owner.AddChild(signalToSpawn);
			signalToSpawn.SetUpPlanet(_signals[0], getSignalSpawnPosition(_signals[0]));
		}
	}

	private Vector2 getSignalSpawnPosition(SignalResource signal)
	{
		RandomNumberGenerator randomNumberGenerator = new RandomNumberGenerator();
		randomNumberGenerator.Randomize(); // Ensure random numbers are seeded properly

		Vector2 spawnPosition;
		bool isValidPosition;
		int attemptCount = 0;

		do
		{
			// Generate a random position within the spawn bounds
			float x = randomNumberGenerator.RandfRange(signal.SpawnPositions[0].X, signal.SpawnPositions[1].X);
			float y = randomNumberGenerator.RandfRange(signal.SpawnPositions[0].Y, signal.SpawnPositions[1].Y);
			spawnPosition = new Vector2(x, y);

			// Initially assume it's valid
			isValidPosition = true;

			// Check if the position is too close to the exclusion point (-500, 500)
			if (spawnPosition.DistanceTo(exclusionPoint) < MIN_DISTANCE_FROM_POINT)
			{
				isValidPosition = false;
				GD.Print("Position too close to exclusion point: " + spawnPosition);
				continue; // Skip this position and try again
			}

			// Check if the position is too close to previously spawned signals
			foreach (var prevPosition in spawnedPositions)
			{
				if (spawnPosition.DistanceTo(prevPosition) < MIN_DISTANCE_BETWEEN_SIGNALS)
				{
					isValidPosition = false;
					GD.Print("Position too close to another signal: " + spawnPosition);
					break;
				}
			}

			attemptCount++;

		} while (!isValidPosition && attemptCount < MAX_ATTEMPTS); // Stop if a valid position is found or after MAX_ATTEMPTS

		// If no valid position is found after MAX_ATTEMPTS, log an error
		if (!isValidPosition)
		{
			GD.PrintErr("Could not find a valid spawn position after " + MAX_ATTEMPTS + " attempts.");
		}

		// Once we have a valid position, add it to the list of spawned positions
		spawnedPositions.Add(spawnPosition);

		GD.Print("Final spawn position: " + spawnPosition);
		return spawnPosition;
	}
}

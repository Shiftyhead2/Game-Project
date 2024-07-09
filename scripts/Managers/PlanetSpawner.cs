using Godot;

public partial class PlanetSpawner : Node
{
	[Export]
	private Godot.Collections.Array<PlanetResource> _planets = new Godot.Collections.Array<PlanetResource>();
	[Export]
	private PackedScene _planetScene;



	// Called when the node enters the scene tree for the first time.
	public override async void _Ready()
	{
		await Owner.ToSignal(Owner, Node.SignalName.Ready);
		SpawnPlanets();
	}

	private void SpawnPlanets()
	{
		foreach (PlanetResource planet in _planets)
		{
			Planet planetToSpawn = ResourceLoader.Load<PackedScene>(_planetScene.ResourcePath).Instantiate() as Planet;
			Owner.AddChild(planetToSpawn);
			planetToSpawn.SetUpPlanet(planet);
			if (planetToSpawn.IsPlanetEarth())
			{
				EventManager.ExecuteSetPlanetEarth(planetToSpawn);
			}
		}
	}
}

using Godot;

public partial class GameManager : Node
{
	public static GameManager instance { get; private set; }

	[Export]
	public Planet earthPlanet { get; private set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (instance != null)
		{
			QueueFree();
			return;
		}
		instance = this;
		EventManager.OnSetPlanetEarth += SetEarth;
		EventManager.OnScanPlanet += CalculateDistanceBetweenPlanets;
	}

	public override void _ExitTree()
	{
		EventManager.OnSetPlanetEarth -= SetEarth;
		EventManager.OnScanPlanet -= CalculateDistanceBetweenPlanets;
	}


	private void SetEarth(Planet planet)
	{
		earthPlanet = planet;
	}


	private void CalculateDistanceBetweenPlanets(Planet planet)
	{
		if (earthPlanet == null)
		{
			return;
		}

		float distance = earthPlanet.Position.DistanceTo(planet.Position);

		GD.Print($"The distance between {earthPlanet.Name} and {planet.Name} is: {distance}m");
	}


}

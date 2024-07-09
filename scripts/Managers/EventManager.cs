using System;

public static class EventManager
{
	public static Action<Planet> OnSetPlanetEarth;

	public static Action<Planet> OnScanPlanet;

	public static void ExecuteSetPlanetEarth(Planet planet = null)
	{
		OnSetPlanetEarth?.Invoke(planet);
	}

	public static void ExecuteOnScanPlanet(Planet planet = null)
	{
		OnScanPlanet?.Invoke(planet);
	}
}

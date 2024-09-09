using System;

public static class EventManager
{
	public static Action<Signals> OnScanPlanet;

	public static void ExecuteOnScanPlanet(Signals planet = null)
	{
		OnScanPlanet?.Invoke(planet);
	}
}

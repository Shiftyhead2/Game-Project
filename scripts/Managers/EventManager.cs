using System;

public static class EventManager
{
	public static Action<Signals> OnScanPlanet;
	public static Action OnDetectSignals;


	public static void ExecuteOnScanPlanet(Signals planet = null)
	{
		OnScanPlanet?.Invoke(planet);
	}

	public static void ExecuteOnDetectSignals()
	{
		OnDetectSignals?.Invoke();
	}
}

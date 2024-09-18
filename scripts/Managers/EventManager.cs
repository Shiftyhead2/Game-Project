using System;

public static class EventManager
{
	public static Action<Signals> OnScanSignal;
	public static Action OnDetectSignals;


	public static void ExecuteOnScanSignal(Signals signal = null)
	{
		OnScanSignal?.Invoke(signal);
	}

	public static void ExecuteOnDetectSignals()
	{
		OnDetectSignals?.Invoke();
	}
}

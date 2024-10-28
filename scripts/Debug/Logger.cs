using System;
using Godot;

public static class Logger
{
	private static DateTime _dateTime;
	public static void LogMessage(string from, string message)
	{
		GetCurrentTime();
		GD.Print($"{_dateTime.ToLongTimeString()} {from}: {message}");
	}

	public static void LogWarning(string from, string message)
	{
		GetCurrentTime();
		GD.PushWarning($"{_dateTime.ToLongTimeString()} {from}: {message}");
	}

	public static void LogError(string from, string message)
	{
		GetCurrentTime();
		GD.PushError($"{_dateTime.ToLongTimeString()} {from}: {message}");
	}

	private static void GetCurrentTime()
	{
		_dateTime = DateTime.Now;
	}
}

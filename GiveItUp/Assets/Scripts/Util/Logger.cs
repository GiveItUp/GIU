using UnityEngine;
using System.Collections;

public class Logger
{
	private static bool enabledLog = false;
	private static bool enabledError = false;
	private static bool enabledWarning = false;
	
	public static void Log(string msg)
	{
		if (enabledLog)
			Debug.Log(msg);
	}
	
	public static void Warning(string msg)
	{
		if (enabledWarning)
			Debug.LogWarning(msg);
	}
	
	public static void Error(string msg)
	{
		if (enabledError)
			Debug.LogError(msg);
	}
}

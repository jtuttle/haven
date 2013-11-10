using System;
using UnityEngine;

public static class MpUtils
{
	public enum LogLevel {
		Info,
		Warning,
		Error
	}
	
	public static void Log(LogLevel level, string channel, string fmt, params object[] p)
	{
		if (GameConfig.MpLoggingEnabled) {
			string formatted = "[" + channel + "] " + string.Format (fmt, p);
			if (level == LogLevel.Info) {
				Debug.Log(formatted);
			} else if (level == LogLevel.Warning) {
				Debug.LogWarning(formatted);
			} else {
				Debug.LogError (formatted);
			}
		}
	}
}



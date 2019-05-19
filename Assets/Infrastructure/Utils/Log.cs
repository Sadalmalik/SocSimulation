using UnityEngine;

public class Log
{
    public static void Temp(string message, params object[] args)
    {
        Debug.Log("TEMP: "+string.Format(message, args));
    }
    public static void Info(string message, params object[] args)
    {
        Debug.Log("INFO: " + string.Format(message, args));
    }
    public static void Warning(string message, params object[] args)
    {
        Debug.LogWarning("WARNING: " + string.Format(message, args));
    }
    public static void Error(string message, params object[] args)
    {
        Debug.LogError("ERROR: " + string.Format(message, args));
    }
}

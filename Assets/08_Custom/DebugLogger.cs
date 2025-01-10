using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugLogger
{
    static DebugLogger()
    {
#if UNITY_EDITOR
        Debug.unityLogger.logEnabled = true;
#else
        Debug.unityLogger.logEnabled = false;
#endif
    }

    public static void Log(string message) => Debug.Log(message);
}

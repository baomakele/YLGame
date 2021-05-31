using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonLuaTools : MonoBehaviour
{
    public static void Log(string content)
    {
        Debug.Log("Log =" + content);
    }

    public static void LogWarning(string content)
    {
        Debug.LogWarning("LogWarning =" + content);
    }

    public static void LogError(string content)
    {
        Debug.LogError("LogError =" + content);
    }

    public static byte[] CreateBuffer(int bufferSize)
    {
        return new byte[bufferSize];
    }

}

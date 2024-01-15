using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugLogDisplay : MonoBehaviour
{
    public string output = "";
    public string stack = "";

    public void OnEnable()
    {
        Application.logMessageReceived += Handlelog;
    }

    private void OnDisable()
    {
        Application.logMessageReceived -= Handlelog;
    }

    void Handlelog(string logString, string stackTrace, LogType type)
    {
        output = logString;
        stack = stackTrace;
    }

    void OnGUI()
    {
        GUI.Label(new Rect(150, 40, 800,60), output);
        GUI.Label(new Rect(150, 65, 800, 60), stack);
    }
}
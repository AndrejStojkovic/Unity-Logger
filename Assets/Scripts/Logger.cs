using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class Log
{
    public string Message;
    public LogType Type;
    public DateTime Time;
}

public class LoggerColors
{
    private static Color defaultColor = Color.white;
    public static readonly IReadOnlyDictionary<LogType, Color> ColorMap = new Dictionary<LogType, Color>()
    {
        { LogType.Log,  Color.white },
        { LogType.Warning,  Color.yellow },
        { LogType.Error,  Color.red },
        { LogType.Assert,  Color.cyan },
        { LogType.Exception,  Color.blue },
    };

    public static Color GetColor(LogType logType)
    {
        Color color = ColorMap[logType];
        return color != null ? color : defaultColor;
    }
}

public class Logger : MonoBehaviour
{
    private static Logger instance = new Logger();
    public static Logger Instance
    {
        get
        {
            return instance;
        }
    }

    [SerializeField]
    private LoggerType LoggerType = LoggerType.LogAndPrint;

    public int MaxMessages = 16;
    public LoggerScreen LoggerScreen;
    Queue<Log> logQueue = new Queue<Log>();

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if(instance != null)
            Debug.Log("[LOGGER] Logger started and active!");
    }

    public static void Log(string logString) => instance.QueueLog(logString, LogType.Log);
    public static void LogWarning(string logString) => instance.QueueLog(logString, LogType.Warning);
    public static void LogError(string logString) => instance.QueueLog(logString, LogType.Error);
    public static void Assert(bool condition,string logString) => instance.QueueAssert(condition, logString, LogType.Assert);

    private void QueueLog(string logString, LogType logType)
    {
        DebugLog(logString, logType);
        PrintLog(logString, logType);
    }

    private void QueueAssert(bool condition, string logString, LogType logType)
    {
        AssertLog(condition, logString);
        if(!condition) PrintLog(logString, logType);
    }

    private void DebugLog(string logString, LogType logType)
    {
        if(LoggerType != LoggerType.LogOnly && LoggerType != LoggerType.LogAndPrint)
            return;

        switch(logType)
        {
            case LogType.Log:
                Debug.Log(logString);
                break;
            case LogType.Warning:
                Debug.LogWarning(logString);
                break;
            case LogType.Error:
                Debug.LogError(logString);
                break;
        }
    }

    private void AssertLog(bool condition, string logString)
    {
        Debug.Assert(condition, logString);
    }

    private void PrintLog(string logString, LogType logType)
    {
        if(LoggerType != LoggerType.PrintOnly && LoggerType != LoggerType.LogAndPrint)
            return;

        logQueue.Enqueue(new Log()
        {
            Message = $"[{DateTime.Now:HH:mm:ss}] - [{logType}] : {logString}",
            Type = logType,
            Time = DateTime.Now
        });

        if(MaxMessages != -1)
        {
            while(logQueue.Count > MaxMessages)
            {
                logQueue.Dequeue();
            }
        }
    }

    void OnGUI()
    {
        GUILayout.BeginArea(LoggerScreen.GetScreenPosition());
        Log[] logs = logQueue.ToArray();
        for(int i = logs.Length - 1; i >= 0; i--)
        {
            GUIStyle style = new GUIStyle(EditorStyles.label);
            style.normal.textColor = LoggerColors.GetColor(logs[i].Type);
            GUILayout.Label(logs[i].Message, style);
        }
        GUILayout.EndArea();
    }
}

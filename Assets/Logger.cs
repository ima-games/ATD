using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//非编辑模式下，关掉所有Log
#if !UNITY_EDITOR
        Debug.unityLogger.logEnabled = false;
#endif

/// <summary>
/// 单例类：Debug模式下的日志调试
/// </summary>
public class Logger : MonoBehaviour
{
    public bool log_All;
    public bool log_Default;
    public bool log_Individual;
    public bool log_Hatred;
    public bool log_Buff;
    public bool log_Skill;
    public bool log_Data;

    private static Logger instance;

    void Awake()
    {
        instance = this;
    }


    static private string defaultLog = "Default Log：";
    static private string individualLog = "Individual Log：";
    static private string hatredLog = "Hatred Log：";
    static private string buffLog = "Buff Log：";
    static private string skillLog = "Skill Log：";
    static private string dataLog = "Data Log：";

    static public void Log(string content,LogType logType = LogType.Default)
    {
#if UNITY_EDITOR
        switch (logType)
        {
            case LogType.Default:
                content = defaultLog + content;
                if (!instance.log_All && !instance.log_Default) return;
                break;
            case LogType.Individual:
                content = individualLog + content;
                if (!instance.log_All && !instance.log_Individual) return;
                break;
            case LogType.Hatred:
                content = hatredLog + content;
                if (!instance.log_All && !instance.log_Hatred) return;
                break;
            case LogType.Buff:
                content = buffLog + content;
                if (!instance.log_All && !instance.log_Buff) return;
                break;
            case LogType.Skill:
                content = skillLog + content;
                if (!instance.log_All && !instance.log_Skill) return;
                break;
            case LogType.Data:
                content = dataLog + content;
                if (!instance.log_All && !instance.log_Data) return;
                break;
        }
        //日志输出
        Debug.Log(content);
#endif
    }
}

public enum LogType{
    Default,
    Individual,
    Hatred,
    Buff,
    Skill,
    Data
}


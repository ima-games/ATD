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

    private static Logger instance;

    void Awake()
    {
        instance = this;
    }


    private string defaultLog = "Default Log：";
    private string individualLog = "Individual Log：";
    private string hatredLog = "Hatred Log：";
    private string buffLog = "Buff Log：";
    private string skillLog = "Skill Log：";

    static public void Log(string content,LogType logType = LogType.Default)
    {
#if UNITY_EDITOR
        switch (logType)
        {
            case LogType.Default:
                content = instance.defaultLog + content;
                if (!instance.log_All && !instance.log_Default) return;
                break;
            case LogType.Individual:
                content = instance.individualLog + content;
                if (!instance.log_All && !instance.log_Individual) return;
                break;
            case LogType.Hatred:
                content = instance.hatredLog + content;
                if (!instance.log_All && !instance.log_Hatred) return;
                break;
            case LogType.Buff:
                content = instance.buffLog + content;
                if (!instance.log_All && !instance.log_Buff) return;
                break;
            case LogType.Skill:
                content = instance.skillLog + content;
                if (!instance.log_All && !instance.log_Skill) return;
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
    Skill
}


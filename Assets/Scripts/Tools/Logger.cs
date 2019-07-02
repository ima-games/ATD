using System.Collections;
using System.Collections.Generic;
using UnityEngine;

////非编辑模式下，关掉所有Log
//#if !UNITY_EDITOR
//        Debug.unityLogger.logEnabled = false;
//#endif

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
    public bool log_Tower;
    public bool log_Monster;
    public bool log_AI;

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
    static private string towerLog = "Tower Log：";
    static private string monsterLog = "Monster Log：";
    static private string AILog = "AI Log：";

    static public void Log(string content,LogType logType = LogType.Default)
    {
#if UNITY_EDITOR
        switch (logType)
        {
            case LogType.Default:
                if (!instance.log_All && !instance.log_Default) return;
                content = defaultLog + content;
                break;
            case LogType.Individual:
                if (!instance.log_All && !instance.log_Individual) return;
                content = individualLog + content;
                break;
            case LogType.Hatred:
                if (!instance.log_All && !instance.log_Hatred) return;
                content = hatredLog + content;
                break;
            case LogType.Buff:
                if (!instance.log_All && !instance.log_Buff) return;
                content = buffLog + content;
                break;
            case LogType.Skill:
                if (!instance.log_All && !instance.log_Skill) return;
                content = skillLog + content;
                break;
            case LogType.Data:
                if (!instance.log_All && !instance.log_Data) return;
                content = dataLog + content;
                break;
            case LogType.Tower:
                if (!instance.log_All && !instance.log_Tower) return;
                content = towerLog + content;
                break;
            case LogType.Monster:
                if (!instance.log_All && !instance.log_Monster) return;
                content = monsterLog + content;
                break;
            case LogType.AI:
                if (!instance.log_All && !instance.log_AI) return;
                content = AILog + content;
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
    Data,
    Tower,
    Monster,
    AI
}


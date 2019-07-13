using UnityEngine;

/// <summary>
/// AOE技能
/// </summary>
public class AOESkill : ISkill
{
    public int buffID;         //目的Buff
    public float range = 0.0f; //范围

    public AOESkill(int buffID,float range = 0.0f)
    {
    }

    public float GetColdTimePercent()
    {
        throw new System.NotImplementedException();
    }

    public void InitSkill(Individual master)
    {

    }

    public bool IsColdTimeEnd()
    {
        throw new System.NotImplementedException();
    }

    public void ReleaseSkill(Individual master)
    {

    }

    public void UpdateSkill(Individual master)
    {

    }
}
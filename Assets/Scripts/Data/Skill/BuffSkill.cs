using UnityEngine;
/// <summary>
/// 被动Buff技能
/// </summary>
public class BuffSkill : ISkill
{
    public Buff buff;   //Buff对象
    public bool isAura = true; //光环
    public float range = 0.0f; //范围

    public void InitSkill(GameObject master)
    {

    }

    public void ReleaseSkill(GameObject master)
    {
        //DO NOTHING
    }
}

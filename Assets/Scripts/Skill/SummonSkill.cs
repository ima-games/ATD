using UnityEngine;

/// <summary>
/// 召唤性技能
/// </summary>
public class SummonSkill : ISkill
{
    public GameObject summonObject;//召唤物

    public float GetColdTimePercent()
    {
        throw new System.NotImplementedException();
    }

    public void InitSkill(Individual master)
    {
        //DO NOTHING
    }

    public bool IsColdTimeEnd()
    {
        throw new System.NotImplementedException();
    }

    public void ReleaseSkill(Individual master)
    {
        //在施放者前方2单位距离的位置召唤物体
        GameObject.Instantiate(summonObject,master.transform.position + master.transform.forward * 2.0f, Quaternion.identity, master.transform.parent);
    }

    public void UpdateSkill(Individual master)
    {

    }
}

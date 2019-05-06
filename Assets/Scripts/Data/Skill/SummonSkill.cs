using UnityEngine;

/// <summary>
/// 召唤性技能
/// </summary>
public class SummonSkill : ISkill
{
    public GameObject summonObject;//召唤物

    public void InitSkill(GameObject master)
    {
        //DO NOTHING
    }

    public void ReleaseSkill(GameObject master)
    {
        //在施放者前方2单位距离的位置召唤物体
        GameObject.Instantiate(summonObject,master.transform.position + master.transform.forward * 2.0f, Quaternion.identity, master.transform.parent);
    }


}

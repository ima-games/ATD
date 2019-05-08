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
        this.buffID = buffID;
        this.range = range;
    }

    public void InitSkill(GameObject master)
    {

    }

    public void ReleaseSkill(GameObject master)
    {
        foreach (var individual in LogicManager.AliveIndividualList)
        {
            //在光环范围内
            if ((individual.transform.position - master.transform.position).sqrMagnitude < range * range)
            {
                master.GetComponent<MessageSystem>().SendMessage(2, individual.ID, buffID);
            }
        }
    }

    public void UpdateSkill(GameObject master)
    {

    }
}
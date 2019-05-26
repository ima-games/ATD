using UnityEngine;
/// <summary>
/// 被动Buff技能
/// </summary>
public class BuffSkill : ISkill
{
    public int buffID;         //目的Buff
    public bool isAura = true; //光环
    public float range = 0.0f; //范围

    public BuffSkill(int buffID,bool isAura = true, float range = 0.0f)
    {
        this.buffID = buffID;
        this.isAura = isAura;
        this.range = range;
    }

    public void InitSkill(GameObject master)
    {
        if (!isAura)
        {
            var individual = master.GetComponent<Individual>();
            master.GetComponent<MessageSystem>().SendMessage(2, individual.ID,buffID);
        }
    }

    public void ReleaseSkill(GameObject master)
    {
        //DO NOTHING
    }

    public void UpdateSkill(GameObject master)
    {
        if (isAura)
        {
            foreach(var individual in LogicManager.AliveIndividualList)
            {
                //在光环范围内
                if((individual.transform.position - master.transform.position).sqrMagnitude < range* range)
                {
                    master.GetComponent<MessageSystem>().SendMessage(2,individual.ID, buffID);
                }
            }
        }
    }
}

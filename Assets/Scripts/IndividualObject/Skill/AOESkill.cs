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

    public void InitSkill(Individual master)
    {

    }

    public void ReleaseSkill(Individual master)
    {
        Factory.TraversalIndividualsInCircle(
            (individual) => { master.GetComponent<MessageSystem>().SendMessage(2, individual.ID, buffID); }
            , master.transform.position, range);
    }

    public void UpdateSkill(Individual master)
    {

    }
}
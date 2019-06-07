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

    public void InitSkill(Individual master)
    {
        if (!isAura)
        {
            var individual = master.GetComponent<Individual>();
            master.GetComponent<MessageSystem>().SendMessage(2, individual.ID,buffID);
        }
    }

    public void ReleaseSkill(Individual master)
    {
        //DO NOTHING
    }

    public void UpdateSkill(Individual master)
    {
        if (isAura)
        {
            Factory.TraversalIndividualsInCircle(
                (individual) => { master.GetComponent<MessageSystem>().SendMessage(2, individual.ID, buffID); }
                , master.transform.position, range);
        }
    }
}

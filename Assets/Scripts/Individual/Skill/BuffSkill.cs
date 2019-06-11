using UnityEngine;
/// <summary>
/// 被动Buff技能
/// </summary>
public class BuffSkill : ISkill
{
    public int buffID;               //目的Buff
    public bool isAura = true;       //光环
    public bool releasable = true;   //是否主动释放
    public float range = 0.01f;      //范围

    public BuffSkill(int buffID,bool releasable = true,bool isAura = true, float range = 0.01f)
    {
        this.buffID = buffID;
        this.isAura = isAura;
        this.range = range;
        this.releasable = releasable;
    }

    public void InitSkill(Individual master)
    {
        if (!releasable && !isAura)
        {
            var individual = master.GetComponent<Individual>();
            master.GetComponent<MessageSystem>().SendMessage(2, individual.ID,buffID);
        }
    }

    public void ReleaseSkill(Individual master)
    {
        if (releasable)
        {
            Factory.TraversalIndividualsInCircle(
                (individual) => { master.GetComponent<MessageSystem>().SendMessage(2, individual.ID, buffID); }
                , master.transform.position, range);
        }
    }

    public void UpdateSkill(Individual master)
    {
        if (!releasable && isAura)
        {
            Factory.TraversalIndividualsInCircle(
                (individual) => { master.GetComponent<MessageSystem>().SendMessage(2, individual.ID, buffID); }
                , master.transform.position, range);
        }
    }
}

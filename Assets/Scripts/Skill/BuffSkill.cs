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

    private float coldTime = 5.0f;  //冷却时间
    private float timer = 5.0f;     //冷却计时

    public BuffSkill(int buffID,bool releasable = true,bool isAura = true, float range = 0.01f)
    {
        this.buffID = buffID;
        this.isAura = isAura;
        this.range = range;
        this.releasable = releasable;
    }

    public float GetColdTimePercent()
    {
        if (!releasable) return 1.0f;

        return timer / coldTime;
    }

    public void InitSkill(Individual master)
    {

        if (!releasable && !isAura)
        {
            var individual = master.GetComponent<Individual>();
            master.GetComponent<MessageSystem>().SendMessage(2, individual.ID,buffID);
        }
    }

    public bool IsColdTimeEnd()
    {
        return timer > coldTime;
    }

    public void ReleaseSkill(Individual master)
    {
        if (releasable && IsColdTimeEnd())
        {
            timer = 0.0f;

            Factory.TraversalIndividualsInCircle(
                (individual) => { master.GetComponent<MessageSystem>().SendMessage(2, individual.ID, buffID); }
                , master.transform.position, range);
        }
    }

    public void UpdateSkill(Individual master)
    {
        //增加计时
        timer =Mathf.Min(timer+Time.deltaTime, coldTime+0.1f);

        if (!releasable && isAura)
        {
            Factory.TraversalIndividualsInCircle(
                (individual) => { master.GetComponent<MessageSystem>().SendMessage(2, individual.ID, buffID); }
                , master.transform.position, range);
        }
    }
}

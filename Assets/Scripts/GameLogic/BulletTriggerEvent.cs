using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO!!
public class BulletTriggerEvent : MonoBehaviour
{
    //发射炮弹的塔
    public Individual tower;

    //可能的炮弹添加BUFF
    public int buffID = 0;

    //是否范围性
    public bool ifRange = false;
    //范围大小
    public float range = 3.0f;

    //死亡产生的特效对象（该对象会自动删除）
    public GameObject dieEffect;

    private void OnTriggerEnter(Collider collison)
    {
        var collisonObject = collison.gameObject;

        //个体子弹
        if (!ifRange)
        {
            //子弹打到玩家、非个体单位
            if (collison.name == "PlayerHandle" || LayerMask.LayerToName(collisonObject.layer) != "Individual")
            {
                return;
            }

            //此处应该使用tower的消息系统来发消息
            MessageSystem messageSystem = tower.GetComponent<MessageSystem>();
            Individual otherIndividual = collisonObject.GetComponent<Individual>();

            messageSystem.SendMessage(1, otherIndividual.ID, tower.attack);

            if(buffID != 0)
            {
                messageSystem.SendMessage(2, otherIndividual.ID,buffID);
            }
        }
        //范围子弹 TODO
        else
        {
            Factory.TraversalIndividualsInCircle(
             (individual) => { tower.GetComponent<MessageSystem>().SendMessage(2, individual.ID, buffID); }
                , transform.position, range);
        }



        //特效对象产生
        if (dieEffect)
        GameObject.Instantiate(dieEffect, transform.position,dieEffect.transform.rotation , transform.parent);

        Destroy(gameObject);
    }

    private void OnSpecialEffect()
    {
        
    }
}

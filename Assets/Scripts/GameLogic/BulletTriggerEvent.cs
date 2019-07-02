using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BulletTriggerEvent : MonoBehaviour
{
    public Individual tower;

    public GameObject dieEffect;    //死亡产生的特效对象（该对象会自动删除）

    private void OnTriggerEnter(Collider collison)
    {
        //Individual tower = gameObject.transform.parent.gameObject.GetComponent<Individual>();
        var collisonObject = collison.gameObject;

        //foreach (Transform TowerChild in tower.transform)
        //{
        //    if (TowerChild.gameObject == collisonObject)
        //    {
        //        isCollideTower = true;
        //    }
        //}
        if (collison.name == "Plane")
        {
            //火球落地特效
            Debug.Log("fire ball");
            Destroy(gameObject);
        }
        //子弹打到玩家、非个体单位
        if (collison.name == "PlayerHandle" || LayerMask.LayerToName(collisonObject.layer) != "Individual")
        {
            //Destroy(gameObject);
            return;
        }

        
        
        //此处应该使用tower的消息系统来发消息
        MessageSystem messageSystem = tower.GetComponent<MessageSystem>();
        Individual otherIndividual = collisonObject.GetComponent<Individual>();

        messageSystem.SendMessage(1, otherIndividual.ID,tower.attack);

        //特效对象产生
        GameObject.Instantiate(dieEffect, transform.position,dieEffect.transform.rotation , transform.parent);

        Destroy(gameObject);
    }

    private void OnSpecialEffect()
    {
        
    }
}

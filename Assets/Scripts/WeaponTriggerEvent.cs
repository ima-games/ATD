using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponTriggerEvent : MonoBehaviour
{
    public Individual master;
    private bool attacking =false;

    private void OnTriggerEnter(Collider other)
    {
        var otherGo = other.gameObject;

        Debug.Log("Weapon Trigger Enter！");

        //正在攻击,武器打到的是自己,武器打到的是非个体单位
        if (attacking || otherGo == master.gameObject)
            return;

        MessageSystem messageSystem = master.GetComponent<MessageSystem>();
        Individual otherIndividual = otherGo.GetComponent<Individual>();

        messageSystem.SendMessage(1,otherIndividual.ID,master.attack);
        //0.5秒内才能再触发一次攻击消息，避免一次攻击多次触发
        attacking = true;
        StartCoroutine(AttackTimerCount());
    }


    IEnumerator AttackTimerCount()
    {
        yield return 0.5f;
        attacking = false;
    }

}

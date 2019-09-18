using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 玩家武器触发事件
/// </summary>
public class WeaponTriggerEvent : MonoBehaviour {
    private Individual master;
    private List<GameObject> attackedObjects = new List<GameObject>();
    private bool attackable = false;

    private void Awake()
    {
        master = GetComponentInParent<Individual>();
    }

    private void OnEnable()
    {
        attackable = true;
    }

    private void OnDisable()
    {
        attackable = false;
    }

    private void OnTriggerEnter (Collider other) {
        if (!attackable) return;

        var otherGo = other.gameObject;

        //武器打到的是自己,武器打到的是非个体单位
        if (otherGo == master.gameObject || LayerMask.LayerToName(otherGo.layer) != "Individual" )
            return;

        //已经对某目标触发过攻击，则不再触发
        if (attackedObjects.Contains(otherGo))
            return;
        
        //添加被攻击对象 到 已攻击目标
        attackedObjects.Add(otherGo);

        Individual target = otherGo.GetComponent<Individual>();
        master.GetComponent<BaseIndividualController>().Attack(target);
    }

    public void StartAttack()
    {
        this.enabled = true;
        attackedObjects.Clear();
    }

    public void EndAttack()
    {
        this.enabled = false;
    }
}
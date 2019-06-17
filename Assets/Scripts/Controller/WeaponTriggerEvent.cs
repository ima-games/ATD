using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponTriggerEvent : MonoBehaviour {
    public Individual master;

    //TODO 
    private List<GameObject> attackedObjects = new List<GameObject>();
    bool attackable = false;

    private void Start()
    {
        
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

        if (attackedObjects.Contains(otherGo))
            return;
        
        Logger.Log("Weapon Trigger Attack！", LogType.Individual);

        //添加被攻击对象 到 已攻击目标
        attackedObjects.Add(otherGo);

        Individual target = otherGo.GetComponent<Individual>();
        master.Attack(target);
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
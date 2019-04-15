using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;

public class BuffSystem : MonoBehaviour
{
    List<Buff> myBuffs = new List<Buff>();
    Individual myIndividual;

    private void Awake()
    {
        myIndividual = GetComponent<Individual>();
    }

    private void FixedUpdate()
    {
        BuffUpdate();
    }
    
    /// <summary>
    /// 消息系统接口 传入添加的buff的ID
    /// </summary>
    /// <param name="buffID"></param>
    public void StickBuff(int buffID)
    {
        //1.把buffID加到表里，count+1       ----AddBuff
        //2.把buff数据同步到实体组件        ----BuffSync
        //3.时间到，把buff去掉              ----DestroyBuff
        AddBuff(buffID);
    }

    //添加buff
    private void AddBuff(int buffID)
    {
        Buff buff = new Buff
        {
            ID = buffID,
            time = BuffDataBase.Instance.GetBuffData(buffID).Time
        };
        
        myBuffs.Add(buff);
        BuffSync(buffID);
    }
    
    //属性同步，添加buff
    private void BuffSync(int buffID)
    {
        BuffData BuffAdding = new BuffData();
        BuffAdding = BuffDataBase.Instance.GetBuffData(buffID);
        myIndividual.HealthChange(BuffAdding.HpChange);
        myIndividual.HealthChange(BuffAdding.HpChange_p);
        myIndividual.AttackChange(BuffAdding.AttackChange);
        myIndividual.AttackChange(BuffAdding.AttackChange_p);
        myIndividual.AttackSpeedChange(BuffAdding.AttSpeedChange_p);
        myIndividual.SpeedChange(BuffAdding.SpeedChange_p);
        myIndividual.RecoverRateChange(BuffAdding.HpReturnChange);
        myIndividual.RecoverRateChange(BuffAdding.HpReturnChange_p);
        myIndividual.ReviveCountChange(BuffAdding.AddReviveCount);
        
    }

    //属性同步，移除buff
    private void BuffRemove(int buffID)
    {
        BuffData BuffAdding = new BuffData();
        BuffAdding = BuffDataBase.Instance.GetBuffData(buffID);
        myIndividual.HealthChange(-BuffAdding.HpChange);
        myIndividual.HealthChange(-BuffAdding.HpChange_p);
        myIndividual.AttackChange(-BuffAdding.AttackChange);
        myIndividual.AttackChange(-BuffAdding.AttackChange_p);
        myIndividual.AttackSpeedChange(-BuffAdding.AttSpeedChange_p);
        myIndividual.SpeedChange(-BuffAdding.SpeedChange_p);
        myIndividual.RecoverRateChange(-BuffAdding.HpReturnChange);
        myIndividual.RecoverRateChange(-BuffAdding.HpReturnChange_p);
        myIndividual.ReviveCountChange(-BuffAdding.AddReviveCount);
    }

    //BUFF时间更新
    private void BuffUpdate()
    {
        foreach (Buff temp in myBuffs)
        {
            temp.time -= Time.fixedDeltaTime;
            if (temp.time <= 0)
            {
                myBuffs.Remove(temp);
                BuffRemove(temp.ID);
            }
        }
    }
}

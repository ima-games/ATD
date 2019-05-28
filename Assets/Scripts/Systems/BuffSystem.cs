using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;

/// <summary>
/// Buff系统
/// </summary>
/// Note:现在相同ID的Buff不应该同时存在，而视为叠加次数/刷新时间
public class BuffSystem : MonoBehaviour
{
    Dictionary<int,Buff> myBuffs = new Dictionary<int, Buff>();
    Individual myIndividual;

    //buff栏显示
    [SerializeField] private List<Buff> buffShow = new List<Buff>();

    //初始化状态栏
    [SerializeField] private List<int> initBuff = new List<int>();


    private void Awake()
    {
        myIndividual = GetComponent<Individual>();
    }

    private void Start()
    {
        InitializeBuffList();
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
        Buff buff;
        if (myBuffs.TryGetValue(buffID,out buff))
        {

        }
        else
        {
            buff = new Buff();
            buff.ID = buffID;
        }

        myBuffs.Add(buff);
        //BuffSync(buffID);

        //把buffID加入到buff栏中显示在面板里
        buffShow.Add(buffID);

        Debug.Log("ID为 "+buffID+" 已加入到列表");

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
                //BuffRemove(temp.ID);

                buffShow.Remove(temp.ID);
            }
        }
    }

    private void InitializeBuffList()
    {
        if (initBuff.Count == 0) return;
        //将初始化buff表里的ID依次加入到buff表里
        for(int i=0;i< initBuff.Count; i++)
        {
            AddBuff(initBuff[i]);
        }

        initBuff.Clear();
    }
}

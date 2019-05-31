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
    [SerializeField] private List<int> initBuffs = new List<int>();

    //待删除buff
    private int[] buffsToDelete = new int[64];
    private int buffsToDeleteCount = 0;

    private void Awake()
    {
        myIndividual = GetComponent<Individual>();
    }

    private void Start()
    {
        InitializeBuffList();
    }

    private void Update()
    {

    }

    private void FixedUpdate()
    {
        CleanBuffsToDelete();
        BuffsUpdate();
    }
    
    /// <summary>
    /// 消息系统接口 传入添加的buff的ID
    /// </summary>
    /// <param name="buffID"></param>
    public void StickBuff(int buffID)
    {
        AddBuff(buffID);
    }

    //添加buff
    private void AddBuff(int buffID)
    {
        Buff buff;
        BuffData buffData = BuffDataBase.Instance.GetBuffData(buffID);

        Logger.Log("Buff " + buffID + " 已添加", LogType.Buff);

        //若buff列表没有对应的Buff，则新建一个Buff对象
        if (!myBuffs.TryGetValue(buffID,out buff))
        {
            buff = new Buff();
            myBuffs.Add(buffID, buff);
            //把buffID加入到buff栏中显示在面板里
            buffShow.Add(buff);
            //同步属性：增加BUFF
            AddBuffSync(buffID);
        }
        
        //对该buff属性进行更新
        buff.ID = buffID;
        buff.isTrigger = buffData.isTrigger;
        buff.time += buffData.Time;
        buff.repeatCount += buffData.Count;


    }

    //移除buff
    private void RemoveBuff(Buff buff)
    {
        Logger.Log("Buff " + buff.ID + "已移除", LogType.Buff);

        //添加到待删除队列
        buffsToDelete[buffsToDeleteCount] = buff.ID;
        buffsToDeleteCount++;
        //可视化移除
        buffShow.Remove(buff);
    }

    /// <summary>
    /// Buff属性增加性同步
    /// </summary>
    /// <param name="buffdata"></param>
    private void AddBuffSync(int buffID)
    {
        BuffData buffdata = BuffDataBase.Instance.GetBuffData(buffID);
        myIndividual.HealthChange(buffdata.HpChange);
        myIndividual.HealthChange(buffdata.HpChange_p);
        myIndividual.AttackChange(buffdata.AttackChange);
        myIndividual.AttackChange(buffdata.AttackChange_p);
        myIndividual.AttackSpeedChange(buffdata.AttSpeedChange_p);
        myIndividual.SpeedChange(buffdata.SpeedChange_p);
        myIndividual.RecoverRateChange(buffdata.HpReturnChange);
        myIndividual.RecoverRateChange(buffdata.HpReturnChange_p);
        myIndividual.ReviveCountChange(buffdata.AddReviveCount);

        Logger.Log("生效Buff" + buffID + " ", LogType.Buff);
    }

    /// <summary>
    /// Buff属性移除性同步
    /// </summary>
    /// <param name="buffdata"></param>
    private void RemoveBuffSync(int buffID)
    {
        BuffData buffdata = BuffDataBase.Instance.GetBuffData(buffID);
        myIndividual.HealthChange(-buffdata.HpChange);
        myIndividual.HealthChange(-buffdata.HpChange_p);
        myIndividual.AttackChange(-buffdata.AttackChange);
        myIndividual.AttackChange(-buffdata.AttackChange_p);
        myIndividual.AttackSpeedChange(-buffdata.AttSpeedChange_p);
        myIndividual.SpeedChange(-buffdata.SpeedChange_p);
        myIndividual.RecoverRateChange(-buffdata.HpReturnChange);
        myIndividual.RecoverRateChange(-buffdata.HpReturnChange_p);
        myIndividual.ReviveCountChange(-buffdata.AddReviveCount);

        Logger.Log("失效Buff" + buffID + " ", LogType.Buff);
    }

    //BUFF时间更新
    private void BuffsUpdate()
    {
        foreach (var itr in myBuffs)
        {
            Buff buff = itr.Value;
            int count = itr.Value.repeatCount;
            //触发型buff机制
            if (buff.isTrigger)
            {
                //注意顺序：要先检测次数用尽，再执行次数减少
                //TRICK，避免多次释放/新建1次性触发Buff对象

                //触发次数用完
                if (count == 0)
                {
                    RemoveBuff(itr.Value);
                }
                //无限触发
                else if (count < 0)
                {
                    AddBuffSync(buff.ID);
                }
                //仍有触发次数
                else
                {
                    buff.repeatCount -= 1;
                    AddBuffSync(buff.ID);
                }
            }
            //持续性buff机制
            else
            {
                //无限触发
                if (count < 0)
                {
                    //DoNothing
                }
                //正常减少时间
                else
                {
                    buff.time -= Time.fixedDeltaTime;
                    if (itr.Value.time <= 0.0f)
                    {
                        RemoveBuff(itr.Value);
                        RemoveBuffSync(buff.ID);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Buff初始化列表
    /// </summary>
    private void InitializeBuffList()
    {
        if (initBuffs.Count == 0) return;
        //将初始化buff表里的ID依次加入到buff表里
        for(int i=0 ; i < initBuffs.Count; i++)
        {
            AddBuff(initBuffs[i]);
        }

        initBuffs.Clear();
    }

    /// <summary>
    /// 清理待删除Buff
    /// </summary>
    private void CleanBuffsToDelete()
    {
        for(int i = 0;i<buffsToDeleteCount; ++i)
        {
            myBuffs.Remove(buffsToDelete[i]);
        }
    }
    
}

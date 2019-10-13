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

    private BuffEffectManager buffEffectManager;
    private MessageSystem messageSystem;

    //TODO
    private BloodHUD bloodHUD;

    private void Awake()
    {
        messageSystem = GetComponent<MessageSystem>();
        myIndividual = GetComponent<Individual>();
        buffEffectManager = GameObject.FindGameObjectWithTag("Effects").GetComponent<BuffEffectManager>();
        bloodHUD = GameObject.Find("BloodHUD").GetComponent<BloodHUD>();
    }

    private void Start()
    {
        InitializeBuffList();
        //订阅消息
        messageSystem.registerBuffEvent((int sender, int buffID) => { AddBuff(buffID); });
        //订阅消息
        messageSystem.registerDieEvent((int sender) => { if (sender == myIndividual.ID) { this.enabled = false; } });
    }

    private void Update()
    {

    }

    private void FixedUpdate()
    {
        CheckBuffsToDelete();
        CleanBuffsToDelete();
        BuffsUpdate();
    }


    //添加buff
    private void AddBuff(int buffID)
    {
        Logger.Log("Buff " + buffID + " 已传入", LogType.Buff);

        Buff buff;
        BuffData buffData = BuffDataBase.Instance.GetBuffData(buffID);

        //若已有同种Buff，则对该Buff对象属性进行更新
        if (myBuffs.TryGetValue(buffID, out buff))
        {
            buff.time += buffData.Time;
            buff.repeatCount += buffData.Count;
        }
        //若buff列表没有同种Buff，则新建一个Buff对象
        else
        {
            buff = new Buff();
            //对该buff属性进行更新
            buff.ID = buffID;
            buff.isTrigger = buffData.isTrigger;
            buff.time = buffData.Time;
            buff.repeatCount = buffData.Count;
            //持续型Buff，第一次进入就要进行同步增加属性
            if (!buffData.isTrigger)
            {
                AddBuffSync(buffID);
            }
            //添加buff到列表
            myBuffs.Add(buffID, buff);
            //添加到显示面板里
            buffShow.Add(buff);
        }

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

        //TODO
        if (buffdata.HpChange != 0) bloodHUD.ShowBlood(buffdata.HpChange,transform);

        //myIndividual.HealthChange(buffdata.HpChange_p);
        myIndividual.AttackChange(buffdata.AttackChange);
        myIndividual.AttackChange(buffdata.AttackChange_p);
        myIndividual.AttackSpeedChange(buffdata.AttSpeedChange_p);
        myIndividual.SpeedChange(buffdata.SpeedChange_p);
        myIndividual.RecoverRateChange(buffdata.HpReturnChange);
        myIndividual.RecoverRateChange(buffdata.HpReturnChange_p);
        myIndividual.ReviveCountChange(buffdata.AddReviveCount);
        buffEffectManager.PlayEffect(transform,buffID);

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
        //myIndividual.HealthChange(-buffdata.HpChange_p);
        myIndividual.AttackChange(-buffdata.AttackChange);
        myIndividual.AttackChange(-buffdata.AttackChange_p);
        myIndividual.AttackSpeedChange(-buffdata.AttSpeedChange_p);
        myIndividual.SpeedChange(-buffdata.SpeedChange_p);
        myIndividual.RecoverRateChange(-buffdata.HpReturnChange);
        myIndividual.RecoverRateChange(-buffdata.HpReturnChange_p);
        myIndividual.ReviveCountChange(-buffdata.AddReviveCount);

        buffEffectManager.StopEffect(transform,buffID);

        Logger.Log("失效Buff" + buffID + " ", LogType.Buff);
    }

    //BUFF时间更新
    private void BuffsUpdate()
    {
        foreach (var itr in myBuffs)
        {
            Buff buff = itr.Value;
            //触发型buff机制
            if (buff.isTrigger)
            {
                AddBuffSync(buff.ID);
                if (buff.repeatCount > 0)buff.repeatCount -= 1;
                buff.time -= Time.fixedDeltaTime;
            }
            //持续性buff机制
            else
            {
                buff.time -= Time.fixedDeltaTime;
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
    /// 检查是否有过期的Buff
    /// </summary>
    private void CheckBuffsToDelete()
    {
        foreach (var itr in myBuffs)
        {
            Buff buff = itr.Value;
            //buff次数和时间都消耗完
            if (buff.repeatCount == 0 && buff.time < 0.01f)
            {
                RemoveBuff(itr.Value);
                //持续类型需要进行移除同步属性
                if (!buff.isTrigger)
                {
                    RemoveBuffSync(buff.ID);
                }
            }
        }
    }

    /// <summary>
    /// 清理待删除Buff
    /// </summary>
    private void CleanBuffsToDelete()
    {
        //移除
        for (int i = 0;i<buffsToDeleteCount; ++i)
        {
            myBuffs.Remove(buffsToDelete[i]);
        }
        buffsToDeleteCount = 0;
    }
    
}

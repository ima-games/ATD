using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("System/MessageSystem")]
public class MessageSystem : MonoBehaviour
{
    // BroadcastMessage  朝物体和所有子物体发送消息
    // SendMessage  朝物体下所有组件发送消息
    // SendMessageUpwards  朝物体和上级父物体发送信息
    Individual SelfIndicidual;

    private List<Action<Individual, float>> attackEventListeners = new List<Action<Individual, float>>();
    private List<Action<Individual, int>> buffEventListeners = new List<Action<Individual, int>>();
    private List<Action<Individual>> dieEventListeners = new List<Action<Individual>>();

    private void Awake()
    {
        SelfIndicidual = GetComponent<Individual>();
        //将事件类型和函数绑定
        EventCenter.AddListener<int, int, int, object>(EventType.Message, SolveMessage);
    }


    //-----------------------接受消息选择器-----------------------

    /// <summary>
    /// 处理消息的函数，消息类型对消息进行转发，将参数传入消息，伤害消息的来源将会被加入到仇恨表
    /// </summary>
    /// <param name="messageID"></param>
    /// <param name="ob"></param>
    /// <param name="senderID"></param>
    /// <param name="receverID"></param>
    public void SolveMessage(int messageID, int senderID, int receverID, object ob)
    {
        //看看自己是不是接收器的ID
        //0表示广播
        if (receverID != 0 && (SelfIndicidual == null || receverID != SelfIndicidual.ID)) return;

        Individual sourceInd = Factory.GetIndividual(senderID);

        //对消息来源进行选择，传参并转发，若为伤害信息，则加入到仇恨表中
        switch (messageID)
        {
            case 1: UnderAttack(sourceInd,(float)ob); break;
            case 2: GainBuff(sourceInd, (int)ob); break;
            case 3: IndividualDie(sourceInd); break;
            default: break;
        }
    }


    //-----------------------发出消息转发器-----------------------

    /// <summary>
    /// 消息类型 1 普通攻击 :自身对ID为receverID的个体发起攻击，伤害量为ob
    ///          2 加buff   :自身对ID为receverID的个体添加一个ID为ob的buff
    ///          3 死亡消息 :自身个体死亡
    /// </summary>
    /// <param name="messageID">消息类型</param>
    /// <param name="receverID">接收器</param>
    /// <param name="ob">消息参数</param>
    public void SendMessage(int messageID, int receverID, object ob)
    {
        switch (messageID)
        {
            case 1: EventCenter.Broadcast<int, int, int, object>(EventType.Message, messageID, SelfIndicidual.ID, receverID, ob); break;
            case 2: EventCenter.Broadcast<int, int, int, object>(EventType.Message, messageID, SelfIndicidual.ID, receverID, ob); break;
            case 3: EventCenter.Broadcast<int, int, int, object>(EventType.Message, messageID, SelfIndicidual.ID, receverID, ob); break;
            default: break;
        }
    }

    //-----------------------个体对象的各组件可调用的订阅函数------------

    /// <summary>
    /// 订阅攻击函数 action参数：攻击者，受伤者，伤害量
    /// </summary>
    /// <param name="action 参数：攻击者，伤害量"></param>
    public void registerAttackEvent(Action<Individual,float> action)
    {
        attackEventListeners.Add(action);
    }

    /// <summary>
    /// 订阅Buff函数 action参数：发送者，buffID
    /// </summary>
    /// <param name="action参数：发送者，buffID"></param>
    public void registerBuffEvent(Action<Individual, int> action)
    {
        buffEventListeners.Add(action);
    }

    /// <summary>
    /// 订阅死亡函数 action参数：死亡个体
    /// </summary>
    /// <param name="action"></param>
    public void registerDieEvent(Action<Individual> action)
    {
        dieEventListeners.Add(action);
    }


    //-----------------------以下为消息类型-----------------------

    //被攻击调用，攻击者，受伤者，伤害量
    private void UnderAttack(Individual sender,float damage)
    {
        for (int i = 0; i < attackEventListeners.Count; ++i)
        {
            attackEventListeners[i](sender, damage);
        }
    }

    //获得一个buff，发送者，接受者，buffID
    private void GainBuff(Individual sender,  int buffID)
    {
        for(int i = 0; i < buffEventListeners.Count; ++i)
        {
            buffEventListeners[i](sender,buffID);
        }
    }

    //sender个体死亡
    private void IndividualDie(Individual sender)
    {
        for (int i = 0; i < dieEventListeners.Count; ++i)
        {
            dieEventListeners[i](sender);
        }
    }
}

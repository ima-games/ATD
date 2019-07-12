using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("System/MessageSystem")]
public class MessageSystem : MonoBehaviour
{

    public int selfID = 0;
    // BroadcastMessage  朝物体和所有子物体发送消息
    // SendMessage  朝物体下所有组件发送消息
    // SendMessageUpwards  朝物体和上级父物体发送信息
    private List<Action<int, float>> attackEventListeners = new List<Action<int, float>>();
    private List<Action<int, int>> buffEventListeners = new List<Action<int, int>>();
    private List<Action<int>> dieEventListeners = new List<Action<int>>();

    private void Awake()
    {
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
        if (receverID != 0 && receverID != selfID ) return;

        //对消息来源进行选择，传参并转发，若为伤害信息，则加入到仇恨表中
        switch (messageID)
        {
            case 1: UnderAttack(senderID, (float)ob); break;
            case 2: GainBuff(senderID, (int)ob); break;
            case 3: IndividualDie(senderID); break;
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
            case 1: EventCenter.Broadcast<int, int, int, object>(EventType.Message, messageID, selfID, receverID, ob); break;
            case 2: EventCenter.Broadcast<int, int, int, object>(EventType.Message, messageID, selfID, receverID, ob); break;
            case 3: EventCenter.Broadcast<int, int, int, object>(EventType.Message, messageID, selfID, receverID, ob); break;
            default: break;
        }
    }

    //-----------------------个体对象的各组件可调用的订阅函数------------

    /// <summary>
    /// 订阅攻击函数 action参数：攻击者ID，受伤者ID，伤害量
    /// </summary>
    /// <param name="action 参数：攻击者ID，伤害量"></param>
    public void registerAttackEvent(Action<int,float> action)
    {
        attackEventListeners.Add(action);
    }

    /// <summary>
    /// 订阅Buff函数 action参数：发送者ID，buffID
    /// </summary>
    /// <param name="action参数：发送者ID，buffID"></param>
    public void registerBuffEvent(Action<int, int> action)
    {
        buffEventListeners.Add(action);
    }

    /// <summary>
    /// 订阅死亡函数 action参数：死亡个体ID
    /// </summary>
    /// <param name="action"></param>
    public void registerDieEvent(Action<int> action)
    {
        dieEventListeners.Add(action);
    }


    //-----------------------以下为消息类型-----------------------

    //被攻击调用，攻击者，受伤者，伤害量
    private void UnderAttack(int senderID, float damage)
    {
        for (int i = 0; i < attackEventListeners.Count; ++i)
        {
            attackEventListeners[i](senderID, damage);
        }
    }

    //获得一个buff，发送者，接受者，buffID
    private void GainBuff(int senderID, int buffID)
    {
        for(int i = 0; i < buffEventListeners.Count; ++i)
        {
            buffEventListeners[i](senderID,buffID);
        }
    }

    //sender个体死亡
    private void IndividualDie(int senderID)
    {
        for (int i = 0; i < dieEventListeners.Count; ++i)
        {
            dieEventListeners[i](senderID);
        }
    }
}

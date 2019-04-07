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
    HatredSystem SelfHatredSystem;


    private void Awake()
    {
        //将事件类型和函数绑定
        EventCenter.AddListener<int, int, int, object>(EventType.Message, SolveMessage);
    }

    private void Start()
    {
        SelfIndicidual = GetComponent<Individual>();
        SelfHatredSystem = GetComponent<HatredSystem>();
    }

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
        if (receverID != SelfIndicidual.ID) return;
        //对消息来源进行选择，传参并转发，若为伤害信息，则加入到仇恨表中
        switch(messageID)
        {
            case 1:UnderAttack(senderID, receverID, ob);break;
            default: break;
        }
    }
    /// <summary>
    /// 消息类型 1 普通攻击 2...
    /// </summary>
    /// <param name="messageID">消息类型</param>
    /// <param name="receverID">接收器</param>
    /// <param name="ob">消息参数</param>
    public void SendMessage(int messageID,  int receverID, object ob)
    {
        switch (messageID)
        {
            case 1: EventCenter.Broadcast<int, int, int, object>(EventType.Message, messageID, SelfIndicidual.ID, receverID, ob); break;
            default: break;
        }
    }


    //-----------------------以下为消息类型-----------------------

    

    /// <summary>
    /// 
    /// </summary>
    /// <param name="senderID">攻击者ID</param>
    /// <param name="receverID">受害者ID</param>
    /// <param name="ob">伤害量</param>
    private void UnderAttack(int senderID, int receverID, object ob)
    {
        SendMessage("ReduceHealth", ob);
        SelfHatredSystem.AddHateValue(LogicManager.Instance.GetIndividual(senderID));
    }
}

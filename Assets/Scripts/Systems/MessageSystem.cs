using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageSystem : MonoBehaviour
{
    // BroadcastMessage  朝物体和所有子物体发送消息
    // SendMessage  朝物体下所有组件发送消息
    // SendMessageUpwards  朝物体和上级父物体发送信息

    private void Awake()
    {
        EventCenter.AddListener(EventType.Hurt, Hurt);
    }

    public void Hurt()
    {
        SendHurtMessage(25.0f);
    }
    
    private void SendHurtMessage(float Damage)
    {
        Debug.Log("Finding the HP ");
        BroadcastMessage("ReduceHP",25);
    }
    
}

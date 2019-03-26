using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageSystem : MonoBehaviour
{
    // BroadcastMessage  朝物体和所有子物体发送消息
    // SendMessage  朝物体下所有组件发送消息
    // SendMessageUpwards  朝物体和上级父物体发送信息

    // Start is called before the first frame update
    void Start()
    {
        BroadcastMessage("GetMessage",0, SendMessageOptions.RequireReceiver);
        SendMessage("GetMessage", 1);
        SendMessageUpwards("GetMessage", 2);
    }

    // Update is called once per frame 
    void Update()
    {
        
    }
}

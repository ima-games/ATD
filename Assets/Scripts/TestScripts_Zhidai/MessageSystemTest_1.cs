using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageSystemTest_1 : MonoBehaviour
{
    private void Awake()
    {
        EventCenter.AddListener(EventType.Hurt, ReduceHealthPoint);
    }
    private void ReduceHealthPoint()
    {
        Debug.Log("You have Output a message!!");
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            EventCenter.Broadcast(EventType.Hurt);
            EventCenter.Broadcast(EventType.GetMessage);
        }
    }
}

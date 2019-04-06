using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageSystemTest_1 : MonoBehaviour
{
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            EventCenter.Broadcast(EventType.GetMessage);
        }
    }
}

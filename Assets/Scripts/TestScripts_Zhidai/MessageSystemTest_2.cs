using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageSystemTest_2 : MonoBehaviour
{
    private void GetMessage(int id)
    {
        string[] messagefrom = new string[3];
        messagefrom[0] = "BroadcastMessage";
        messagefrom[1] = "SendMessage";
        messagefrom[2] = "SendMessageUpwards";
        Debug.Log("MESSAGE COME FROM " + messagefrom[id] + " : This is " + gameObject.name);
    }
}

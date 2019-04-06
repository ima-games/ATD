using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageTest_3 : MonoBehaviour
{
    MessageSystem MS;
    // Start is called before the first frame update
    void Start()
    {
        MS = GetComponent<MessageSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.J))
        {
            MS.SendMessage(1,5,50);
        }
    }
}

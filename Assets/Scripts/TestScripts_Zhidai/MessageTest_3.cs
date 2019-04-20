using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageTest_3 : MonoBehaviour
{
    MessageSystem MS;
    Individual myIndividual;
    // Start is called before the first frame update
    void Start()
    {
        MS = GetComponent<MessageSystem>();
        myIndividual = GetComponent<Individual>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.J))
        {
            MS.SendMessage(1, 5, -25);
            myIndividual.HealthChange(-14);
            //测试buff系统
            MS.SendMessage(2, 5, 0);
            MS.SendMessage(2, 5, 1);
            MS.SendMessage(2, 5, 2);
        }
    }
}

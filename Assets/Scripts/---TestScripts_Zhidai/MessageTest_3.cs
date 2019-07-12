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
            MS.SendMessage(1, 3, 25);
            myIndividual.HealthChange(-14);
            //测试buff系统
            MS.SendMessage(2, 3, 0);
            MS.SendMessage(2, 3, 1);
            MS.SendMessage(2, 3, 2);
        }
    }
}

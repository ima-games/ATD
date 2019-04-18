using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class MessageSystemTest_2 : MonoBehaviour
{
    private void Awake()
    {
        EventCenter.AddListener(EventType.GetMessage, Output);
    }

    void Output()
    {
        StartCoroutine("OutputPlay");
        //Debug.Log("I have got your message!");
    }

    IEnumerator OutputPlay()
    {
        transform.localScale = new Vector3(3, 3, 3);

        yield return new WaitForSeconds(0.5f);

        transform.localScale = new Vector3(1, 1, 1);
        
    }
}

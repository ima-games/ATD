using System.Collections;
using System.Collections.Generic;
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
        Debug.Log("I have got your message!");
    }

    IEnumerator OutputPlay()
    {
        float timer = 0.2f;
        transform.localScale = new Vector3(2, 2, 2);

        while(timer>=0)
        {
            timer -= Time.deltaTime;
            yield return 0;
        }

        transform.localScale = new Vector3(1, 1, 1);
        
    }
}

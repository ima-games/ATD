using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyTextUI : MonoBehaviour
{
    private LogicManager logicManager;
    private Text text;
    private string moneyString = " $";

    private void Awake()
    {
        logicManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<LogicManager>();
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = logicManager.Cash + moneyString;
    }
}

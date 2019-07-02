using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyTextUI : MonoBehaviour
{
    private MoneyManager logicManager;
    private Text text;
    private string moneyString = " $";

    private void Awake()
    {
        logicManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<MoneyManager>();
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = logicManager.Cash + moneyString;
    }
}

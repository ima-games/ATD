using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HealthShow : MonoBehaviour
{
    Individual playerIndividual;
    Scrollbar myScrollbar;

    private void Start()
    {
        myScrollbar = GetComponent<Scrollbar>();
        playerIndividual = LogicManager.GetIndividual(1);
    }
    
    // Update is called once per frame
    void Update()
    {
        if(playerIndividual==null) playerIndividual = LogicManager.GetIndividual(2);
        myScrollbar.size = (float)playerIndividual.health / 100;
    }

    
}

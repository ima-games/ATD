using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BloodBarUI : MonoBehaviour
{
    private Individual playerIndividual;
    private Scrollbar myScrollbar;

    private void Awake()
    {
        myScrollbar = GetComponent<Scrollbar>();
    }

    private void Start()
    {
        playerIndividual = Factory.PlayerIndividual;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerIndividual.enabled == false)
        {
            myScrollbar.size = 0;
        }
        else
        {
            myScrollbar.size = playerIndividual.health / playerIndividual.maxHealth;
        }
    }


}

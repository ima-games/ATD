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
        if (playerIndividual == null)
        {
            myScrollbar.size = 0;
            return;
        }

        if (!playerIndividual.gameObject.activeInHierarchy)
        {
            playerIndividual = null;
            return;
        }

        myScrollbar.size = playerIndividual.health / playerIndividual.maxHealth;
    }


}

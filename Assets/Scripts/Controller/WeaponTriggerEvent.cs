using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponTriggerEvent : MonoBehaviour {
    public Individual master;

    private void OnCollisionEnter(Collision collision)
    {
        var otherGo = collision.gameObject;

        //武器打到的是自己,武器打到的是非个体单位
        if (otherGo == master.gameObject || LayerMask.LayerToName(otherGo.layer) != "Individual")
            return;

        Logger.Log("Weapon Collision Enter！", LogType.Individual);

        MessageSystem messageSystem = master.GetComponent<MessageSystem>();
        Individual otherIndividual = otherGo.GetComponent<Individual>();

        messageSystem.SendMessage(1, otherIndividual.ID, master.attack);
    }

    private void OnTriggerEnter (Collider other) {
        var otherGo = other.gameObject;

        //武器打到的是自己,武器打到的是非个体单位
        if (otherGo == master.gameObject || LayerMask.LayerToName(otherGo.layer) != "Individual" )
            return;

        Logger.Log("Weapon Trigger Enter！", LogType.Individual);

        MessageSystem messageSystem = master.GetComponent<MessageSystem> ();
        Individual otherIndividual = otherGo.GetComponent<Individual> ();

        messageSystem.SendMessage(1,otherIndividual.ID,master.attack);
    }

}
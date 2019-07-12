using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO：基地受伤逻辑
public class BaseKillMonsterLogic : MonoBehaviour
{
    private IndividualController baseIndividualController;
    private Individual baseIndividual;

    private void Awake()
    {
        baseIndividual = GetComponent<Individual>();
        baseIndividualController = GetComponent<IndividualController>();
    }

    void OnCollisionEnter(Collision collision)
    {
        Logger.Log(collision.gameObject.name, LogType.Individual);

        if (collision.gameObject.layer.Equals(LayerMask.NameToLayer("Individual")))
        {
            var ind = collision.gameObject.GetComponent<Individual>();
            if(ind.enabled && ind.power == Individual.Power.Monster)
            {
                collision.gameObject.GetComponent<IndividualController>().GetDamaged(0,999999.0f);

                baseIndividualController.GetDamaged(0, 5.0f);
                float BaseHp = baseIndividual.health;

                Logger.Log("基地血量为" + BaseHp, LogType.Individual);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO：基地受伤逻辑
public class BaseKillMonsterLogic : MonoBehaviour
{
    private Individual baseIndividual;

    void OnCollisionEnter(Collision collision)
    {
        Logger.Log(collision.gameObject.name, LogType.Individual);

        if (collision.gameObject.layer.Equals(LayerMask.NameToLayer("Individual"))&& collision.gameObject.GetComponent<Individual>().enabled)
        {
            collision.gameObject.GetComponent<Individual>().GetDamage(999999.0f);

            baseIndividual.health -= 5.0f;
            float BaseHp = baseIndividual.health;

            Logger.Log("基地血量为" + BaseHp,LogType.Individual);
        }
    }

    private void Awake()
    {
        baseIndividual = GetComponent<Individual>();
    }

}

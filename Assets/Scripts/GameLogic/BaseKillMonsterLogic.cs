using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseKillMonsterLogic : MonoBehaviour
{
    Individual baseIndividual;

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.tag.Equals("Enemy"))
        {
            Destroy(collision.gameObject);

            baseIndividual.ReduceHealth(1);
            float BaseHp = baseIndividual.health;

            Debug.Log("基地血量为" + BaseHp);
        }
    }

    private void Awake()
    {
        baseIndividual = gameObject.GetComponent<Individual>();
    }

}

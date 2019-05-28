using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseKillMonsterLogic : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.tag.Equals("Enemy"))
        {
            Destroy(collision.gameObject);
            int BaseHp = --gameObject.GetComponent<Individual>().health;

            Debug.Log("基地血量为" + BaseHp);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

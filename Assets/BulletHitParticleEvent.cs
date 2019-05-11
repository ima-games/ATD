using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHitParticleEvent : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DieAfterSeconds(2.0f));
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="对象死亡时间"></param>
    /// <returns></returns>
    IEnumerator DieAfterSeconds(float dieTime)
    {
        yield return new WaitForSeconds(dieTime);
        GameObject.Destroy(gameObject);
    }
}

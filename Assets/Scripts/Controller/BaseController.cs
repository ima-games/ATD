using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseController : IndividualController
{
    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void Attack(int targetID)
    {

    }

    public override void Die()
    {
        //避免物理碰撞事件
        gameObject.layer = 0;//default layer
        gameObject.SetActive(false);
    }

    public override void GetDamaged(int sourceID , float damage)
    {

    }

    public override void Walk(Vector3 velocity)
    {
        throw new System.NotImplementedException();
    }

}

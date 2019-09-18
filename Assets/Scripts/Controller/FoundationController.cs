using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoundationController : BaseIndividualController
{
    private void Start()
    {
        InitRegister();
    }

    public override void Walk(Vector3 velocity)
    {
        //基地不可行走
        throw new System.NotImplementedException();
    }

    public override void GetDamaged(int sourceID , float damage)
    {

    }

    public override void Attack(Individual ind)
    {
        //基地不可攻击
        throw new System.NotImplementedException();
    }

    public override void Die()
    {
        gameObject.SetActive(false);
    }
}

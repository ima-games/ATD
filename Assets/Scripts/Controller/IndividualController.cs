using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 个体控制器，基类用于提供接口
/// </summary>
public abstract class IndividualController : MonoBehaviour
{
    public abstract void Walk(Vector3 velocity);

    public abstract void Attack(int targetID);

    public abstract void GetDamaged(int sourceID, float damage);

    public abstract void Die();
}

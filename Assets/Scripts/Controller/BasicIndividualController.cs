using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 个体模型控制器，基类用于提供接口
/// </summary>
public abstract class BaseIndividualController : MonoBehaviour
{
    protected MessageSystem messageSystem;

    /// <summary>
    /// 初始化注册监听消息(必须在Start函数初始化)
    /// </summary>
    protected void InitRegister()
    {
        //注册受伤监听事件
        messageSystem = GetComponent<MessageSystem>();
        messageSystem.registerAttackEvent(GetDamaged);
    }

    //行走：每帧
    public abstract void Walk(Vector3 velocity);

    //受伤：触发
    public abstract void GetDamaged(int sourceID, float damage);

    //攻击：触发
    public abstract void Attack(Individual ind);

    //死亡：触发
    public abstract void Die();


}

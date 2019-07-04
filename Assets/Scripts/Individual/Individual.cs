using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Individual : MonoBehaviour
{
    public IndividualType individualType = IndividualType.Normal;   //个体类型

    public int ID = 0;
    public float health = 100;         //生命
    public float maxHealth = 100;      //最大生命值
    public float attack = 10;          //攻击力
    public float attackSpeed = 1.0f;   //攻击速度
    public float speed = 1.0f;         //速度
    public float attackDistance = 1.0f;//攻击距离
    public float recoverRate = 0.0f;   //回复速率

    public int hatredValue = 10;       //攻击时造成的仇恨值,先默认设定为10

    public enum Power {Monster, Human, Neutral}
    public Power power = Power.Neutral; //所属势力

    public bool movable = true;         //是否可移动
    public bool attackable = true;      //是否可攻击
    public bool controllable = true;    //是否可自由控制

    public bool tauntable = true;       //是否可被嘲讽
    public bool charmable = true;       //是否可被魅惑
    public bool restrictable = true;    //是否可被束缚
    public bool speedDownAble = true;   //是否可被减速
    public bool dizzyAble = true;       //是否可被麻痹

    public int reviveCount = 0;         //复活次数
    public int maxReviveCount = 0;      //最大复活次数

    //用于辅助的系统组件
    private MessageSystem messageSystem;
    private IndividualController controller;

    private void Awake()
    {
        //向工厂注册个体
        Factory.RegisterIndividual(this, individualType);

        messageSystem = GetComponent<MessageSystem>();
        controller = GetComponent<IndividualController>();
    }

    void Start()
    {
        RegisterEvent();
    }

    void RegisterEvent()
    {
        //订阅 受到攻击 消息
        messageSystem.registerAttackEvent((Individual attacker,float damage)=>{ GetDamage(damage); });
    }

    /// <summary>
    /// 个体属性更新
    /// </summary>
    private void Update()
    {
        health += recoverRate * Time.deltaTime;
    }

    //--------------------个体行为-------------------
    public void GetDamage(float damage)
    {
        HealthChange(-damage);
    }

    public void Attack(Individual target)
    {
        messageSystem.SendMessage(1,target.ID,attack);
    }

    //--------------------属性更改--------------------

    //改变固定数值的生命值
    public void HealthChange(float increment)
    {
        health += increment;
        health = Mathf.Min(health, maxHealth);

        if (health < 0)
        {
            //利用控制器执行死亡行为
            controller.Die();
            //通知死亡行为
            messageSystem.SendMessage(3,0,0);
            //个体脚本禁用
            this.enabled = false;
        }
        else if (increment < 0.0f)
        {
            //利用控制器执行受伤行为xcv 
            controller.GetDamaged();
        }
    }

    ////改变百分比生命值
    //public void HealthChange(double increment_p)
    //{
    //    health = (int)(1.0f + increment_p) * health;
    //    health = Mathf.Min(health, maxHealth);
    //    if (health < 0)
    //    {
    //        Dead();
    //    }
    //}
    
    //改变固定数值的攻击力
    public void AttackChange(int increment)
    {
        attack += increment;
    }

    //改变百分比攻击力
    public void AttackChange(double increment_p)
    {
        attack = (int)(1.0f + increment_p) * attack;
    }

    //改变百分比攻速
    public void AttackSpeedChange(double increment_p)
    {
        attackSpeed = (float)(attackSpeed + increment_p);
    }

    //改变百分比速度
    public void SpeedChange(double increment_p)
    {
        speed = (float)(speed + increment_p);
    }

    //改变固定数值的回血速度
    public void RecoverRateChange(int increment)
    {
        recoverRate += increment;
    }

    //改变百分比回血速度
    public void RecoverRateChange(double increment_p)
    {
        recoverRate = (float)(1.0f + increment_p) * recoverRate;
    }

    //改变固定数值的复活次数
    public void ReviveCountChange(int increment)
    {
        reviveCount += increment;
    }

}

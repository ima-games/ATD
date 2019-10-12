using BehaviorDesigner.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 怪物个体对象控制器
/// </summary>
public class MonsterController : BaseIndividualController
{
    public float removeTime = 3.0f; //死亡后移除时间

    private Individual selfIndividual;
    private Animator animator;
    private new Rigidbody rigidbody;
    private NavMeshAgent navMeshAgent;
    private BehaviorTree behaviorTree;
    private HatredSystem hatredSystem;

    private void Awake()
    {
        selfIndividual = GetComponent<Individual>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        behaviorTree = GetComponent<BehaviorTree>();
        hatredSystem = GetComponent<HatredSystem>();
    }

    private void Start()
    {
        InitRegister();
    }

    private void Update()
    {
        navMeshAgent.speed = selfIndividual.speed;
    }

    private void FixedUpdate()
    {

        //移动
        Walk(rigidbody.velocity);
    }


    /// <summary>
    /// 怪物移动函数
    /// </summary>
    /// <param name="velocity"></param>
    public override void Walk(Vector3 velocity)
    {
        animator.SetFloat("Velocity",velocity.magnitude);
    }

    /// <summary>
    /// 怪物对象受到伤害
    /// </summary>
    /// <param name="sourceID"></param>
    /// <param name="damage"></param>
    public override void GetDamaged(int sourceID, float damage)
    {
        selfIndividual.HealthChange(-damage);
        //生命值少于0，调用死亡行为
        if (selfIndividual.health < 0)
        {
            Die();
        }
        //调用受伤行为
        else
        {
            animator.SetTrigger("Hit");
        }
    }

    /// <summary>
    /// 怪物攻击
    /// </summary>
    public override void Attack(Individual ind)
    {
        if (ind == null) return;

        messageSystem.SendMessage(1, ind.ID, selfIndividual.attack);
    }

    /// <summary>
    /// 怪物对象死亡，执行一些必要的死亡操作（但未被移除）
    /// </summary>
    public override void Die()
    {
        //避免物理碰撞事件
        gameObject.layer = 12;//Dead layer

        //播放死亡动画
        animator.SetTrigger("Die");

        //关闭脚本
        
        behaviorTree.enabled = false;
        messageSystem.enabled = false;
        selfIndividual.enabled = false;
        this.enabled = false;

        //发出死亡消息
        messageSystem.SendMessage(3,0,0);

        StartCoroutine(RemoveObject());
    }
    
    /// <summary>
    /// 动画攻击事件用
    /// </summary>
    public void StartAttack()
    {
        var target = hatredSystem.GetMostHatedTarget();
        //若存在仇恨目标且距离在一定范围
        //TODO
        if (target && (target.position-transform.position).sqrMagnitude < 2.0f * transform.localScale.x * transform.localScale.x)
        {
            //攻击之
            Attack(target.GetComponent<Individual>());
        }

    }

    //--------------辅助函数-------------------------------------------

    // 移除个体对象
    private IEnumerator RemoveObject()
    {
        yield return new WaitForSeconds(removeTime);
        Destroy(gameObject);
        yield break;
    }

}

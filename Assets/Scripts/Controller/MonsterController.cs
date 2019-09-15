using BehaviorDesigner.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 怪物个体对象控制器
/// </summary>
public class MonsterController : IndividualController
{
    public float removeTime = 3.0f; //死亡后移除时间

    private Individual selfIndividual;
    private Animator animator;
    private new Rigidbody rigidbody;
    private NavMeshAgent navMeshAgent;
    private BehaviorTree behaviorTree;
    private MessageSystem messageSystem;

    private void Awake()
    {
        selfIndividual = GetComponent<Individual>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        behaviorTree = GetComponent<BehaviorTree>();
        messageSystem = GetComponent<MessageSystem>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //注册受伤监听事件
        messageSystem.registerAttackEvent(GetDamaged);
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        //移动
        Walk(rigidbody.velocity);
    }


    public override void Walk(Vector3 velocity)
    {
        animator.SetFloat("Velocity",velocity.magnitude);
    }

    public override void Attack(int targetID)
    {
        messageSystem.SendMessage(1, targetID, selfIndividual.attack);
        //animator.SetTrigger("Attack");
    }

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
    /// 个体对象死亡，执行一些必要的死亡操作（但未被移除）
    /// </summary>
    public override void Die()
    {
        //避免物理碰撞事件
        gameObject.layer = 0;//default layer

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

    // 移除个体对象
    IEnumerator RemoveObject()
    {
        yield return new WaitForSeconds(removeTime);

        Destroy(gameObject);

        yield break;
    }

}

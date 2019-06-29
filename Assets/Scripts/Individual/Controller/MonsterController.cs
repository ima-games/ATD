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

    private Animator animator;
    private new Rigidbody rigidbody;
    private NavMeshAgent navMeshAgent;
    private BuffSystem buffSystem;
    private HatredSystem hatredSystem;
    private BehaviorTree behaviorTree;
    private MessageSystem messageSystem;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        buffSystem = GetComponent<BuffSystem>();
        hatredSystem = GetComponent<HatredSystem>();
        behaviorTree = GetComponent<BehaviorTree>();
        messageSystem = GetComponent<MessageSystem>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Walk(rigidbody.velocity);
    }

    public override void Walk(Vector3 velocity)
    {
        animator.SetFloat("Velocity",velocity.magnitude);
    }

    public override void Attack()
    {
        animator.SetTrigger("Attack");
    }

    public override void GetDamaged()
    {
        animator.SetTrigger("Hit");
    }

    /// <summary>
    /// 个体对象死亡，执行一些必要的死亡操作（但未被移除）
    /// </summary>
    public override void Die()
    {
        //避免物理碰撞事件
        gameObject.layer = 0;//default layer

        //关闭脚本
        buffSystem.enabled = false ;
        hatredSystem.enabled =false;
        behaviorTree.enabled = false;
        messageSystem.enabled = false;

        //播放死亡动画
        animator.SetTrigger("Die");

        //rigidbody.freezeRotation = false;

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

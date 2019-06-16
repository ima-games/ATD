using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : MonoBehaviour
{
    private Animator animator;
    private new Rigidbody rigidbody;
    private NavMeshAgent navMeshAgent;

    private MessageSystem messageSystem;

    private void Awake()
    {
        messageSystem = GetComponent<MessageSystem>();
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        RegisterMessage();   
    }

    void RegisterMessage()
    {
        messageSystem.registerAttackEvent(
            (Individual attacker, float damage) =>
            {
                animator.SetTrigger("Hit");
            }
            );
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("Velocity", rigidbody.velocity.magnitude);
    }

    public void Attack()
    {
        animator.SetTrigger("Attack");
    }

    public void GetDamaged()
    {
        animator.SetTrigger("Hit");
    }


}

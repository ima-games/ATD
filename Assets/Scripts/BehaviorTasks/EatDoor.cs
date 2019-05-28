using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
    [TaskDescription("贪婪之门的攻击行为")]
    [TaskCategory("Tower Attack Model")]
    [TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}EatIcon.png")]
    public class EatDoor : Action
    {
        public SharedTransform target;
        //public GameObject bullet;
        public GameObject bulletPoint;
        public float bulletSpeed;
        public float bulletForce;
        public float arriveDistance = 0.1f;
        public float attackRate;

        private Individual master;          //寄主
        private bool attacking = false;     //攻击正在进行中
        
        IEnumerator Attack()
        {
            attacking = true;//开始攻击
            Vector3 pullTarget = bulletPoint.transform.position - target.Value.transform.position;
            pullTarget.y = 0;
            Debug.Log(pullTarget);
            target.Value.GetComponent<Rigidbody>().velocity = pullTarget.normalized * bulletSpeed;
            
            //target.Value.GetComponent<BehaviorTree>().

            if (pullTarget.magnitude < arriveDistance)
            {
                Debug.Log("eat" + target.Name);
            }
            yield return new WaitForSeconds(attackRate);
            attacking = false;//准备好下一次攻击

        }

        public override void OnStart()
        {
            master = gameObject.GetComponent<Individual>();
            attackRate = 1.0f / master.GetComponent<Individual>().attackSpeed;
        }

        public override TaskStatus OnUpdate()
        {
            if (target.Value == null)
            {
                return TaskStatus.Failure;
            }
            if (!attacking)
            {
                StartCoroutine(Attack());
            }

            return TaskStatus.Success;
        }

        
    }
}


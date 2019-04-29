using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
    [TaskDescription("火焰塔的攻击行为")]
    [TaskCategory("Tower Attack Model")]
    [TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}FireBallIcon.png")]
    public class FireBall : Action
    {
        public SharedTransform target;
        public GameObject bullet;
        public float bulletSpeed;
        public float bulletForce;
        public float attackRate;

        private Individual master;          //寄主
        private bool attacking = false;     //攻击正在进行中

        private float distanceToTarget;

        IEnumerator Attack()
        {
            attacking = true;//开始攻击
            Vector3 fireDirection = target.Value.position + new Vector3(0, 50, 0);
            GameObject bulletObj = GameObject.Instantiate(bullet,
                fireDirection, Quaternion.identity);

            //给子弹对象脚本赋值
            bulletObj.GetComponent<BulletTriggerEvent>().tower = master;
            //bulletObj.GetComponent<Rigidbody>().velocity = Vector3.down * bulletSpeed;
            bulletObj.GetComponent<Rigidbody>().AddForce(Vector3.down * bulletForce);

            yield return new WaitForSeconds(attackRate);

            attacking = false;//可以准备下一次攻击了
        }

        IEnumerator StartShoot()
        {
            Vector3 targetPos = target.Value.transform.position;
            GameObject bulletObj = GameObject.Instantiate(bullet,
                gameObject.transform.position + new Vector3(0, 10, 0), Quaternion.identity);
            //让始终它朝着目标
            bulletObj.transform.LookAt(targetPos);

            //计算弧线中的夹角
            float angle = Mathf.Min(1, Vector3.Distance(bulletObj.transform.position, targetPos) / distanceToTarget) * 45;
            bulletObj.transform.rotation = bulletObj.transform.rotation * Quaternion.Euler(Mathf.Clamp(-angle, -42, 42), 0, 0);
            float currentDist = Vector3.Distance(bulletObj.transform.position, target.Value.transform.position);

            bulletObj.transform.Translate(Vector3.forward * Mathf.Min(bulletSpeed * Time.deltaTime, currentDist));
            yield return new WaitForSeconds(attackRate);
        }

        public override void OnStart()
        {
            master = gameObject.GetComponent<Individual>();
            //初始化寄主的攻击速度 攻击间隔 = 1s / 攻击速度
            attackRate = 1.0f / master.GetComponent<Individual>().attackSpeed;

            distanceToTarget = Vector3.Distance(this.transform.position, target.Value.transform.position);
            
        }

        public override TaskStatus OnUpdate()
        {
            if (target.Value == null)
            {
                return TaskStatus.Failure;
            }

            //无需检测目标对象是否正确，CanSeeMonster已经过滤掉非Monster对象
            //若没在进行攻击，则可以进行一次攻击行为
            if (!attacking)
            {
                //StartCoroutine(Attack());
                StartCoroutine(StartShoot());
            }
            return TaskStatus.Success;
        }
        public override void OnReset()
        {
            //bullet=
            bulletSpeed = 25;
            bulletForce = 10;
            attackRate = 1;
        }
    }
}



using BehaviorDesigner.Runtime.Tasks.Movement;
using BehaviorDesigner.Runtime.ObjectDrawers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
    [ExecuteInEditMode]
    [TaskDescription("沥青塔的攻击行为")]
    [TaskCategory("Tower Attack Model")]
    [TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}SlowDownIcon.png")]
    public class SlowDown : Action
    {
        public Individual tower;
        public SharedTransform target;
        public float fieldOfViewAngle = 90;
        public float viewDistance = 10;
        public float attackRate;
        public int RayCastCount = 4;


        private Individual master;
        private bool attacking = false;
        private float rayAngle;
        private List<Vector3> n;
        private Dictionary<string, GameObject> rh;

        IEnumerator Attack()
        {
            attacking = true;//开始攻击

            GetRays();
            ShootRay();
            CalculateHurt();

            yield return new WaitForSeconds(attackRate);

            attacking = false;//可以准备下一次攻击了
        }

        void ShootRay()
        {
            RaycastHit[] hits;
            
            Vector3 dir = transform.position;

            for (int i = 0; i < n.Count; i++)
            {
                hits = Physics.RaycastAll(dir, n[i], viewDistance, 1 << 10);
                for (int j = 0; j < hits.Length; j++) 
                {
                    if (!rh.ContainsKey(hits[j].transform.name))
                    {
                        rh.Add(hits[j].transform.name, hits[j].transform.gameObject);
                    }
                }

                Debug.DrawRay(dir, n[i], Color.black);
            }
            
            
        }

        void CalculateHurt()
        {
            if (rh == null)
            {
                Debug.LogError("空字典");
            }
            
            foreach (var j in rh)
            {
                Debug.Log("name:"+j.Key+" gameobj:"+j.Value.name);
                MessageSystem messageSystem = tower.GetComponent<MessageSystem>();
                Individual otherIndividual = j.Value.GetComponent<Individual>();

                messageSystem.SendMessage(1, otherIndividual.ID, tower.attack);
            }

            
        }

        void GetRays()
        {
            n = new List<Vector3>();
            for (int i = RayCastCount / 2; i > 0; i--)
            {
                Vector3 newVec = Quaternion.Euler(0, -1 * rayAngle * i, 0) * transform.forward * viewDistance;
                n.Add(newVec);
            }

            for (int i = 0; i < RayCastCount / 2; i++)
            {
                Vector3 newVec = Quaternion.Euler(0, rayAngle * i, 0) * transform.forward * viewDistance;
                n.Add(newVec);
            }
        }


        public override void OnStart()
        {
            master = gameObject.GetComponent<Individual>();
            attackRate = 1.0f / master.GetComponent<Individual>().attackSpeed;
            rayAngle = fieldOfViewAngle / RayCastCount;
            rh = new Dictionary<string, GameObject>();
        }

        public override TaskStatus OnUpdate()
        {
            
            if (target.Value == null)
            {
                return TaskStatus.Failure;
            }
            else
            {
                Vector3 dir = target.Value.transform.position - transform.position;
                dir.y = 0;
                gameObject.transform.rotation = Quaternion.LookRotation(dir);

                

                if (!attacking)
                {
                    StartCoroutine(Attack());

                }
            }
            return TaskStatus.Success;
        }

        public override void OnDrawGizmos()
        {
            if (fieldOfViewAngle == 0 || viewDistance == 0)
            {
                return;
            }
            MovementUtility.DrawLineOfSightColor(Owner.transform, Vector3.zero, fieldOfViewAngle,
                viewDistance, false, Color.red);


        }
    }
}


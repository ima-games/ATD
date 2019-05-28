using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
    [TaskDescription("设定的行径点移动(只执行一次)")]
    [TaskCategory("Movement")]
    [TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}SeekIcon.png")]
    public class SeekList : Action
    {
        [Tooltip("移动速度")]
        public SharedFloat speed;
        [Tooltip("转向角速度")]
        public SharedFloat angularSpeed;
        [Tooltip("小于该值时，判定已到达")]
        public SharedFloat arriveDistance = 0.1f;
        [Tooltip("路径点列表")]
        public SharedTransformList waypoints;

        [SerializeField]
        [Tooltip("该值不可修改！！！")]
        private int waypointIndex;

        private bool dynamicTarget;
        // A cache of the NavMeshAgent
        private UnityEngine.AI.NavMeshAgent navMeshAgent;

        public override void OnAwake()
        {
            // cache for quick lookup
            navMeshAgent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
        }

        public override void OnStart()
        {
            // initially move towards the closest waypoint
            float localDistance;
            float minDistance = 1000f;
            for (int i = 0; i < waypoints.Value.Count; ++i)
            {
                localDistance = Vector3.Magnitude(transform.position - waypoints.Value[i].position);
                if (localDistance < minDistance)
                {
                    minDistance = localDistance;
                    waypointIndex = i;
                }
            }

            // set the speed, angular speed, and destination then enable the agent
            navMeshAgent.speed = speed.Value;
            navMeshAgent.angularSpeed = angularSpeed.Value;
            navMeshAgent.enabled = true;
            navMeshAgent.destination = Target();
        }

        // Patrol around the different waypoints specified in the waypoint array. Always return a task status of running. 
        public override TaskStatus OnUpdate()
        {
            if (!navMeshAgent.pathPending)
            {
                var thisPosition = transform.position;
                thisPosition.y = navMeshAgent.destination.y; // ignore y
                if (Vector3.SqrMagnitude(thisPosition - navMeshAgent.destination) < arriveDistance.Value)
                {
                    if (waypointIndex<waypoints.Value.Count - 1)
                    {
                        waypointIndex++;
                        navMeshAgent.destination = Target();
                    }
                    else
                    {
                        return TaskStatus.Success;
                    }
                    
                }
            }

            return TaskStatus.Running;
        }

        public override void OnEnd()
        {
            // Disable the nav mesh
            navMeshAgent.enabled = false;
        }

        // Return the current waypoint index position
        private Vector3 Target()
        {
            return waypoints.Value[waypointIndex].position;
        }

        // Reset the public variables
        public override void OnReset()
        {
            arriveDistance = 0.1f;
            waypoints = null;
        }
        

    }

}

using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Movement
{
    [TaskDescription("Patrol around the specified waypoints using the Unity NavMesh.")]
    [TaskCategory("Movement")]
    [HelpURL("http://www.opsive.com/assets/BehaviorDesigner/Movement/documentation.php?id=7")]
    [TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}PatrolIcon.png")]
    public class Patrol : Action
    {
        [Tooltip("The speed of the agent")]
        public SharedFloat speed;
        [Tooltip("Angular speed of the agent")]
        public SharedFloat angularSpeed;
        [Tooltip("The agent has arrived when the square magnitude is less than this value")]
        public SharedFloat arriveDistance = 0.1f;
        [Tooltip("Should the agent patrol the waypoints randomly?")]
        public SharedBool randomPatrol = false;
        [Tooltip("The waypoints to move to")]
        public SharedTransformList waypoints;

        // A cache of the NavMeshAgent
        private UnityEngine.AI.NavMeshAgent navMeshAgent;
        // The current index that we are heading towards within the waypoints array
        private int waypointIndex;

        public override void OnAwake()
        {
            // cache for quick lookup
            navMeshAgent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
        }

        public override void OnStart()
        {
            // initially move towards the closest waypoint
            float distance = Mathf.Infinity;
            float localDistance;
            for (int i = 0; i < waypoints.Value.Count; ++i) {
                if ((localDistance = Vector3.Magnitude(transform.position - waypoints.Value[i].position)) < distance) {
                    distance = localDistance;
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
            if (!navMeshAgent.pathPending) {
                var thisPosition = transform.position;
                thisPosition.y = navMeshAgent.destination.y; // ignore y
                if (Vector3.SqrMagnitude(thisPosition - navMeshAgent.destination) < arriveDistance.Value) {
                    if (randomPatrol.Value) {
                        waypointIndex = Random.Range(0, waypoints.Value.Count);
                    } else {
                        waypointIndex = (waypointIndex + 1) % waypoints.Value.Count;
                    }
                    navMeshAgent.destination = Target();
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
            randomPatrol = false;
        }

        // Draw a gizmo indicating a patrol 
        public override void OnDrawGizmos()
        {
#if UNITY_EDITOR
            if (waypoints == null) {
                return;
            }
            var oldColor = UnityEditor.Handles.color;
            UnityEditor.Handles.color = Color.yellow;
            for (int i = 0; i < waypoints.Value.Count; ++i) {
                UnityEditor.Handles.SphereCap(0, waypoints.Value[i].position, waypoints.Value[i].rotation, 1);
            }
            UnityEditor.Handles.color = oldColor;
#endif
        }
    }
}
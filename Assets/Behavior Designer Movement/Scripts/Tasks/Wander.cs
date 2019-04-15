using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Movement
{
    [TaskDescription("Wander using the Unity NavMesh.")]
    [TaskCategory("Movement")]
    [HelpURL("http://www.opsive.com/assets/BehaviorDesigner/Movement/documentation.php?id=9")]
    [TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}WanderIcon.png")]
    public class Wander : Action
    {
        [Tooltip("The speed of the agent")]
        public SharedFloat speed;
        [Tooltip("Angular speed of the agent")]
        public SharedFloat angularSpeed;
        [Tooltip("How far ahead of the current position to look ahead for a wander")]
        public SharedFloat wanderDistance = 20;
        [Tooltip("The amount that the agent rotates direction")]
        public SharedFloat wanderRate = 2;

        // A cache of the NavMeshAgent
        private UnityEngine.AI.NavMeshAgent navMeshAgent;

        public override void OnAwake()
        {
            // cache for quick lookup
            navMeshAgent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
        }

        public override void OnStart()
        {
            // set the speed, angular speed, and destination then enable the agent
            navMeshAgent.speed = speed.Value;
            navMeshAgent.angularSpeed = angularSpeed.Value;
            navMeshAgent.enabled = true;
            navMeshAgent.destination = Target();
        }

        // There is no success or fail state with wander - the agent will just keep wandering
        public override TaskStatus OnUpdate()
        {
            navMeshAgent.destination = Target();
            return TaskStatus.Running;
        }

        public override void OnEnd()
        {
            // Disable the nav mesh
            navMeshAgent.enabled = false;
        }

        // Return targetPosition if targetTransform is null
        private Vector3 Target()
        {
            // point in a new random direction and then multiply that by the wander distance
            var direction = transform.forward + Random.insideUnitSphere * wanderRate.Value;
            return transform.position + direction.normalized * wanderDistance.Value;
        }

        // Reset the public variables
        public override void OnReset()
        {
            wanderDistance = 20;
            wanderRate = 2;
        }
    }
}
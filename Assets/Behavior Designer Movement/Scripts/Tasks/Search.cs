using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Movement
{
    [TaskDescription("Search for a target by combining the wander, within hearing range, and the within seeing range tasks using the Unity NavMesh.")]
    [TaskCategory("Movement")]
    [HelpURL("http://www.opsive.com/assets/BehaviorDesigner/Movement/documentation.php?id=10")]
    [TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}SearchIcon.png")]
    public class Search : Action
    {
        [Tooltip("The speed of the agent")]
        public SharedFloat speed;
        [Tooltip("Angular speed of the agent")]
        public SharedFloat angularSpeed;
        [Tooltip("How far ahead of the current position to look ahead for a wander")]
        public SharedFloat wanderDistance = 10;
        [Tooltip("The amount that the agent rotates direction")]
        public SharedFloat wanderRate = 1;
        [Tooltip("The field of view angle of the agent (in degrees)")]
        public SharedFloat fieldOfViewAngle = 90;
        [Tooltip("The distance that the agent can see")]
        public SharedFloat viewDistance = 30;
        [Tooltip("Should the search end if audio was heard?")]
        public SharedBool senseAudio = true;
        [Tooltip("How far away the unit can hear")]
        public SharedFloat hearingRadius = 30;
        [Tooltip("The offset relative to the pivot position")]
        public SharedVector3 offset;
        [Tooltip("The LayerMask of the objects that we are searching for")]
        public LayerMask objectLayerMask;
        [Tooltip("The furtuer away a sound source is the less likely the agent will be able to hear it. " +
                 "Set a threshold for the the minimum audibility level that the agent can hear")]
        public SharedFloat linearAudibilityThreshold = 0.03f;
        [Tooltip("The object that is within sight")]
        public SharedTransform objectFound;

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

        // Keep searching until an object is seen or heard (if senseAudio is enabled)
        public override TaskStatus OnUpdate()
        {
            navMeshAgent.destination = Target();
            // Detect if any objects are within sight
            objectFound.Value = MovementUtility.WithinSight(transform, offset.Value, fieldOfViewAngle.Value, viewDistance.Value, objectLayerMask);
            // If an object was seen then return success
            if (objectFound.Value != null) {
                return TaskStatus.Success;
            }
            // Detect if any object are within audio range (if enabled)
            if (senseAudio.Value) {
                objectFound.Value = MovementUtility.WithinHearingRange(transform, offset.Value, linearAudibilityThreshold.Value, hearingRadius.Value, objectLayerMask);
                // If an object was heard then return success
                if (objectFound.Value != null) {
                    return TaskStatus.Success;
                }
            }

            // No object has been seen or heard so keep searching
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
            wanderDistance = 10;
            wanderRate = 1;
            fieldOfViewAngle = 90;
            viewDistance = 30;
            senseAudio = true;
            hearingRadius = 30;
            linearAudibilityThreshold = 0.03f;
        }
    }
}
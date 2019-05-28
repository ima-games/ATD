using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Movement
{
    [TaskDescription("Evade the target specified using the Unity NavMesh.")]
    [TaskCategory("Movement")]
    [HelpURL("http://www.opsive.com/assets/BehaviorDesigner/Movement/documentation.php?id=6")]
    [TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}EvadeIcon.png")]
    public class Evade : Action
    {
        [Tooltip("The speed of the agent")]
        public SharedFloat speed;
        [Tooltip("Angular speed of the agent")]
        public SharedFloat angularSpeed;
        [Tooltip("The agent has fleed when the square magnitude is greater than this value")]
        public SharedFloat fleedDistance = 10;
        [Tooltip("The distance to look ahead when fleeing")]
        public SharedFloat lookAheadDistance = 5;
        [Tooltip("How far to predict the distance ahead of the target. Lower values indicate less distance should be predicated")]
        public SharedFloat targetDistPrediction = 20;
        [Tooltip("Multiplier for predicting the look ahead distance")]
        public SharedFloat targetDistPredictionMult = 20;
        [Tooltip("The transform that the agent is evading")]
        public SharedTransform targetTransform;

        // The position of the target at the last frame
        private Vector3 targetPosition;
        // A cache of the NavMeshAgent
        private UnityEngine.AI.NavMeshAgent navMeshAgent;

        public override void OnAwake()
        {
            // cache for quick lookup
            navMeshAgent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
        }

        public override void OnStart()
        {
            targetPosition = targetTransform.Value.position;

            // set the speed, angular speed, and destination then enable the agent
            navMeshAgent.speed = speed.Value;
            navMeshAgent.angularSpeed = angularSpeed.Value;
            navMeshAgent.enabled = true;
            navMeshAgent.destination = Target();
        }

        // Evade from the target. Return success once the agent has fleed the target by moving far enough away from it
        // Return running if the agent is still fleeing
        public override TaskStatus OnUpdate()
        {
            if (!navMeshAgent.pathPending && Vector3.SqrMagnitude(transform.position - targetTransform.Value.position) > fleedDistance.Value) {
                return TaskStatus.Success;
            }

            navMeshAgent.destination = FleePosition();

            return TaskStatus.Running;
        }

        public override void OnEnd()
        {
            // Disable the nav mesh
            navMeshAgent.enabled = false;
        }

        private Vector3 Target()
        {
            // Calculate the current distance to the target and the current speed
            var distance = (targetTransform.Value.position - transform.position).magnitude;
            var speed = navMeshAgent.velocity.magnitude;

            float futurePrediction = 0;
            // Set the future prediction to max prediction if the speed is too small to give an accurate prediction
            if (speed <= distance / targetDistPrediction.Value) {
                futurePrediction = targetDistPrediction.Value;
            } else {
                futurePrediction = (distance / speed) * targetDistPredictionMult.Value; // the prediction should be accurate enough
            }

            // Predict the future by taking the velocity of the target and multiply it by the future prediction
            var prevTargetPosition = targetPosition;
            targetPosition = targetTransform.Value.position;
            return targetPosition + (targetPosition - prevTargetPosition) * futurePrediction;
        }

        // Evade in the opposite direction
        private Vector3 FleePosition()
        {
            return transform.position + (transform.position - Target()).normalized * lookAheadDistance.Value;
        }

        // Reset the public variables
        public override void OnReset()
        {
            fleedDistance = 10;
            lookAheadDistance = 5;
            targetDistPrediction = 20;
            targetDistPredictionMult = 20;
        }
    }
}
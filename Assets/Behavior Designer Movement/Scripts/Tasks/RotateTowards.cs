using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Movement
{
    [TaskDescription("Rotates towards the specified rotation. The rotation can either be specified by a transform or rotation. If the transform "+
                     "is used then the rotation will not be used.")]
    [TaskCategory("Movement")]
    [HelpURL("http://www.opsive.com/assets/BehaviorDesigner/Movement/documentation.php?id=2")]
    [TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}RotateTowardsIcon.png")]
    public class RotateTowards : Action
    {
        [Tooltip("The agent is done rotating when the square magnitude is less than this value")]
        public SharedFloat rotationEpsilon = 0.5f;
        [Tooltip("Max rotation delta")]
        public SharedFloat maxLookAtRotationDelta;
        [Tooltip("Should the rotation only affect the Y axis?")]
        public SharedBool onlyY;
        [Tooltip("The transform that the agent is rotating towards")]
        public SharedTransform targetTransform;
        [Tooltip("If target is null then use the target rotation")]
        public SharedVector3 targetRotation;

        public override void OnStart()
        {
            if ((targetTransform == null || targetTransform.Value == null) && targetRotation == null) {
                Debug.LogError("Error: A RotateTowards target value is not set.");
                targetRotation = new SharedVector3(); // create a new SharedQuaternion to prevent repeated errors
            }
        }

        public override TaskStatus OnUpdate()
        {
            var rotation = Target();
            // Return a task status of success once we are done rotating
            if (Quaternion.Angle(transform.rotation, rotation) < rotationEpsilon.Value) {
                return TaskStatus.Success;
            }
            // We haven't reached the target yet so keep rotating towards it
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, maxLookAtRotationDelta.Value);
            return TaskStatus.Running;
        }

        // Return targetPosition if targetTransform is null
        private Quaternion Target()
        {
            if (targetTransform == null || targetTransform.Value == null) {
                return Quaternion.Euler(targetRotation.Value);
            }
            var position = targetTransform.Value.position - transform.position;
            if (onlyY.Value) {
                position.y = 0;
            }
            return Quaternion.LookRotation(position);
        }

        // Reset the public variables
        public override void OnReset()
        {
            rotationEpsilon = 0.5f;
        }
    }
}
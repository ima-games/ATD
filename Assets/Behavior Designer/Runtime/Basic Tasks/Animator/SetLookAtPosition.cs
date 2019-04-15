using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityAnimator
{
    [TaskCategory("Basic/Animator")]
    [TaskDescription("Sets the look at position. Returns Success.")]
    public class SetLookAtPosition : Action
    {
        [Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
        public SharedGameObject targetGameObject;
        [Tooltip("The position to lookAt")]
        public SharedVector3 position;

        private Animator animator;
        private GameObject prevGameObject;
        private bool positionSet;

        public override void OnStart()
        {
            var currentGameObject = GetDefaultGameObject(targetGameObject.Value);
            if (currentGameObject != prevGameObject) {
                animator = currentGameObject.GetComponent<Animator>();
                prevGameObject = currentGameObject;
            }
            positionSet = false;
        }

        public override TaskStatus OnUpdate()
        {
            if (animator == null) {
                Debug.LogWarning("Animator is null");
                return TaskStatus.Failure;
            }

            return positionSet ? TaskStatus.Success : TaskStatus.Running;
        }

        public override void OnAnimatorIK()
        {
            if (animator == null) {
                return;
            }
            animator.SetLookAtPosition(position.Value);
            positionSet = true;
        }

        public override void OnReset()
        {
            targetGameObject = null;
            position = Vector3.zero;
        }
    }
}
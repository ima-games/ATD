using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
    public class GetMostHatredTarget : Action
    {
        [Tooltip("AI的寄主")]
        public SharedGameObject master;

        [Tooltip("目标对象")]
        public SharedTransform targetTransform;

        public override TaskStatus OnUpdate()
        {
            var ts = master.Value.GetComponent<HatredSystem>().GetMostHatedTarget().transform;
            targetTransform.SetValue(ts);
            return TaskStatus.Success;
        }

    }

}
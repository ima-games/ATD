using UnityEngine;


namespace BehaviorDesigner.Runtime.Tasks
{
    [TaskDescription("获取最大仇恨值的非基地目标")]
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
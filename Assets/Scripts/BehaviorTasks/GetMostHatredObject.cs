using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
    public class GetMostHatredObject : Action
    {
        public SharedTransform target;

        private HatredSystem MostHated;

        public override void OnStart()
        {
            MostHated = gameObject.GetComponent<HatredSystem>();
        }

        public override TaskStatus OnUpdate()
        {
            target.Value = MostHated.GetMostHatedTarget();
            if (target != null)
                return TaskStatus.Success;
            return TaskStatus.Failure;
        }


    }
}
    

    


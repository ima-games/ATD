using BehaviorDesigner.Runtime.Tasks.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
    [TaskDescription("看到视野范围内的敌人")]
    [TaskCategory("Movement")]
    [TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}CanSeeObjectIcon.png")]
    public class canSeeEnemy : Conditional
    {
        public SharedTransform targetObject;
        public SharedFloat fieldOfViewAngle = 90;
        public SharedFloat viewDistance = 10;

        public SharedTransform objectInSight;


        public override TaskStatus OnUpdate()
        {
                // If the target object is null then determine if there are any objects within sight based on the layer mask
                if (targetObject.Value == null)
                {
                    objectInSight.Value = MovementUtility.WithinSight(transform, Vector3.zero, fieldOfViewAngle.Value, viewDistance.Value, 0);
                }
                else
                { // If the target is not null then determine if that object is within sight
                    objectInSight.Value = MovementUtility.WithinSight(transform, Vector3.zero, fieldOfViewAngle.Value, viewDistance.Value, targetObject.Value);
                }

            if (objectInSight.Value != null)
            {
                // Return success if an object was found
                return TaskStatus.Success;
            }
            // An object is not within sight so return failure
            return TaskStatus.Failure;
        }

        //public override TaskStatus OnUpdate()
        //{
        //    if (objectInSight.Value != null)
        //    {
        //        return TaskStatus.Success;
        //    }
        //    foreach (var target_v in targetObject)
        //    {
        //        float distance = (target_v.Value.position - transform.position).magnitude;
        //        float angle = Vector3.Angle(transform.forward, target_v.Value.position - transform.position);
        //        if (distance < viewDistance.Value && angle < fieldOfViewAngle.Value * 0.5f)
        //        {
        //            objectInSight = target_v;
        //            return TaskStatus.Success;
        //        }
        //    }

        //    return TaskStatus.Failure;
        //}
        // Draw the line of sight representation within the scene window
        public override void OnDrawGizmos()
        {
            if (fieldOfViewAngle == null || viewDistance == null)
            {
                return;
            }
            
            MovementUtility.DrawLineOfSight(Owner.transform, Vector3.zero, fieldOfViewAngle.Value, viewDistance.Value, false);
        }
    }
}




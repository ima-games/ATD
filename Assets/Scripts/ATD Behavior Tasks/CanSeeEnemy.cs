using BehaviorDesigner.Runtime.Tasks.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
    [TaskDescription("看到视野范围内的敌人")]
    [TaskCategory("Movement")]
    [TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}CanSeeObjectIcon.png")]
    public class CanSeeEnemy : Conditional
    {
        public SharedTransform targetObject;
        public LayerMask objectLayerMask;
        public SharedFloat fieldOfViewAngle = 90;
        public SharedFloat viewDistance = 1000;
        public SharedVector3 offset;
        public SharedTransform objectInSight;

        // Returns success if an object was found otherwise failure
        public override TaskStatus OnUpdate()
        {
            objectInSight.Value = MovementUtility.WithinSight(transform, offset.Value, fieldOfViewAngle.Value, viewDistance.Value, objectLayerMask, "Enemy");
          

            if (objectInSight.Value != null)
            {
                // Return success if an object was found
                return TaskStatus.Success;
            }
            // An object is not within sight so return failure
            return TaskStatus.Failure;
        }

        // Reset the public variables
        public override void OnReset()
        {
            fieldOfViewAngle = 90;
            viewDistance = 1000;
            offset = Vector3.zero;
        }

        // Draw the line of sight representation within the scene window
        public override void OnDrawGizmos()
        {
            if (fieldOfViewAngle == null || viewDistance == null)
            {
                return;
            }
            
            MovementUtility.DrawLineOfSight(Owner.transform, offset.Value, fieldOfViewAngle.Value, viewDistance.Value, false);
        }
    }
}




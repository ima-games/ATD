using UnityEngine;
using System.Collections.Generic;

namespace BehaviorDesigner.Runtime.Tasks.Movement
{
    [TaskDescription("Check to see if the any objects are within sight of the agent.")]
    [TaskCategory("Movement")]
    [HelpURL("http://www.opsive.com/assets/BehaviorDesigner/Movement/documentation.php?id=11")]
    [TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}CanSeeObjectIcon.png")]
    public class CanSeeObject : Conditional
    {
#if !(UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2)
        [Tooltip("Should the 2D version be used?")]
        public bool usePhysics2D;
#endif
        [Tooltip("The object that we are searching for. If this value is null then the objectLayerMask will be used")]
        public SharedTransform targetObject;
        [Tooltip("The LayerMask of the objects that we are searching for")]
        public LayerMask objectLayerMask;
        [Tooltip("The field of view angle of the agent (in degrees)")]
        public SharedFloat fieldOfViewAngle = 90;
        [Tooltip("The distance that the agent can see ")]
        public SharedFloat viewDistance = 1000;
        [Tooltip("The offset relative to the pivot position")]
        public SharedVector3 offset;
        [Tooltip("The object that is within sight")]
        public SharedTransform objectInSight;

        // Returns success if an object was found otherwise failure
        public override TaskStatus OnUpdate()
        {
#if !(UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2)
            if (usePhysics2D) {
                // If the target object is null then determine if there are any objects within sight based on the layer mask
                if (targetObject.Value == null) {
                    objectInSight.Value = MovementUtility.WithinSight2D(transform, offset.Value, fieldOfViewAngle.Value, viewDistance.Value, objectLayerMask);
                } else { // If the target is not null then determine if that object is within sight
                    objectInSight.Value = MovementUtility.WithinSight2D(transform, offset.Value, fieldOfViewAngle.Value, viewDistance.Value, targetObject.Value);
                }
            } else {
#endif
                // If the target object is null then determine if there are any objects within sight based on the layer mask
                if (targetObject.Value == null) {
                    objectInSight.Value = MovementUtility.WithinSight(transform, offset.Value, fieldOfViewAngle.Value, viewDistance.Value, objectLayerMask);
                } else { // If the target is not null then determine if that object is within sight
                    objectInSight.Value = MovementUtility.WithinSight(transform, offset.Value, fieldOfViewAngle.Value, viewDistance.Value, targetObject.Value);
                }
#if !(UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2)
            }
#endif
            if (objectInSight.Value != null) {
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
            if (fieldOfViewAngle == null || viewDistance == null) {
                return;
            }

            bool is2D = false;
#if !(UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2)
            is2D = usePhysics2D;
#endif
            MovementUtility.DrawLineOfSight(Owner.transform, offset.Value, fieldOfViewAngle.Value, viewDistance.Value, is2D);
        }
    }
}
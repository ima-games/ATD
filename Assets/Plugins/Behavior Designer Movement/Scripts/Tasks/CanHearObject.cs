using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Movement
{
    [TaskDescription("Check to see if the any objects are within hearing range of the current agent.")]
    [TaskCategory("Movement")]
    [HelpURL("http://www.opsive.com/assets/BehaviorDesigner/Movement/documentation.php?id=12")]
    [TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}CanHearObjectIcon.png")]
    public class CanHearObject : Conditional
    {
#if !(UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2)
        [Tooltip("Should the 2D version be used?")]
        public bool usePhysics2D;
#endif
        [Tooltip("The object that we are searching for. If this value is null then the objectLayerMask will be used")]
        public SharedTransform targetObject;
        [Tooltip("The LayerMask of the objects that we are searching for")]
        public LayerMask objectLayerMask;
        [Tooltip("How far away the unit can hear")]
        public SharedFloat hearingRadius = 50;
        [Tooltip("The furtuer away a sound source is the less likely the agent will be able to hear it. " +
                 "Set a threshold for the the minimum audibility level that the agent can hear")]
        public SharedFloat linearAudibilityThreshold = 0.05f;
        [Tooltip("The offset relative to the pivot position")]
        public SharedVector3 offset;
        [Tooltip("The returned object that is heard")]
        public SharedTransform objectHeard;

        // Returns success if an object was found otherwise failure
        public override TaskStatus OnUpdate()
        {
            // If the target object is null then determine if there are any objects within hearing range based on the layer mask
            if (targetObject.Value == null) {
#if !(UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2)
                if (usePhysics2D) {
                    objectHeard.Value = MovementUtility.WithinHearingRange2D(transform, offset.Value, linearAudibilityThreshold.Value, hearingRadius.Value, objectLayerMask);
                } else {
#endif
                    objectHeard.Value = MovementUtility.WithinHearingRange(transform, offset.Value, linearAudibilityThreshold.Value, hearingRadius.Value, objectLayerMask);
#if !(UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2)
                }
#endif
            } else { // If the target is not null then determine if that object is within sight
                objectHeard.Value = MovementUtility.WithinHearingRange(transform, offset.Value, linearAudibilityThreshold.Value, targetObject.Value);
            }
            if (objectHeard.Value != null) {
                // Return success if an object was heard
                return TaskStatus.Success;
            }
            // An object is not within heard so return failure
            return TaskStatus.Failure;
        }

        // Reset the public variables
        public override void OnReset()
        {
            hearingRadius = 50;
            linearAudibilityThreshold = 0.05f;
        }

        // Draw the hearing radius
        public override void OnDrawGizmos()
        {
#if UNITY_EDITOR
            if (hearingRadius == null) {
                return;
            }
            var oldColor = UnityEditor.Handles.color;
            UnityEditor.Handles.color = Color.yellow;
            UnityEditor.Handles.DrawWireDisc(Owner.transform.position, Owner.transform.up, hearingRadius.Value);
            UnityEditor.Handles.color = oldColor;
#endif
        }
    }
}
using BehaviorDesigner.Runtime.Tasks.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
    [TaskDescription("看到视野范围内的怪物")]
    [TaskCategory("ATDConditional")]
    [TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}CanSeeObjectIcon.png")]
    public class CanSeeEnemy : Conditional
    {

        [Tooltip("Should the 2D version be used?")]
        public bool usePhysics2D;
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
            //寄主势力
            Individual.Power masterPower = gameObject.GetComponent<Individual>().power;
            //计算出对应的敌对势力
            Individual.Power enemyPower = Individual.Power.Neutral;
            switch (masterPower)
            {
                case Individual.Power.Monster:enemyPower = Individual.Power.Human; break;
                case Individual.Power.Human: enemyPower = Individual.Power.Monster; break;
            }

            if (usePhysics2D)
            {
                //单个目标对象来筛选敌对目标
                if (targetObject.IsShared)
                {
                    if (targetObject.Value)
                    {
                        objectInSight.Value = MovementUtility.WithinSight2D(transform, offset.Value, fieldOfViewAngle.Value, viewDistance.Value, targetObject.Value);
                    }
                }
                //个体层+敌对势力来筛选敌对目标
                else
                {
                    objectInSight.Value = ExtendedMovementUtility.WithinSight2D(transform, offset.Value, fieldOfViewAngle.Value, viewDistance.Value, objectLayerMask, enemyPower);
                }
            }
            else
            {
                //单个目标对象来筛选敌对目标
                if (targetObject.IsShared)
                {
                    if (targetObject.Value)
                    {
                        objectInSight.Value = MovementUtility.WithinSight(transform, offset.Value, fieldOfViewAngle.Value, viewDistance.Value, targetObject.Value);
                    }
                }
                //个体层+敌对势力来筛选敌对目标
                else
                {
                    objectInSight.Value = ExtendedMovementUtility.WithinSight(transform, offset.Value, fieldOfViewAngle.Value, viewDistance.Value, objectLayerMask, enemyPower);
                }
            }

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

            bool is2D = false;
            is2D = usePhysics2D;

            MovementUtility.DrawLineOfSight(Owner.transform, offset.Value, fieldOfViewAngle.Value, viewDistance.Value, is2D);
        }
    }
}
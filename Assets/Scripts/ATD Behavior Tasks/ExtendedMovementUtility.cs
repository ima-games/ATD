using UnityEngine;
using BehaviorDesigner.Runtime.Tasks.Movement;


namespace BehaviorDesigner.Runtime.Tasks.Movement
{
    public static class ExtendedMovementUtility
    {
        /// <summary>
        /// 检测对应layer且对应势力的对象
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="positionOffset"></param>
        /// <param name="fieldOfViewAngle"></param>
        /// <param name="viewDistance"></param>
        /// <param name="objectLayerMask"></param>
        /// <param name="势力"></param>
        /// <returns></returns>
        public static Transform WithinSight2D(Transform transform, Vector3 positionOffset, float fieldOfViewAngle, float viewDistance, LayerMask objectLayerMask, Individual.Power power)
        {
            Transform objectFound = null;
            var hitColliders = Physics2D.OverlapCircleAll(transform.position, viewDistance, objectLayerMask);
            if (hitColliders != null)
            {
                for (int i = 0; i < hitColliders.Length; ++i)
                {
                    Transform obj;
                    //检测势力是否一致
                    if (hitColliders[i].gameObject.GetComponent<Individual>().power != power)
                    {
                        continue;
                    }
                    // Call the 2D WithinSight function to determine if this specific object is within sight
                    if ((obj = MovementUtility.WithinSight(transform, positionOffset, fieldOfViewAngle, viewDistance, hitColliders[i].transform)) != null)
                    {
                        objectFound = obj;
                    }
                }
            }
            return objectFound;
        }


        /// <summary>
        ///  检测对应layer且对应势力的对象
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="positionOffset"></param>
        /// <param name="fieldOfViewAngle"></param>
        /// <param name="viewDistance"></param>
        /// <param name="objectLayerMask"></param>
        /// <param name="势力"></param>
        /// <returns></returns>
        public static Transform WithinSight(Transform transform, Vector3 positionOffset, float fieldOfViewAngle, float viewDistance, LayerMask objectLayerMask, Individual.Power power)
        {
            Transform objectFound = null;
            var hitColliders = Physics.OverlapSphere(transform.position, viewDistance, objectLayerMask);
            if (hitColliders != null)
            {
                for (int i = 0; i < hitColliders.Length; ++i)
                {
                    Transform obj;
                    //检测势力是否一致
                    if (hitColliders[i].gameObject.GetComponent<Individual>().power != power)
                    {
                        continue;
                    }
                    // Call the WithinSight function to determine if this specific object is within sight
                    if ((obj = MovementUtility.WithinSight(transform, positionOffset, fieldOfViewAngle, viewDistance, hitColliders[i].transform)) != null)
                    {
                        objectFound = obj;
                    }
                }
            }
            return objectFound;
        }
    }
}
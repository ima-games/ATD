using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
    [TaskDescription("火焰塔的攻击行为")]
    [TaskCategory("Tower Attack Model")]
    [TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}FireBallIcon.png")]
    public class FireBall : Action
    {
        public SharedTransform target;
        public GameObject bullet;
        public GameObject bulletPoint;
        public float bulletSpeed;
        public float attackRate;

        private Individual master;          //寄主
        private bool attacking = false;     //攻击正在进行中
    }
}



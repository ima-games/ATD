using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftArmAnimatorFix : MonoBehaviour
{
    private Animator animator;
    public Vector3 a;

    void Awake() {
        animator = GetComponent<Animator>();
    }

    void OnAnimatorIK() {
        if (animator.GetBool("defense") == false) {
            Transform lefLowerArm = animator.GetBoneTransform(HumanBodyBones.LeftLowerArm);
            lefLowerArm.localEulerAngles += a;
            animator.SetBoneLocalRotation(HumanBodyBones.LeftLowerArm, Quaternion.Euler(lefLowerArm.localEulerAngles));
        }
    }
}

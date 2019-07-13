using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndividualModelControll : MonoBehaviour
{
    private Animator animator;
    private PlayerController plac;
    public Vector3 a;
    public WeaponTriggerEvent weapon;

    void Awake() {
        animator = GetComponent<Animator>();
        plac = GetComponentInParent<PlayerController>();
    }

    void OnAnimatorMove()
    {
        SendMessageUpwards("OnUpdateRM", animator.deltaPosition);

    }

    public void ResetTrigger(string triggerName) {
        animator.ResetTrigger(triggerName);
    }

    void StartAttack()
    {
        weapon.StartAttack();
    }

    void EndAttack()
    {
        weapon.EndAttack();
    }

    void OnAnimatorIK()
    {
        if (plac.leftIsShield)
        {
            if (animator.GetBool("defense") == false)
            {
                Transform lefLowerArm = animator.GetBoneTransform(HumanBodyBones.LeftLowerArm);
                lefLowerArm.localEulerAngles += a;
                animator.SetBoneLocalRotation(HumanBodyBones.LeftLowerArm, Quaternion.Euler(lefLowerArm.localEulerAngles));
            }
        }
    }
}

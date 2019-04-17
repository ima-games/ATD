using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepBool : StateMachineBehaviour {
    public string boolName;
    public bool status;
    public bool resetOnExit = true;
    override public void OnStateUpdate (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.SetBool (boolName, status);
    }

    public override void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (resetOnExit) {
            animator.SetBool (boolName, !status);
        }
    }
}
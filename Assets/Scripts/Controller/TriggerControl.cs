using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerControl : MonoBehaviour
{
    private Animator animator;

    void Awake() {
        animator = GetComponent<Animator>();
    }

    public void ResetTrigger(string triggerName) {
        animator.ResetTrigger(triggerName);
    }
}

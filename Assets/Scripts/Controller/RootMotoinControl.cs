using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootMotoinControl : MonoBehaviour
{
    private Animator animator;

    void Awake() {
        animator = GetComponent<Animator>();
    }

    void OnAnimatorMove() {
        SendMessageUpwards("OnUpdateRM",animator.deltaPosition);

    }
}

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

    //TODO : 暂时消除攻击动画播放时缺失事件的错误提示
    void ClearSignalAttack()
    {

    }
}

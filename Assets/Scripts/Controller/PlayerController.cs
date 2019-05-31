using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public GameObject model;
    public PlayerInput playerInput;
    public LockController lockController;
    public float walkSpeed = 1.6f;
    public float runMultiplier = 2.4f;
    public float jumpVelocity = 5.0f;
    public float rollVeticalVelocity = 0f;
    [Header ("动画平滑系数")]
    public float rotateRatio = 0.3f; //转身
    public float runRatio = 0.3f; //切换奔跑

    [Space (10)]
    [Header ("摩擦力设定")]
    public PhysicMaterial frictionOne;
    public PhysicMaterial frictionZero;

    [SerializeField]
    private Animator animator;

    private new Rigidbody rigidbody;
    private CapsuleCollider capsuleCollider;
    private Vector3 planeVec;
    private Vector3 thrustVec;
    private bool canAttack;
    private bool lockPlane = false;
    private bool trackDirection = false;
    //private float lerpTarget;
    private Vector3 deltaPos;
    public bool leftIsShield = true;

    void Awake () {
        animator = model.GetComponent<Animator> ();
        rigidbody = GetComponent<Rigidbody> ();
        capsuleCollider = GetComponent<CapsuleCollider> ();
    }

    void Update () {

        if (playerInput.lockon) {
            lockController.LockSwitch ();
        }

        if (lockController.lockState == false) {
            float targetRunMulti = ((playerInput.run) ? 2.0f : 1.0f);
            animator.SetFloat ("forward", playerInput.Dmag *
                Mathf.Lerp (animator.GetFloat ("forward"), targetRunMulti, runRatio));
        } else {
            Vector3 localDevc = transform.InverseTransformVector (playerInput.Dvec);
            animator.SetFloat ("forward", localDevc.z * ((playerInput.run) ? 2.0f : 1.0f));
            animator.SetFloat ("right", localDevc.x * ((playerInput.run) ? 2.0f : 1.0f));
        }

        //animator.SetBool ("defense", playerInput.defense);

        if (playerInput.roll || rigidbody.velocity.magnitude > 7f) {
            animator.SetTrigger ("roll");
            canAttack = false;
        }

        if (playerInput.jump) {
            animator.SetTrigger ("jump");
            canAttack = false;
        }

        if ((playerInput.lHand || playerInput.rHand) && (CheckState ("ground") || CheckStateTag ("attack")) && canAttack) {
            if (playerInput.rHand) {
                animator.SetBool ("R0L1", false);
                animator.SetTrigger ("attack");
            } else if (playerInput.lHand && !leftIsShield) {
                animator.SetBool ("R0L1", true);
                animator.SetTrigger ("attack");
            }
        }

        if (leftIsShield) {
            if (CheckState ("ground")) {
                animator.SetBool ("defense", playerInput.defense);
                animator.SetLayerWeight (animator.GetLayerIndex ("defense"), 1);
            } else {
                animator.SetBool("defense",false);
                //animator.SetLayerWeight (animator.GetLayerIndex ("defense"), 0);
            }
        } else {
            animator.SetLayerWeight (animator.GetLayerIndex ("defense"), 0);
        }

        if (lockController.lockState == false) {
            if (playerInput.Dmag > 0.1f) //转身硬直
            {
                Vector3 targetForward = Vector3.Slerp (model.transform.forward, playerInput.Dvec, rotateRatio);
                model.transform.forward = targetForward;
            }

            if (lockPlane == false) {
                planeVec = playerInput.Dmag * model.transform.forward * walkSpeed *
                    ((playerInput.run) ? runMultiplier : 1.0f);
            }
        } else {
            if (trackDirection == false) {
                model.transform.forward = transform.forward;
            } else {
                model.transform.forward = planeVec.normalized;
            }
            if (lockPlane == false) {
                planeVec = playerInput.Dvec * walkSpeed * ((playerInput.run) ? runMultiplier : 1.0f);
            }
        }
    }

    void FixedUpdate () {
        rigidbody.position += deltaPos;
        rigidbody.velocity = new Vector3 (planeVec.x, rigidbody.velocity.y, planeVec.z) + thrustVec;
        thrustVec = Vector3.zero;
        deltaPos = Vector3.zero;
    }

    /// <summary>
    /// 询问当前是否为此层级中的此状态
    /// </summary>
    /// <param name="stateName">所查询状态名</param>
    /// <param name="layerName">所查询层级名</param>
    /// <returns></returns>
    private bool CheckState (string stateName, string layerName = "Base Layer") {
        return animator.GetCurrentAnimatorStateInfo (animator.GetLayerIndex (layerName)).IsName (stateName);
    }

    private bool CheckStateTag (string tagName, string layerName = "Base Layer") {
        return animator.GetCurrentAnimatorStateInfo (animator.GetLayerIndex (layerName)).IsTag (tagName);
    }

    #region 动画事件信息
    public void OnJumpEnter () {
        thrustVec = new Vector3 (0, jumpVelocity, 0);
        playerInput.inputEnabled = false;
        lockPlane = true;
        trackDirection = true;
    }
    public void IsGround () {
        //print("IsGround");
        animator.SetBool ("isGround", true);
    }
    public void IsNotGround () {
        //print("IsNotGround");
        animator.SetBool ("isGround", false);
    }
    public void OnGroundEnter () {
        playerInput.inputEnabled = true;
        lockPlane = false;
        canAttack = true;
        capsuleCollider.material = frictionOne;
        trackDirection = false;
    }
    public void OnGroundExit () {
        capsuleCollider.material = frictionZero;
    }
    public void OnFallEnter () {
        playerInput.inputEnabled = false;
        lockPlane = true;
    }
    public void OnRollEnter () {
        thrustVec = new Vector3 (0, rollVeticalVelocity, 0);
        playerInput.inputEnabled = false;
        lockPlane = true;
        trackDirection = true;
    }
    public void OnJabEnter () {
        playerInput.inputEnabled = false;
        lockPlane = true;
    }
    public void OnJabUpdate () {
        thrustVec = model.transform.forward * animator.GetFloat ("jabVelocity");
    }
    public void OnAttack1hAEnter () {
        playerInput.inputEnabled = false;
        // lockPlane = true;
        // lerpTarget = 1.0f;
    }
    public void OnAttack1hAUpdate () {
        thrustVec = model.transform.forward * animator.GetFloat ("attack1aAVelocity");
        // float currentweight = animator.GetLayerWeight (animator.GetLayerIndex ("attack"));
        // currentweight = Mathf.Lerp (currentweight, lerpTarget, 0.1f); //idle切换到攻击1hA
        // animator.SetLayerWeight (animator.GetLayerIndex ("attack"), currentweight);
    }
    // public void OnAttackIdleEnter () {
    //     playerInput.inputEnabled = true;
    //     // lockPlane = false;
    //     // animator.SetLayerWeight(animator.GetLayerIndex("attack"), 0f);
    //     // lerpTarget = 0f;
    // }

    // public void OnAttackIdleUpdate () {
    //     // float currentweight = animator.GetLayerWeight (animator.GetLayerIndex ("attack"));
    //     // currentweight = Mathf.Lerp (currentweight, lerpTarget, 0.1f); //攻击完切换到idle
    //     // animator.SetLayerWeight (animator.GetLayerIndex ("attack"), currentweight);
    // }
    #endregion

    #region AnimationEvent
    public void OnUpdateRM (object _deltaPos) {
        if (CheckState ("attack1hC")) {
            //deltaPos += (Vector3)_deltaPos;
            deltaPos += (0.8f * deltaPos + 0.2f * (Vector3) _deltaPos) / 1.0f;
        }
    }
    #endregion
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject model;
    public PlayerInput playerInput;
    public float walkSpeed = 1.6f;
    public float runMultiplier = 2.4f;
    public float jumpVelocity = 5.0f;
    public float rollHorizontalVelocity = 0f;
    [Header("动画平滑系数")]
    public float rotateRatio = 0.3f;//转身
    public float runRatio = 0.3f;//切换奔跑

    [Space(10)]
    [Header("摩擦力设定")]
    public PhysicMaterial frictionOne;
    public PhysicMaterial frictionZero;

    [SerializeField]
    private Animator animator;

    private Rigidbody rigidbody;
    private CapsuleCollider capsuleCollider;
    private Vector3 planeVec;
    private Vector3 thrustVec;
    private bool canAttack;
    private bool lockPlane = false;
    private float lerpTarget;
    private Vector3 deltaPos;

    void Awake() {
        playerInput = GetComponent<PlayerInput>();
        animator = model.GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();//if(rigib == null){}
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    void Update() {
        float targetRunMulti = ((playerInput.run) ? 2.0f : 1.0f);
        animator.SetFloat("forward", playerInput.Dmag *
            Mathf.Lerp(animator.GetFloat("forward"), targetRunMulti, runRatio));

        if (rigidbody.velocity.magnitude > 1.0f) {
            animator.SetTrigger("roll");
        }

        if (playerInput.jump) {
            animator.SetTrigger("jump");
            canAttack = false;
        }

        if (playerInput.attack && CheckState("ground") && canAttack) {
            animator.SetTrigger("attack");
        }

        if (playerInput.Dmag > 0.01f)//转身硬直
        {
            Vector3 targetForward = Vector3.Slerp(model.transform.forward, playerInput.Dvec, rotateRatio);
            model.transform.forward = targetForward;
        }

        if (lockPlane == false) {
            planeVec = playerInput.Dmag * model.transform.forward * walkSpeed *
              ((playerInput.run) ? runMultiplier : 1.0f);
        }
        //print(CheckState("idle", "attack"));
    }

    void FixedUpdate() {
        rigidbody.position += deltaPos;
        rigidbody.velocity = new Vector3(planeVec.x, rigidbody.velocity.y, planeVec.z) + thrustVec;
        thrustVec = Vector3.zero;
        deltaPos = Vector3.zero;
    }

    /// <summary>
    /// 询问当前是否为此层级中的此状态
    /// </summary>
    /// <param name="stateName">所查询状态名</param>
    /// <param name="layerName">所查询层级名</param>
    /// <returns></returns>
    private bool CheckState(string stateName, string layerName = "Base Layer") {//mark
        int layerIndex = animator.GetLayerIndex(layerName);
        return animator.GetCurrentAnimatorStateInfo(layerIndex).IsName(stateName);
    }

    #region AnimationEvent
    public void OnUpdateRM(object _deltaPos) {
        if(CheckState("attack1hC","attack")) {
            deltaPos += (Vector3)_deltaPos;
        }
    }
    #endregion

    #region 动画事件信息
    public void OnJumpEnter() {
        thrustVec = new Vector3(0, jumpVelocity, 0);
        playerInput.inputEnabled = false;
        lockPlane = true;
    }
    public void IsGround() {
        //print("IsGround");
        animator.SetBool("isGround", true);
    }
    public void IsNotGround() {
        //print("IsNotGround");
        animator.SetBool("isGround", false);
    }
    public void OnGroundEnter() {
        playerInput.inputEnabled = true;
        lockPlane = false;
        canAttack = true;
        capsuleCollider.material = frictionOne;
    }
    public void OnGroundExit() {
        capsuleCollider.material = frictionZero;
    }
    public void OnFallEnter() {
        playerInput.inputEnabled = false;
        lockPlane = true;
    }
    public void OnRollEnter() {
        thrustVec = new Vector3(0, rollHorizontalVelocity, 0);
        playerInput.inputEnabled = false;
        lockPlane = true;
    }
    public void OnJabEnter() {
        playerInput.inputEnabled = false;
        lockPlane = true;
    }
    public void OnJabUpdate() {
        thrustVec = model.transform.forward * animator.GetFloat("jabVelocity");
    }
    public void OnAttack1hAEnter() {
        playerInput.inputEnabled = false;
        lerpTarget = 1.0f;
    }
    public void OnAttack1hAUpdate() {
        thrustVec = model.transform.forward * animator.GetFloat("attack1aAVelocity");
        float currentweight = animator.GetLayerWeight(animator.GetLayerIndex("attack"));
        currentweight = Mathf.Lerp(currentweight, lerpTarget, 0.1f);//idle切换到攻击1hA
        animator.SetLayerWeight(animator.GetLayerIndex("attack"), currentweight);
    }
    public void OnAttackIdleEnter() {
        playerInput.inputEnabled = true;
        //animator.SetLayerWeight(animator.GetLayerIndex("attack"), 0f);
        lerpTarget = 0f;
    }
    public void OnAttackIdleUpdate() {
        float currentweight = animator.GetLayerWeight(animator.GetLayerIndex("attack"));
        currentweight = Mathf.Lerp(currentweight, lerpTarget, 0.1f);//攻击完切换到idle
        animator.SetLayerWeight(animator.GetLayerIndex("attack"), currentweight);
    }
    #endregion
}

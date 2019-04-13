using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject model;
    public PlayerInput playerInput;
    public float walkSpeed = 1.0f;
    public float runMultiplier = 2.0f;
    public float jumpVelocity = 4.0f;
    public float rollVelocity = 3.0f;
    [Header("动画平滑系数")]
    public float rotateRatio = 0.3f;//转身
    public float runRatio = 0.3f;//切换奔跑

    [Header("FrictionSetting")]
    public PhysicMaterial frictionOne;
    public PhysicMaterial frictionZero;

    [SerializeField]
    private Animator animator;
    private Rigidbody rigidbody;
    private Vector3 planeVec;
    private Vector3 thrustVec;
    private bool canAttack;
    private CapsuleCollider capsuleCollider;

    private bool lockPlane = false;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        animator = model.GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();//if(rigib == null){}
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    void Update()
    {
        float targetRunMulti = ((playerInput.run) ? 2.0f : 1.0f);
        animator.SetFloat("forward", playerInput.Dmag *
            Mathf.Lerp(animator.GetFloat("forward"), targetRunMulti, runRatio));

        if (rigidbody.velocity.magnitude > 1.0f)
        {
            animator.SetTrigger("roll");
        }

        if (playerInput.jump)
        {
            animator.SetTrigger("jump");
            canAttack = false;
        }

        if (playerInput.attack && CheckState("ground") && canAttack)
        {
            animator.SetTrigger("attack");
        }

        if (playerInput.Dmag > 0.01f)//转身硬直
        {
            Vector3 targetForward = Vector3.Slerp(model.transform.forward, playerInput.Dvec, rotateRatio);
            model.transform.forward = targetForward;
        }

        if (lockPlane == false)
        {
            planeVec = playerInput.Dmag * model.transform.forward * walkSpeed *
              ((playerInput.run) ? runMultiplier : 1.0f);
        }
        //print(CheckState("idle", "attack"));
    }

    void FixedUpdate()
    {
        rigidbody.velocity = new Vector3(planeVec.x, rigidbody.velocity.y, planeVec.z) + thrustVec;
        thrustVec = Vector3.zero;
    }

    private bool CheckState(string stateName, string layerName = "Base Layer")
    {
        int layerIndex = animator.GetLayerIndex(layerName);
        return animator.GetCurrentAnimatorStateInfo(layerIndex).IsName(stateName);
    }

    #region 信息
    public void OnJumpEnter()
    {
        thrustVec = new Vector3(0, jumpVelocity, 0);
        playerInput.inputEnabled = false;
        lockPlane = true;
    }
    public void IsGround()
    {
        //print("IsGround");
        animator.SetBool("isGround", true);
    }
    public void IsNotGround()
    {
        //print("IsNotGround");
        animator.SetBool("isGround", false);
    }
    public void OnGroundEnter()
    {
        playerInput.inputEnabled = true;
        lockPlane = false;
        canAttack = true;
        capsuleCollider.material = frictionOne;
    }
    public void OnGroundExit()
    {
        capsuleCollider.material = frictionZero;
    }
    public void OnFallEnter()
    {
        playerInput.inputEnabled = false;
        lockPlane = true;
    }
    public void OnRollEnter()
    {
        thrustVec = new Vector3(0, rollVelocity, 0);
        playerInput.inputEnabled = false;
        lockPlane = true;
    }
    public void OnJabEnter()
    {
        playerInput.inputEnabled = false;
        lockPlane = true;
    }
    public void OnJabUpdate()
    {
        thrustVec = model.transform.forward * animator.GetFloat("jabVelocity");
    }
    public void OnAttack1hAEnter()
    {
        playerInput.inputEnabled = false;
        animator.SetLayerWeight(animator.GetLayerIndex("attack"), 1.0f);
    }
    public void OnAttack1hAUpdate()
    {
        thrustVec = model.transform.forward * animator.GetFloat("attack1aAVelocity");
    }
    public void OnAttackIdle()
    {
        playerInput.inputEnabled = true;
        animator.SetLayerWeight(animator.GetLayerIndex("attack"), 0.0f);
    }
    #endregion
}

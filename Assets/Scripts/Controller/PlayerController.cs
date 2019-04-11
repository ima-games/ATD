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

    [Header("动画平滑系数")]
    public float rotateRatio = 0.3f;//转身
    public float runRatio = 0.3f;//切换奔跑

    [SerializeField]
    private Animator animator;
    private Rigidbody rigib;
    private Vector3 planeVec;
    private Vector3 thrustVec;

    private bool lockPlane = false;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        animator = model.GetComponent<Animator>();
        rigib = GetComponent<Rigidbody>();//if(rigib == null){}
    }

    void Update()
    {
        float targetRunMulti = ((playerInput.run) ? 2.0f : 1.0f);
        animator.SetFloat("forward", playerInput.Dmag *
            Mathf.Lerp(animator.GetFloat("forward"), targetRunMulti, runRatio));
        if (playerInput.jump)
        {
            animator.SetTrigger("jump");
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
    }

    void FixedUpdate()
    {
        //rigib.position += movingVec * Time.fixedDeltaTime;
        rigib.velocity = new Vector3(planeVec.x, rigib.velocity.y, planeVec.z) + thrustVec;
        thrustVec = Vector3.zero;
    }

    /// <summary>
    /// 信息
    /// </summary>
    public void OnJumpEnter()
    {
        thrustVec = new Vector3(0, jumpVelocity, 0);
        playerInput.inputEnabled = false;
        lockPlane = true;
    }
    public void IsGround()
    {
        print("IsGround");
        animator.SetBool("isGround", true);
    }
    public void IsNotGround()
    {
        print("IsNotGround");
        animator.SetBool("isGround", false);
    }
    public void OnGroundEnter()
    {
        playerInput.inputEnabled = true;
        lockPlane = false;
    }
    public void OnFallEnter()
    {
        playerInput.inputEnabled = false;
        lockPlane = true;
    }
}

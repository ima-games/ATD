using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPScontrollerChara : MonoBehaviour
{
    Vector3 movetoPos = Vector3.zero;//当前移动方向
    public GameObject mycamera;
    public bool devilMayCry = false;
    public float moveSpeed = 5.0f;
    public float visonSpeed = 0.2f;
    public float jumpForce = 15;
    public float fallingSpeed = 9.8f;
    public float rollCD = 0.5f;
    public float dodgeCD = 0.5f;
    public float landCD = 0.3f;
    //[Header("闪避加速")]
    //public bool dodgeboost = true;
    //[Header("闪避加速倍率")]
    //public float dodgeboostrate = 1.5f;

    //Inputs
    bool inputJump;
    bool inputAttack1;
    bool inputAttack2;
    bool inputUnarmed;
    bool inputShield;
    bool inputAiming;
    bool inputRoll;
    bool inputDodge;
    bool inputMoving;
    float inputHorizontal = 0;
    float inputVertical = 0;

    //Weapon Models
    public GameObject twoHandAxe;
    public GameObject twoHandSword;
    public GameObject twoHandSpear;
    public GameObject twoHandBow;
    public GameObject twoHandCrossbow;
    public GameObject twoHandClub;
    public GameObject staff;
    public GameObject swordL;
    public GameObject swordR;
    public GameObject maceL;
    public GameObject maceR;
    public GameObject daggerL;
    public GameObject daggerR;
    public GameObject itemL;
    public GameObject itemR;
    public GameObject shield;
    public GameObject pistolL;
    public GameObject pistolR;
    public GameObject rifle;
    public GameObject spear;
    public bool instantWeaponSwitch;

    CharacterController controller;
    Animator animator;
    MessageSystem messagesystem;
    Individual individual;
    int isAboveHash = Animator.StringToHash("IsAbove");

    private new Rigidbody rigidbody;

    #region camera fllow
    Vector3 speedDir;
    Vector3 worldSpeedDir;
    Vector2 nowPos;
    Vector2 LastPos;
    float nowAngle_y;
    Vector3 basePos0;
    Vector3 basePos1;
    Vector3 basePos2;
    Vector3 basePos3;
    Vector3 basePos4;
    Vector3 basePos5;
    Vector3 basePos6;
    Vector3 cmaPos;
    Ray baser0;
    Ray baser1;
    Ray baser2;
    Ray baser3;
    Ray baser4;
    Ray baser5;
    Ray baser6;
    GameObject onCmaObj;
    List<GameObject> onCmaCollision;
    List<GameObject> onRemoveCollision;
    LayerMask mask;
    LayerMask mask2;
    LayerMask mask3;
    RaycastHit onrcHit;
    Color lastColor;
    void cmaDirUpdateSet()//根据鼠标设置Cma的位置
    {

        if (!Cursor.visible)//鼠标不可见时
        {
            nowPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            float rotate_x = (nowPos.x - LastPos.x) * visonSpeed;
            float rotate_y = (LastPos.y - nowPos.y) * visonSpeed;
            LastPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            //完成Player x_横向旋转
            gameObject.transform.Rotate(new Vector3(0, rotate_x, 0));
            if ((nowAngle_y + rotate_y) > 30f || nowAngle_y + rotate_y < -90f)
            {
                return;
            }
            nowAngle_y += rotate_y;
            mycamera.transform.Rotate(new Vector3(rotate_y, 0, 0));
            //视角旋转 摄像机位置变换数据


            //非FPS型
            if (nowAngle_y > -30f && nowAngle_y <= 30f)
            {
                mycamera.transform.localPosition = new Vector3(0, -0.75f + 3.75f * ((nowAngle_y + 30f) / 60f), -5f + 1f * ((nowAngle_y + 30f) / 60f));
            }
            else
            {
                mycamera.transform.localPosition = new Vector3(0, -0.75f * ((nowAngle_y + 90f) / 60f), -0.75f - 4.25f * ((nowAngle_y + 90f) / 60f));
            }

            //FPS型

            //if (nowAngle_y > -60f && nowAngle_y <= 30f)
            //{
            //  mycamera.transform.localPosition = new Vector3(0, -2f + 5f * ((nowAngle_y + 60f) / 90f), -3f - 1f * ((nowAngle_y + 60f) / 90f));
            //}
            //else
            //{
            //  mycamera.transform.localPosition = new Vector3(0, -2f * ((nowAngle_y + 90f) / 30f), -0.75f - 3.25f * ((nowAngle_y + 90f) / 30f));
            //}
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                Cursor.visible = false;
                LastPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            }
        }
    }
    void shelterCheckUpdate()//遮挡透明化处理
    {
        basePos0 = gameObject.transform.position;
        basePos1 = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 1f, gameObject.transform.position.z);
        basePos2 = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 0.8f, gameObject.transform.position.z);
        basePos3 = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 2f, gameObject.transform.position.z);
        basePos4 = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 1.5f, gameObject.transform.position.z);
        basePos5 = new Vector3(gameObject.transform.position.x + 0.5f, gameObject.transform.position.y + 1.5f, gameObject.transform.position.z);
        basePos6 = new Vector3(gameObject.transform.position.x - 0.5f, gameObject.transform.position.y + 1.5f, gameObject.transform.position.z);

        cmaPos = mycamera.transform.position;

        baser0 = new Ray(cmaPos, basePos0 - cmaPos);
        baser1 = new Ray(cmaPos, basePos1 - cmaPos);
        baser2 = new Ray(cmaPos, basePos2 - cmaPos);
        baser3 = new Ray(cmaPos, basePos3 - cmaPos);
        baser4 = new Ray(cmaPos, basePos4 - cmaPos);
        baser5 = new Ray(cmaPos, basePos5 - cmaPos);
        baser6 = new Ray(cmaPos, basePos6 - cmaPos);
        //checkOnCmaCollision
        for (int i = 0; i < onCmaCollision.Count; i++)
        {
            onCmaObj = onCmaCollision[i];
            if (null == onCmaObj)
            {
                onRemoveCollision.Add(onCmaObj);
                continue;
            }
            onCmaObj.layer = 11;
            if (Physics.Raycast(baser0, out onrcHit, Vector3.Distance(cmaPos, basePos0), mask3))
            {
                onCmaObj.layer = 10;
                continue;
            }
            if (Physics.Raycast(baser1, out onrcHit, Vector3.Distance(cmaPos, basePos1), mask3))
            {
                onCmaObj.layer = 10;
                continue;
            }
            if (Physics.Raycast(baser2, out onrcHit, Vector3.Distance(cmaPos, basePos2), mask3))
            {
                onCmaObj.layer = 10;
                continue;
            }
            if (Physics.Raycast(baser3, out onrcHit, Vector3.Distance(cmaPos, basePos3), mask3))
            {
                onCmaObj.layer = 10;
                continue;
            }
            if (Physics.Raycast(baser4, out onrcHit, Vector3.Distance(cmaPos, basePos4), mask3))
            {
                onCmaObj.layer = 10;
                continue;
            }
            if (Physics.Raycast(baser5, out onrcHit, Vector3.Distance(cmaPos, basePos5), mask3))
            {
                onCmaObj.layer = 10;
                continue;
            }
            if (Physics.Raycast(baser6, out onrcHit, Vector3.Distance(cmaPos, basePos6), mask3))
            {
                onCmaObj.layer = 10;
                continue;
            }

            onCmaObj.layer = 9;
            onRemoveCollision.Add(onCmaObj);
        }
        for (int i = 0; i < onRemoveCollision.Count; i++)
        {
            onCmaObj = onRemoveCollision[i];
            if (null == onCmaObj)
            {
                onCmaCollision.Remove(onCmaObj);
                continue;
            }
            lastColor = onCmaObj.GetComponent<MeshRenderer>().material.color;
            onCmaObj.GetComponent<MeshRenderer>().material.shader = Shader.Find("Legacy Shaders/Diffuse");
            onCmaObj.GetComponent<MeshRenderer>().material.color = new Color(lastColor.r, lastColor.g, lastColor.b, 1f);
            onCmaCollision.Remove(onCmaObj);
        }
        onRemoveCollision.Clear();

        //addin
        checkCmaCollision(baser0, cmaPos, basePos0);
        checkCmaCollision(baser1, cmaPos, basePos1);
        checkCmaCollision(baser2, cmaPos, basePos2);
        checkCmaCollision(baser3, cmaPos, basePos3);
        checkCmaCollision(baser4, cmaPos, basePos4);
        checkCmaCollision(baser5, cmaPos, basePos5);
        checkCmaCollision(baser6, cmaPos, basePos6);
    }
    void checkCmaCollision(Ray baser0, Vector3 cmaPos, Vector3 basePos0)//检测是否产生遮挡
    {
        for (; ; )
        {
            if (Physics.Raycast(baser0, out onrcHit, Vector3.Distance(cmaPos, basePos0), mask))
            {
                if (onrcHit.collider.gameObject.name == "Plane")
                {
                    break;
                }
                objAddToOnCmaCollision(onrcHit.collider.gameObject);
            }
            else
            {
                break;
            }
        }

    }
    void objAddToOnCmaCollision(GameObject obj)//添加遮挡物体
    {
        obj.layer = 10;
        Material objm = obj.GetComponent<MeshRenderer>().material;
        lastColor = objm.color;
        objm.shader = Shader.Find("Legacy Shaders/Transparent/Diffuse");
        objm.color = new Color(lastColor.r, lastColor.g, lastColor.b, 0.3f);
        onCmaCollision.Add(obj);
    }
    public void outPCOnDragBegin()
    {
        LastPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        nowAngle_y = mycamera.transform.eulerAngles.x;
        if (nowAngle_y > 180f || nowAngle_y < -180f)
        {
            for (; ; )
            {
                if (nowAngle_y > 180f) { nowAngle_y -= 360; }
                else if (nowAngle_y < -180) { nowAngle_y += 360; }

                if (nowAngle_y < 180f && nowAngle_y > -180f) { break; }
            }
        }
    }
    #endregion


    void Start()
    {
        messagesystem = GetComponent<MessageSystem>();
        individual = GetComponent<Individual>();
        controller = gameObject.GetComponent<CharacterController>();
        outPCOnDragBegin();//角色选择初始设置
        Cursor.visible = false;
        if (!mycamera)
        {
            mycamera = Camera.main.gameObject;
        }

        onCmaCollision = new List<GameObject>();
        onRemoveCollision = new List<GameObject>();
        mask = 1 << (LayerMask.NameToLayer("Collision"));
        mask2 = 1 << (LayerMask.NameToLayer("out"));
        mask3 = 1 << (LayerMask.NameToLayer("check"));

        animator = gameObject.GetComponentInChildren<Animator>();

    }

    Vector3 moveDirection = Vector3.zero;

    private static bool canitAction = true;
    private static bool canitAttack = true;
    private static bool canitMove = true;
    private static bool canitJump = true;

    void Inputs()
    {
        inputJump = Input.GetButtonDown("Jump");
        inputAttack1 = Input.GetButtonDown("Attack1");
        inputAttack2 = Input.GetButtonDown("Attack2");
        inputUnarmed = Input.GetButtonDown("Unarmed");
        inputShield = Input.GetButtonDown("Shield");
        inputHorizontal = Input.GetAxisRaw("Horizontal");
        inputVertical = Input.GetAxisRaw("Vertical");
        inputAiming = Input.GetButtonDown("Aiming");
        inputRoll = Input.GetButtonDown("Roll");
        inputDodge = Input.GetButtonDown("Dodge");
        inputMoving = (inputHorizontal != 0f) || (inputVertical != 0f);
    }

    void Update()
    {
        Inputs();
        //移动
        float x = inputHorizontal;
        float z = inputVertical;
        //bool inputMoving = (x != 0f || z != 0f);
        if (canitMove)
        {
            animator.SetFloat("Velocity X", x);
            animator.SetFloat("Velocity Z", z);
        }

        if (inputMoving && canitMove)
            animator.SetBool("Moving", true);
        else
            animator.SetBool("Moving", false);


        if(!controller.isGrounded)
            animator.SetTrigger("JumpTrigger");

        //翻滚&闪避
        if (controller.isGrounded && canitMove)//在地上 or空战模式开启
        {
            if (Input.GetKeyDown(KeyCode.LeftControl) && canitAction == true)//按下LeftControl
                StartCoroutine(IEaction(x, z, 1));

            if (Input.GetKeyDown(KeyCode.LeftShift) && canitAction == true)//按下shift
                StartCoroutine(IEaction(x, z, 2));
        }
        //跳跃
        if (controller.isGrounded)//在地上 
        {
            animator.SetInteger("Jumping", 0);//滞空状态的Jumping为0
            moveDirection = new Vector3(x * moveSpeed, 0, z * moveSpeed);
            moveDirection = transform.TransformDirection(moveDirection);//对齐到镜头坐标系
            if (inputJump && canitMove)//按下空格
                StartCoroutine(IEJump());
        }
        else//滞空状态
        {
            animator.SetInteger("Jumping", 2);//滞空状态的Jumping为非0
            if (controller.isGrounded)//落地时
                EventLand();
        }

        //近战攻击
        if (canitAction == true && canitAttack == true)
        {
            if (inputAttack1)
            {
                EventAttack_L1();
                StartCoroutine(IEattack(1, 1 / individual.attackSpeed));
            }
        }
    }

    private void LateUpdate()
    {
        //下落
        moveDirection.y -= fallingSpeed * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);

        cmaDirUpdateSet();//摄像机位置设置
        shelterCheckUpdate();//遮挡透明化处理
    }

    #region 事件
    //攻击
    void EventAttack_L1()
    {
        //Debug.Log("Attack_L1");
        //messagesystem.SendMessage(1, individual.attack, 50);
    }
    //落地
    void EventLand()
    {
        //Debug.Log("land");
        StartCoroutine(IEmove(landCD));
    }
    #endregion

    #region 协程
    IEnumerator IEmove(float CDtime)
    {
        canitMove = false;
        //Debug.Log("canitmove = false");
        yield return new WaitForSeconds(CDtime);
        canitMove = true;
        //Debug.Log("canitmove = true");
    }

    IEnumerator CD(float CDtime)
    {
        yield return new WaitForSeconds(CDtime);
    }

    IEnumerator IEaction(float x, float z, int actionID)//1Roll,2Dodge
    {
        if (actionID == 1)
        {
            if (x > 0f)
                animator.SetTrigger("RollRightTrigger");
            else if (x < 0f)
                animator.SetTrigger("RollLeftTrigger");
            if (z < 0f)
                animator.SetTrigger("RollBackwardTrigger");
            else
                animator.SetTrigger("RollForwardTrigger");
            canitAction = false;
            yield return new WaitForSeconds(rollCD);
            canitAction = true;
        }
        else if (actionID == 2)
        {
            if (x > 0f)
                animator.SetTrigger("DodgeRightTrigger");
            if (x < 0f)
                animator.SetTrigger("DodgeLeftTrigger");
            canitAction = false;
            yield return new WaitForSeconds(dodgeCD);
            canitAction = true;
        }
    }

    IEnumerator IEattack(int attackid, float CDtime)
    {
        if (attackid == 1)
            animator.SetTrigger("Attack1Trigger");
        canitAttack = false;
        yield return new WaitForSeconds(CDtime);
        canitAttack = true;
    }
    IEnumerator IEJump()
    {
        //animator.SetTrigger("JumpTrigger");
        moveDirection.y = jumpForce;
        canitMove = false;
        yield return new WaitForSeconds(landCD);
        canitMove = true;
    }
    #endregion
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 控制锁定目标器
/// </summary>
public class LockController : MonoBehaviour
{
    public bool lockState;
    public float lockLength = 10f;

    public GameObject player;
    public GameObject model;

    [HideInInspector]
    public GameObject lockTarget;
    [HideInInspector]
    public float lockTargetHalfHight;



    void Awake()
    {
        lockState = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (lockTarget != null)
        {
            if (!lockTarget.activeInHierarchy || Vector3.Distance(model.transform.position, lockTarget.transform.position) > lockLength)
            {
                lockTarget = null;
                lockState = false;
            }
        }
    }

    public void LockSwitch()
    {
        Vector3 modelOrigin1 = model.transform.position;
        Vector3 modelOrigin2 = modelOrigin1 + new Vector3(0, 1, 0);
        Vector3 boxCenter = modelOrigin2 + model.transform.forward * 5.0f;
        Collider[] cols = Physics.OverlapBox(boxCenter, new Vector3(0.5f, 0.5f, 5f), model.transform.rotation, LayerMask.GetMask("Individual"));

        bool ret = false;
        foreach (var col in cols)
        {
            //若已锁定目标是目标，则解除锁定
            if (lockTarget != null && lockTarget == col.gameObject)
            { 
                break;
            }
            //目标不是操控者
            if (col.gameObject != player)
            {
                lockTarget = col.gameObject;
                lockTargetHalfHight = col.bounds.extents.y;
                lockState = true;
                ret = true;
                break;
            }
        }

        if (!ret)
        {
            lockTarget = null;
            lockState = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainCameraController : MonoBehaviour
{
    public PlayerInput playerInput;
    public float horizontalSpeed = 100.0f;
    public float verticalSpeed = 80.0f;
    public float cameraDampValue = 0.05f;
    public Image lockDot;
    public bool lockState;
    public float lockLength = 10f;
    public bool isAI = false;

    private GameObject playerHandle;
    private GameObject cameraHandle;
    private GameObject model;
    private new GameObject camera;
    private float tempEulerX;
    private Vector3 cameraDampVelocity;
    [SerializeField]
    private LockTarget lockTarget;

    void Awake()
    {
        cameraHandle = transform.parent.gameObject;
        playerHandle = cameraHandle.transform.parent.gameObject;
        tempEulerX = 20;
        model = playerHandle.GetComponent<PlayerController>().model;
        camera = Camera.main.gameObject;
        lockDot.enabled = false;
        lockState = false;

        Cursor.lockState = CursorLockMode.Locked;
    }

    void FixedUpdate()
    {

        if (lockTarget == null)
        {
            Vector3 tempModelEuler = model.transform.eulerAngles;
            playerHandle.transform.Rotate(Vector3.up, playerInput.Jright * horizontalSpeed * Time.fixedDeltaTime);
            tempEulerX -= playerInput.Jup * verticalSpeed * Time.fixedDeltaTime;
            tempEulerX = Mathf.Clamp(tempEulerX, -40, 30);
            cameraHandle.transform.localEulerAngles = new Vector3(tempEulerX, 0, 0);
            model.transform.eulerAngles = tempModelEuler;
        }
        else
        {
            Vector3 temmpForward = lockTarget.obj.transform.position - model.transform.position;
            temmpForward.y = 0;
            playerHandle.transform.forward = temmpForward;
            cameraHandle.transform.LookAt(lockTarget.obj.transform);
        }

        camera.transform.position = Vector3.SmoothDamp(camera.transform.position, transform.position, ref cameraDampVelocity, cameraDampValue);
        camera.transform.LookAt(cameraHandle.transform);
    }

    void Update()
    {
        if (lockTarget != null)
        {
            if (lockTarget.obj.GetComponent<Individual>().DieMark || Vector3.Distance(model.transform.position, lockTarget.obj.transform.position) > lockLength)
            {
                lockTarget = null;
                lockDot.enabled = false;
                lockState = false;
            }
            else
            {
                lockDot.rectTransform.position = Camera.main.WorldToScreenPoint(lockTarget.obj.transform.position + new Vector3(0, lockTarget.halfHight, 0));
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
            if (lockTarget != null && lockTarget.obj == col.gameObject)
            {
                break;
            }
            //目标不是操控者
            if (col.gameObject != playerHandle)
            {
                lockTarget = new LockTarget(col.gameObject, col.bounds.extents.y);
                lockDot.enabled = true;
                lockState = true;
                ret = true;
                break;
            }
        }

        if (!ret)
        {
            lockTarget = null;
            lockDot.enabled = false;
            lockState = false;
        }
    }
    private class LockTarget
    {
        public GameObject obj;
        public float halfHight;

        public LockTarget(GameObject _obj, float _halfHigh)
        {
            obj = _obj;
            halfHight = _halfHigh;
        }
    }
}
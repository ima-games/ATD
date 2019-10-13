using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public PlayerInput playerInput;
    public GameObject cameraHandle;
    private Transform cameraPos;

    public LockController lockController;

    public GameObject player;
    public GameObject model;

    public float horizontalSpeed = 100.0f;
    public float verticalSpeed = 80.0f;
    public float cameraDampValue = 0.05f;


    private float tempEulerX;
    private Vector3 cameraDampVelocity;
    private new Camera camera;

    public GameObject TDView;
    public float LerpModulus;

    private enum State
    {
        TPS, TD, Neither
    }
    private State state;
    private bool isToTopDownView;

    private Vector3 TDPosition;
    private Quaternion TDRotation;

    private Vector3 targetPosition;
    private Quaternion targetRotation;

    //TODO
    public TowerSelect mouseSelect;

    private void Awake()
    {
        tempEulerX = 20;
        camera = Camera.main;
        cameraPos = cameraHandle.transform.GetChild(0);
    }

    void Start()
    {
        state = State.TPS;
        TDPosition = TDView.transform.position;
        TDRotation = TDView.transform.rotation;
        targetPosition = TDPosition;
        targetRotation = TDRotation;
        isToTopDownView = false;
    }

    void FixedUpdate()
    {
        GameObject lockTarget = lockController.lockTarget;
        if (lockTarget == null)
        {
            Vector3 tempModelEuler = model.transform.eulerAngles;
            player.transform.Rotate(Vector3.up, playerInput.Jright * horizontalSpeed * Time.fixedDeltaTime);
            tempEulerX -= playerInput.Jup * verticalSpeed * Time.fixedDeltaTime;
            tempEulerX = Mathf.Clamp(tempEulerX, -40, 30);
            cameraHandle.transform.localEulerAngles = new Vector3(tempEulerX, 0, 0);
            model.transform.eulerAngles = tempModelEuler;
        }
        else
        {
            Vector3 temmpForward = lockTarget.transform.position - model.transform.position;
            temmpForward.y = 0;
            player.transform.forward = temmpForward;
            cameraHandle.transform.LookAt(lockTarget.transform);
        }


        if (!isToTopDownView)
        {
            targetPosition = cameraPos.position;
            targetRotation = cameraPos.rotation;
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            state = State.Neither;
            isToTopDownView = !isToTopDownView;
            if (isToTopDownView)
            {
                targetPosition = TDPosition;
                targetRotation = TDRotation;
            }
        }

        if (state == State.Neither)
        {
            float distanSq = Vector3.SqrMagnitude(camera.transform.position - targetPosition);

            if (isToTopDownView && distanSq < 5.0f * 5.0f)
            {
                mouseSelect.enabled = true;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                mouseSelect.enabled = false;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }

            if (distanSq < .015f * .015f)
            {
                if (isToTopDownView)
                {
                    state = State.TD;
                }
                else
                {
                    state = State.TPS;
                }
                transform.position = targetPosition;
                transform.rotation = targetRotation;
            }

        }

        camera.transform.position = Vector3.SmoothDamp(camera.transform.position, targetPosition, ref cameraDampVelocity, cameraDampValue);
        camera.transform.rotation = Quaternion.Slerp(camera.transform.rotation, targetRotation, LerpModulus);
        //transform.position = Vector3.Slerp(transform.position, targetPosition, LerpModulus);
        //camera.transform.LookAt(cameraHandle.transform);
    }
}

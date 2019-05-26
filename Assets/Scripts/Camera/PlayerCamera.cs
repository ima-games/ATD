using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public PlayerInput playerInput;
    public GameObject cameraHandle;
    private Transform cameraPos;

    public GameObject playerHandle;

    public float horizontalSpeed = 100.0f;
    public float verticalSpeed = 80.0f;
    public float cameraDampValue = 0.05f;

    public LockController lockController;
    private float tempEulerX;
    private Vector3 cameraDampVelocity;
    private new Camera camera;

    private GameObject model;

    private void Awake()
    {
        tempEulerX = 20;
        camera = Camera.main;
        cameraPos = cameraHandle.transform.GetChild(0);
        model = playerHandle.GetComponent<PlayerController>().model;
    }

    void Start()
    {
        
    }

    void FixedUpdate()
    {
        GameObject lockTarget = lockController.lockTarget;
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
            Vector3 temmpForward = lockTarget.transform.position - model.transform.position;
            temmpForward.y = 0;
            playerHandle.transform.forward = temmpForward;
            cameraHandle.transform.LookAt(lockTarget.transform);
        }

        camera.transform.position = Vector3.SmoothDamp(camera.transform.position, cameraPos.transform.position, ref cameraDampVelocity, cameraDampValue);
        camera.transform.LookAt(cameraHandle.transform);
    }
}

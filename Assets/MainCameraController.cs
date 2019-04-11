using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraController : MonoBehaviour
{
    public PlayerInput playerInput;
    public float horizontalSpeed = 100.0f;
    public float verticalSpeed = 80.0f;
    public float cameraDampValue = 0.05f;

    private GameObject playerHandle;
    private GameObject cameraHandle;
    private float tempEulerX;
    private GameObject model;
    private GameObject camera;

    private Vector3 cameraDampVelocity;

    void Awake()
    {
        cameraHandle = transform.parent.gameObject;
        playerHandle = cameraHandle.transform.parent.gameObject;
        tempEulerX = 20;
        model = playerHandle.GetComponent<PlayerController>().model;
        camera = Camera.main.gameObject;
    }

    void FixedUpdate()
    {
        Vector3 tempModelEuler = model.transform.eulerAngles;

        playerHandle.transform.Rotate(Vector3.up, playerInput.Jright * horizontalSpeed * Time.fixedDeltaTime);
        //cameraHandle.transform.Rotate(Vector3.right, playerInput.Jup * -verticalSpeed * Time.deltaTime);
        //tempEulerX = cameraHandle.transform.eulerAngles.x;
        tempEulerX -= playerInput.Jup * verticalSpeed * Time.fixedDeltaTime;
        tempEulerX = Mathf.Clamp(tempEulerX, -40, 30);
        cameraHandle.transform.localEulerAngles = new Vector3(tempEulerX, 0, 0);

        model.transform.eulerAngles = tempModelEuler;

        camera.transform.position = Vector3.SmoothDamp(camera.transform.position, transform.position, ref cameraDampVelocity, cameraDampValue);
        camera.transform.eulerAngles = transform.eulerAngles;
    }
}

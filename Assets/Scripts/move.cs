using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{
    public float speed = 1;

    public float jumpSpeed = 10;

    public float gravity = 20;

    public float margin = 0.1f;

    private Vector3 moveDirection = Vector3.zero;

    // 通过射线检测主角是否落在地面或者物体上  
    bool IsGrounded() {
        //这里transform.position 一般在物体的中间位置，注意根据需要修改margin的值
        return Physics.Raycast(transform.position, -Vector3.up, margin);
    }

    // Update is called once per frame  
    void Update() {
        // 控制移动  
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if (IsGrounded()) {
            moveDirection = new Vector3(h, 0, v);
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;
            // 空格键控制跳跃  
            if (Input.GetButton("Jump")) {
                Debug.Log("i m jumping");
                moveDirection.y = jumpSpeed;
            }
        }
        moveDirection.y -= gravity * Time.deltaTime;
        GetComponent<CharacterController>().Move(moveDirection * Time.deltaTime);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{
    [Header("跳跃力度")]
    public float jumpforce = 200;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space)) //按下空格就跳跃
        {
            float v = Input.GetAxis("Vertical");
            float h = Input.GetAxis("Horizontal");
            this.GetComponent<Rigidbody>().AddForce(new Vector3(h, 1, v) * jumpforce);
        }
    }
}

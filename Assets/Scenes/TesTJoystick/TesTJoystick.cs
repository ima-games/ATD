using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TesTJoystick : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        print(Input.GetAxis("Horizontal")+"  "+ Input.GetAxis("Vertical"));
    }
}

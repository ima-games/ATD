using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA {
    public class InputHandle : MonoBehaviour {

        float vertical;
        float horizontal;

        // Start is called before the first frame update
        void Start () {

        }

        // Update is called once per frame
        void FixedUpdate () { 
            GetInput();
        }

        void GetInput () {
            vertical = Input.GetAxis ("Vertical");
            horizontal = Input.GetAxis ("Horizontal");
        }
    }

}
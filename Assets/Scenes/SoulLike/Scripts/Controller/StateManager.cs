using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA {
    public class StateManager : MonoBehaviour {
        public float vertical;
        public float horizontal;

        public GameObject activeModel;
        public Animator animator;
        public Rigidbody rigibody;

        public float delta;

        public void Init () {

            SetupAnimator ();
            rigibody = GetComponent<Rigidbody>();
        }

        void SetupAnimator () {

            if (activeModel == null) {
                animator = GetComponentInChildren<Animator> ();
                if (animator == null) {
                    Debug.Log ("No model found");
                } else {
                    activeModel = animator.gameObject;
                }
            }
            if (animator = null){
                animator = activeModel.GetComponent<Animator> ();
            }

            animator.applyRootMotion = false;
        }

        public void Tick(){

        }
    }
}
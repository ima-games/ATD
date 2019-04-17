using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class Helper : MonoBehaviour
    {
        [Range(0, 1)]
        public float vertical;

        public bool playAnim;
        public string[] oh_attacks;
        public string[] th_attacks;

        public bool twoHanded;
        public bool enableRM;
        public bool useItem;

        Animator anim;

        // Start is called before the first frame update
        void Start() {
            anim = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update() {


            enableRM = !anim.GetBool("canMove");
            anim.applyRootMotion = enableRM;

            if (enableRM) {
                return;
            }

            if (useItem) {
                playAnim = false;
                twoHanded = false;
                anim.Play("use_item");
            }

            anim.SetBool("twoHanded", twoHanded);

            if (playAnim) {

                string targetAnim;

                if (!twoHanded) {
                    int r = Random.Range(0, oh_attacks.Length);
                    targetAnim = oh_attacks[r];
                }
                else {
                    int r = Random.Range(0, th_attacks.Length);
                    targetAnim = th_attacks[r];
                }

                vertical = 0;

                anim.CrossFade(targetAnim, 0.2f);
                //anim.SetBool("canMove",false);
                //enableRM = true;
                playAnim = false;
            }
            anim.SetFloat("vertical", vertical);

            useItem = false;
        }
    }
}
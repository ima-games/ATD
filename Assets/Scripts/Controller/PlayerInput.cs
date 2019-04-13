using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    // Variable
    [Header("KeySettings")]
    public string keyUp = "w";
    public string keyDown = "s";
    public string keyLeft = "a";
    public string keyRight = "d";

    public string keyA;
    public string keyB;
    public string keyC;
    public string keyD;

    public string keyJRight = "right";
    public string keyJLeft = "left";
    public string keyJUp = "up";
    public string keyJDown = "down";

    [Header("OutputSignals")]
    public float Dup;
    public float Dright;
    public float Dmag;//Dup，Dright向量合成
    public Vector3 Dvec;

    public float Jup;
    public float Jright;


    public bool run;
    public bool jump;
    private bool lastJump;
    public bool attack;
    private bool lastAttack;


    [Header("Other")]
    public bool inputEnabled = true; //Flag

    private float targetDup;
    private float targetDright;
    private float velocityDup;
    private float velocityDright;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        Jup = (Input.GetKey(keyJUp) ? 1.0f : 0) - (Input.GetKey(keyJDown) ? 1.0f : 0);
        Jright = (Input.GetKey(keyJRight) ? 1.0f : 0) - (Input.GetKey(keyJLeft) ? 1.0f : 0);
        //print(Jright);


        targetDup = (Input.GetKey(keyUp) ? 1.0f : 0) - (Input.GetKey(keyDown) ? 1.0f : 0);
        targetDright = (Input.GetKey(keyRight) ? 1.0f : 0) - (Input.GetKey(keyLeft) ? 1.0f : 0);
        if (inputEnabled == false) {
            targetDup = 0;
            targetDright = 0;
        }

        Dup = Mathf.SmoothDamp(Dup, targetDup, ref velocityDup, 0.1f);//MARK SmoothDamp
        Dright = Mathf.SmoothDamp(Dright, targetDright, ref velocityDright, 0.1f);

        Vector2 tmpDAxis = SquareToCircle(new Vector2(Dright, Dup));
        float Dright2 = tmpDAxis.x;
        float Dup2 = tmpDAxis.y;
        Dmag = new Vector2(Dright2, Dup2).magnitude;
        Dvec = Dright * transform.right + Dup * transform.forward;

        run = Input.GetKey(keyA);

        bool newjump = Input.GetKey(keyB);
        if (newjump != lastJump && newjump == true) {
            jump = true;
        }

        else {
            jump = false;
        }

        lastJump = newjump;

        bool newAttack = Input.GetKey(keyC);
        if (newAttack != lastAttack && newAttack == true) {
            attack = true;
        }

        else {
            attack = false;
        }

        lastAttack = newAttack;
    }
    private Vector2 SquareToCircle(Vector2 input) {
        Vector2 output = Vector2.zero;
        output.x = input.x * Mathf.Sqrt(1 - (input.y * input.y) / 2.0f);
        output.y = input.y * Mathf.Sqrt(1 - (input.x * input.x) / 2.0f);
        return output;
    }
    //private Vector2 UseNormalized(Vector2 input) {
    //    Vector2 output = input.normalized;
    //    return output;
    //}
}

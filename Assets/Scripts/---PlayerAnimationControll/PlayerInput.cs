using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public Transform player;
    // Variable
    [Header("KeySettings")]
    public string keyUp = "w";
    public string keyDown = "s";
    public string keyLeft = "a";
    public string keyRight = "d";

    public string keyRun = "left shift";
    public string keyJump = "space";
    public string keyAttack = "mouse 0";
    public string keyDefense = "mouse 1";
    public string keyLock = "mouse 2";
    public string keyLHand = "q";
    public string keyRHand = "e";
    

    public MyButton buttonRun = new MyButton();
    public MyButton buttonJump = new MyButton();
    public MyButton buttonAttack = new MyButton();
    public MyButton buttonDefense = new MyButton();
    public MyButton buttonLock = new MyButton();
    public MyButton buttonLHand = new MyButton();
    public MyButton buttonRHand = new MyButton();

    public string keyJRight = "right";
    public string keyJLeft = "left";
    public string keyJUp = "up";
    public string keyJDown = "down";

    [Header("MouseSettings")]
    public bool mouseEnable = false;
    public float mousSensitivityX = 1.0f;
    public float mousSensitivityY = 1.0f;

    [Header("OutputSignals")]
    public float Dup;
    public float Dright;
    public float Dmag; //Dup，Dright向量合成
    public Vector3 Dvec;

    public float Jup;
    public float Jright;

    public bool run;
    public bool jump;
    //public bool attack;
    public bool defense;
    public bool roll;
    public bool lockon;
    public bool lHand;
    public bool rHand;

    [Header("Other")]
    public bool inputEnabled = true; //Flag

    private float targetDup;
    private float targetDright;
    private float velocityDup;
    private float velocityDright;

    void Start() {

    }

    void Update() {

        buttonRun.Tick(Input.GetKey(keyRun));//run
        buttonJump.Tick(Input.GetKey(keyJump));//jump
        buttonAttack.Tick(Input.GetKey(keyAttack));//attack
        buttonDefense.Tick(Input.GetKey(keyDefense));//denfese
        buttonLock.Tick(Input.GetKey(keyLock));//lock
        buttonLHand.Tick(Input.GetKey(keyLHand));//leftHand
        buttonRHand.Tick(Input.GetKey(keyRHand));//rightHand

        //延时
        //print(buttonRun.isExtending || buttonRun.isPressing); 
        //双击
        //print(buttonRun.isExtending && buttonRun.onPress);

        Jup = (Input.GetKey(keyJUp) ? 1.0f : 0) - (Input.GetKey(keyJDown) ? 1.0f : 0);
        Jright = (Input.GetKey(keyJRight) ? 1.0f : 0) - (Input.GetKey(keyJLeft) ? 1.0f : 0);

        if (mouseEnable == true) {
            Jup += Input.GetAxis("Mouse Y") * mousSensitivityX;
            Jright += Input.GetAxis("Mouse X") * mousSensitivityY;
        }

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
        Dvec = Dright2 * player.right + Dup2 * player.forward;

        //Button
        //attack = buttonAttack.onPressed;
        run = (buttonRun.isPressing && !buttonRun.isDelaying) || buttonRun.isExtending;
        jump = buttonJump.onPressed;
        roll = buttonRun.onReleased && buttonRun.isDelaying;
        lHand = buttonLHand.onPressed;
        rHand = buttonRHand.onPressed;
        defense = buttonDefense.isPressing;
        lockon = buttonLock.onPressed;
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

using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public enum Weapon{
	UNARMED = 0,
	TWOHANDSWORD = 1,
	TWOHANDSPEAR = 2,
	TWOHANDAXE = 3,
	TWOHANDBOW = 4,
	TWOHANDCROSSBOW = 5,
	STAFF = 6,
	ARMED = 7,
	RELAX = 8,
	RIFLE = 9,
	TWOHANDCLUB = 10,
	SHIELD = 11,
	ARMEDSHIELD = 12
}

public enum RPGCharacterState{
	DEFAULT,
	BLOCKING,
	STRAFING,
	CLIMBING,
	SWIMMING
}

public class RPGCharacterController : MonoBehaviour{
	#region Variables

	//Components
	[HideInInspector]
	public UnityEngine.AI.NavMeshAgent navMeshAgent;
	[HideInInspector]
	public Rigidbody rb;
	public Animator animator;
	public GameObject target;
	[HideInInspector]
	public Vector3 targetDashDirection;
	CapsuleCollider capCollider;
	ParticleSystem FXSplash;
	public Camera sceneCamera;
	public Vector3 waistRotationOffset;
	public RPGCharacterState rpgCharacterState = RPGCharacterState.DEFAULT;

	//jumping variables
	public float gravity = -9.8f;
	[HideInInspector]
	public float gravityTemp = 0f;
	[HideInInspector]
	public bool canJump;
	bool isJumping = false;
	[HideInInspector]
	public bool isGrounded;
	public float jumpSpeed = 12;
	public float doublejumpSpeed = 12;
	bool doJump = false;
	bool doublejumping = true;
	[HideInInspector]
	public bool canDoubleJump = false;
	[HideInInspector]
	public bool isDoubleJumping = false;
	bool doublejumped = false;
	bool isFalling;
	bool startFall;
	float fallingVelocity = -1f;
	float fallTimer = 0f;
	public float fallDelay = 0.2f;

	// Used for continuing momentum while in air
	public float inAirSpeed = 8f;
	float maxVelocity = 2f;
	float minVelocity = -2f;

	//rolling variables
	public float rollSpeed = 8;
	bool isRolling = false;
	public float rollduration;

	//movement variables
	[HideInInspector]
	public bool useMeshNav;
	[HideInInspector]
	public bool isMoving = false;
	[HideInInspector]
	public bool canMove = true;
	public float walkSpeed = 1.35f;
	float moveSpeed;
	public float runSpeed = 6f;
	float rotationSpeed = 40f;
	Vector3 inputVec;
	Vector3 newVelocity;

	//Weapon and Shield
	public Weapon weapon;
	[HideInInspector]
	public int rightWeapon = 0;
	[HideInInspector]
	public int leftWeapon = 0;
	[HideInInspector]
	public bool isRelax = false;
	bool isSwitchingFinished = true;

	//isStrafing/action variables
	public bool hipShooting = false;
	[HideInInspector]
	public bool canAction = true;
	bool isStrafing = false;
	[HideInInspector]
	public bool isDead = false;
	[HideInInspector]
	public bool isBlocking = false;
	public float knockbackMultiplier = 1f;
	bool isKnockback;
	[HideInInspector]
	public bool isSitting = false;
	bool isAiming = false;
	[HideInInspector]
	public bool
	isClimbing = false;
	[HideInInspector]
	public bool
	isNearLadder = false;
	[HideInInspector]
	public bool isNearCliff = false;
	[HideInInspector]
	public GameObject ladder;
	[HideInInspector]
	public GameObject cliff;
	[HideInInspector]
	public bool isCasting;
	public int special = 0;
	public float aimHorizontal;
	public float aimVertical;
	public float bowPull;
	bool injured;

	//Swimming variables
	public float inWaterSpeed = 8f;

	//Weapon Models
	public GameObject twoHandAxe;
	public GameObject twoHandSword;
	public GameObject twoHandSpear;
	public GameObject twoHandBow;
	public GameObject twoHandCrossbow;
	public GameObject twoHandClub;
	public GameObject staff;
	public GameObject swordL;
	public GameObject swordR;
	public GameObject maceL;
	public GameObject maceR;
	public GameObject daggerL;
	public GameObject daggerR;
	public GameObject itemL;
	public GameObject itemR;
	public GameObject shield;
	public GameObject pistolL;
	public GameObject pistolR;
	public GameObject rifle;
	public GameObject spear;
	public bool instantWeaponSwitch;

	//Inputs
	bool inputJump;
	bool inputLightHit;
	bool inputDeath;
	bool inputUnarmed;
	bool inputShield;
	bool inputAttackL;
	bool inputAttackR;
	bool inputCastL;
	bool inputCastR;
	float inputSwitchUpDown;
	float inputSwitchLeftRight;
	bool inputStrafe;
	float inputTargetBlock = 0;
	float inputDashVertical = 0;
	float inputDashHorizontal = 0;
	float inputHorizontal = 0;
	float inputVertical = 0;
	bool inputAiming;

	#endregion

	#region Initialization

	void Awake(){
		//set the components
		navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
		animator = GetComponentInChildren<Animator>();
		rb = GetComponent<Rigidbody>();
		capCollider = GetComponent<CapsuleCollider>();
		FXSplash = transform.GetChild(2).GetComponent<ParticleSystem>();
		HideAllWeapons();
	}

	#endregion

	#region UpdateAndInput

	void Inputs(){
		//Input abstraction for easier asset updates using outside control schemes
		inputJump = Input.GetButtonDown("Jump");
		inputLightHit = Input.GetButtonDown("LightHit");
		inputDeath = Input.GetButtonDown("Death");
		inputUnarmed = Input.GetButtonDown("Unarmed");
		inputShield = Input.GetButtonDown("Shield");
		inputAttackL = Input.GetButtonDown("AttackL");
		inputAttackR = Input.GetButtonDown("AttackR");
		inputCastL = Input.GetButtonDown("CastL");
		inputCastR = Input.GetButtonDown("CastR");
		inputSwitchUpDown = Input.GetAxisRaw("SwitchUpDown");
		inputSwitchLeftRight = Input.GetAxisRaw("SwitchLeftRight");
		inputStrafe = Input.GetKey(KeyCode.LeftShift);
		inputTargetBlock = Input.GetAxisRaw("TargetBlock");
		inputDashVertical = Input.GetAxisRaw("DashVertical");
		inputDashHorizontal = Input.GetAxisRaw("DashHorizontal");
		inputHorizontal = Input.GetAxisRaw("Horizontal");
		inputVertical = Input.GetAxisRaw("Vertical");
		inputAiming = Input.GetButtonDown("Aiming");
	}

	void Update(){
		Inputs();
		DirectionalAiming();
		if(canMove && !isBlocking && !isDead && !useMeshNav){
			CameraRelativeInput();
		}
		else{
			inputVec = new Vector3(0, 0, 0);
		}
		if(inputJump){
			doJump = true;
		}
		else{
			doJump = false;
		}
		if(rpgCharacterState != RPGCharacterState.SWIMMING){
			Rolling();
			Jumping();
			Blocking();
		}
		if(inputLightHit && canAction && isGrounded && !isBlocking){
			GetHit();
		}
		if(inputDeath && canAction && isGrounded && !isBlocking){
			if(!isDead){
				StartCoroutine(_Death());
			}
			else{
				StartCoroutine(_Revive());
			}
		}
		if(inputUnarmed && canAction && isGrounded && !isBlocking && weapon != Weapon.UNARMED){
			StartCoroutine(_SwitchWeapon(0));
		}
		if(inputShield && canAction && isGrounded && !isBlocking && leftWeapon != 7){
			StartCoroutine(_SwitchWeapon(7));
		}
		if(inputAttackL && canAction && isGrounded && !isBlocking){
			Attack(1);
		}
		if(inputAttackL && canAction && isGrounded && isBlocking){
			StartCoroutine(_BlockHitReact());
		}
		if(inputAttackR && canAction && isGrounded && !isBlocking){
			Attack(2);
		}
		if(inputAttackR && canAction && isGrounded && isBlocking){
			StartCoroutine(_BlockHitReact());
		}
		if(inputCastL && canAction && isGrounded && !isBlocking && !isStrafing){
			AttackKick(1);
		}
		if(inputCastL && canAction && isGrounded && isBlocking){
			StartCoroutine(_BlockBreak());
		}
		if(inputCastR && canAction && isGrounded && !isBlocking && !isStrafing){
			AttackKick(2);
		}
		if(inputCastR && canAction && isGrounded && isBlocking){
			StartCoroutine(_BlockBreak());
		}
		if(inputSwitchUpDown < -0.1f && canAction && !isBlocking && isGrounded && isSwitchingFinished){  
			SwitchWeaponTwoHand(0);
		}
		else if(inputSwitchUpDown > 0.1f && canAction && !isBlocking && isGrounded && isSwitchingFinished){
			SwitchWeaponTwoHand(1);
		}
		if(inputSwitchLeftRight < -0.1f && canAction && !isBlocking && isGrounded && isSwitchingFinished){  
			SwitchWeaponLeftRight(0);
		}
		else if(inputSwitchLeftRight > 0.1f && canAction && !isBlocking && isGrounded && isSwitchingFinished){  
			SwitchWeaponLeftRight(1);
		}
		if(inputSwitchLeftRight == 0 && inputSwitchUpDown == 0){
			isSwitchingFinished = true;
		}
		//Strafing
		if(inputStrafe || inputTargetBlock > 0.1f && canAction && weapon != Weapon.RIFLE){  
			if(!isRelax){
				isStrafing = true;
				animator.SetBool("Strafing", true);
			}
			else{
				Aiming();
			}
			if(inputCastL && canAction && isGrounded && !isBlocking){
				Cast(1, "attack");
			}
			if(inputCastR && canAction && isGrounded && !isBlocking){
				Cast(2, "attack");
			}
		}
		else{  
			isStrafing = false;
			animator.SetBool("Strafing", false);
		}
		//Aiming
		if(Input.GetMouseButtonDown(0)){
			if(useMeshNav){
				RaycastHit hit;
				if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100)){
					navMeshAgent.destination = hit.point;
				}
			}
			if(weapon == Weapon.RIFLE){
				if(hipShooting == true){
					animator.SetTrigger("Attack2Trigger");
				}
				else{
					animator.SetTrigger("Attack1Trigger");
				}
			}
			if((weapon == Weapon.TWOHANDBOW || weapon ==  Weapon.TWOHANDCROSSBOW) && isAiming){
				animator.SetTrigger("Attack1Trigger");
			}
		}
		if(Input.GetMouseButtonDown(2)){
			animator.SetTrigger("ReloadTrigger");
		}
		//Aiming switch
		if(inputAiming){
			if(!isAiming){
				isAiming = true;
				animator.SetBool("Aiming", true);
			}
			else{
				isAiming = false;
				animator.SetBool("Aiming", false);
			}
		}
		//Climbing
		if(rpgCharacterState == RPGCharacterState.CLIMBING && !isClimbing){
			if(inputVertical > 0.1f){
				animator.applyRootMotion = true;
				animator.SetTrigger("Climb-UpTrigger");
				isClimbing = true;
			}
			if(inputVertical < -0.1f){
				animator.applyRootMotion = true;
				animator.SetTrigger("Climb-DownTrigger");
				isClimbing = true;
			}
		}
		if(rpgCharacterState == RPGCharacterState.CLIMBING && isClimbing){
			if(inputVertical == 0){
				isClimbing = false;
			}
		}
		//Slow time
		if(Input.GetKeyDown(KeyCode.T)){
			if(Time.timeScale != 1){
				Time.timeScale = 1;
			}
			else{
				Time.timeScale = 0.15f;
			}
		}
		//Pause
		if(Input.GetKeyDown(KeyCode.P)){
			if(Time.timeScale != 1){
				Time.timeScale = 1;
			}
			else{
				Time.timeScale = 0f;
			}
		}
		//Injury
		if(Input.GetKeyDown(KeyCode.I)){
			if(injured == false){
				injured = true;
				animator.SetBool("Injured", true);
			}
			else{
				injured = false;
				animator.SetBool("Injured", false);
			}
		}
	}

	#endregion

	#region Fixed/Late Updates

	void FixedUpdate(){
		if(rpgCharacterState != RPGCharacterState.SWIMMING){
			CheckForGrounded();
			//apply gravity force
			rb.AddForce(0, gravity, 0, ForceMode.Acceleration);
			//check if character can move
			if(canMove && !isBlocking && rpgCharacterState != RPGCharacterState.CLIMBING){
				AirControl();
			}
			//check if falling
			if(rb.velocity.y < fallingVelocity && rpgCharacterState != RPGCharacterState.CLIMBING){
				isFalling = true;
				animator.SetInteger("Jumping", 2);
				canJump = false;
			}
			else{
				isFalling = false;
			}
		}
		else{
			WaterControl();
		}
		moveSpeed = UpdateMovement();
	}

	//get velocity of rigid body and pass the value to the animator to control the animations
	void LateUpdate(){
		//Get local velocity of charcter
		float velocityXel = transform.InverseTransformDirection(rb.velocity).x;
		float velocityZel = transform.InverseTransformDirection(rb.velocity).z;
		//Update animator with movement values
		animator.SetFloat("Velocity X", velocityXel / runSpeed);
		animator.SetFloat("Velocity Z", velocityZel / runSpeed);
		//if character is alive and can move, set our animator
		if(!isDead && canMove){
			if(moveSpeed > 0){
				animator.SetBool("Moving", true);
				isMoving = true;
			}
			else{
				animator.SetBool("Moving", false);
				isMoving = false;
			}
		}
		//If using Navmesh nagivation, update values
		if(useMeshNav){
			if(navMeshAgent.velocity.sqrMagnitude > 0){
				animator.SetBool("Moving", true);
				animator.SetFloat("Velocity Z", navMeshAgent.velocity.magnitude);
			}
		}
	}

	#endregion

	#region UpdateMovement

	//Moves the character
	float UpdateMovement(){
		Vector3 motion = inputVec;
		if(isGrounded && rpgCharacterState != RPGCharacterState.CLIMBING){
			//reduce input for diagonal movement
			if(motion.magnitude > 1){
				motion.Normalize();
			}
			if(canMove && !isBlocking && !useMeshNav){
				//set speed by walking / running
				if((isStrafing && !isAiming) || injured){
					newVelocity = motion * walkSpeed;
				}
				else{
					newVelocity = motion * runSpeed;
				}
				//if rolling use rolling speed and direction
				if(isRolling){
					//force the dash movement to 1
					targetDashDirection.Normalize();
					newVelocity = rollSpeed * targetDashDirection;
				}
			}
		}
		else{
			if(rpgCharacterState != RPGCharacterState.SWIMMING){
				//if we are falling use momentum
				newVelocity = rb.velocity;
			}
			else{
				newVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
			}
		}
		if(isStrafing && !isRelax){
			//make character point at target
			Quaternion targetRotation;
			Vector3 targetPos = target.transform.position;
			targetRotation = Quaternion.LookRotation(targetPos - new Vector3(transform.position.x, 0, transform.position.z));
			transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetRotation.eulerAngles.y, (rotationSpeed * Time.deltaTime) * rotationSpeed);
		}
		else if(isAiming){
			Aiming();
		}
		else{
			if(canMove){
				RotateTowardsMovementDir();
			}
		}
		//if we are falling use momentum
		newVelocity.y = rb.velocity.y;
		rb.velocity = newVelocity;
		//return a movement value for the animator
		return inputVec.magnitude;
	}

	//rotate character towards direction moved
	void RotateTowardsMovementDir(){
		if(inputVec != Vector3.zero && !isStrafing && !isAiming && !isRolling && !isBlocking && rpgCharacterState != RPGCharacterState.CLIMBING){
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(inputVec), Time.deltaTime * rotationSpeed);
		}
	}

	//All movement is based off camera facing
	void CameraRelativeInput(){
		//Camera relative movement
		Transform cameraTransform = sceneCamera.transform;
		//Forward vector relative to the camera along the x-z plane   
		Vector3 forward = cameraTransform.TransformDirection(Vector3.forward);
		forward.y = 0;
		forward = forward.normalized;
		//Right vector relative to the camera always orthogonal to the forward vector
		Vector3 right = new Vector3(forward.z, 0, -forward.x);
		//directional inputs
		if(!isRolling && !isAiming){
			targetDashDirection = inputDashHorizontal * right + inputDashVertical * -forward;
		}
		inputVec = inputHorizontal * right + inputVertical * forward;
	}

	#endregion

	#region Aiming / Turning

	void Aiming(){
		for(int i = 0; i < Input.GetJoystickNames().Length; i++){
			//if the right joystick is moved, use that for facing
			if(Mathf.Abs(inputDashHorizontal) > 0.1 || Mathf.Abs(inputDashVertical) < -0.1){
				Vector3 joyDirection = new Vector3(inputDashHorizontal, 0, -inputDashVertical);
				joyDirection = joyDirection.normalized;
				Quaternion joyRotation = Quaternion.LookRotation(joyDirection);
				transform.rotation = joyRotation;
			}
		}
		//no joysticks, use mouse aim
		if(Input.GetJoystickNames().Length == 0){
			Plane characterPlane = new Plane(Vector3.up, transform.position);
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			Vector3 mousePosition = new Vector3(0, 0, 0);
			float hitdist = 0.0f;
			if(characterPlane.Raycast(ray, out hitdist)){
				mousePosition = ray.GetPoint(hitdist);
			}
			mousePosition = new Vector3(mousePosition.x, transform.position.y, mousePosition.z);
			Vector3 relativePos = transform.position - mousePosition;
			Quaternion rotation = Quaternion.LookRotation(-relativePos);
			transform.rotation = rotation;

		}
	}

	//Direcitonal aiming used by 2Handed Bow
	void DirectionalAiming(){
		//controls
		if(Input.GetKey(KeyCode.LeftArrow)){
			aimHorizontal -= 0.05f;
		}
		if(Input.GetKey(KeyCode.RightArrow)){
			aimHorizontal += 0.05f;
		}
		if(Input.GetKey(KeyCode.DownArrow)){
			aimVertical -= 0.05f;
		}
		if(Input.GetKey(KeyCode.UpArrow)){
			aimVertical += 0.05f;
		}
		if(aimHorizontal >= 1){
			aimHorizontal = 1;
		}
		if(aimHorizontal <= -1){
			aimHorizontal = -1;
		}
		if(aimVertical >= 1){
			aimVertical = 1;
		}
		if(aimVertical <= -1){
			aimVertical = -1;
		}
		if(Input.GetKey(KeyCode.B)){
			bowPull -= 0.05f;
		}
		if(Input.GetKey(KeyCode.N)){
			bowPull += 0.05f;
		}
		if(bowPull >= 1){
			bowPull = 1;
		}
		if(bowPull <= -1){
			bowPull = -1;
		}
		//Set the animator
		animator.SetFloat("AimHorizontal", aimHorizontal);
		animator.SetFloat("AimVertical", aimVertical);
		animator.SetFloat("BowPull", bowPull);
	}

	//Turning
	public IEnumerator _Turning(int direction){
		if(direction == 1){
			StartCoroutine(_Lock(true, true, true, 0, 0.55f));
			animator.SetTrigger("TurnLeftTrigger");
		}
		if(direction == 2){
			StartCoroutine(_Lock(true, true, true, 0, 0.55f));
			animator.SetTrigger("TurnRightTrigger");
		}
		yield return null;
	}

	//Dodging
	public IEnumerator _Dodge(int direction){
		if(direction == 1){
			StartCoroutine(_Lock(true, true, true, 0, 0.55f));
			animator.SetTrigger("DodgeLeftTrigger");
		}
		if(direction == 2){
			StartCoroutine(_Lock(true, true, true, 0, 0.55f));
			animator.SetTrigger("DodgeRightTrigger");
		}
		yield return null;
	}

	#endregion

	#region Swimming

	void OnTriggerEnter(Collider collide){
		//If entering a water volume
		if(collide.gameObject.layer == 4){
			rpgCharacterState = RPGCharacterState.SWIMMING;
			canAction = false;
			rb.useGravity = false;
			animator.SetTrigger("SwimTrigger");
			animator.SetBool("Swimming", true);
			animator.SetInteger("Weapon", 0);
			weapon = Weapon.UNARMED;
			StartCoroutine(_WeaponVisibility(leftWeapon, 0, false));
			StartCoroutine(_WeaponVisibility(rightWeapon, 0, false));
			animator.SetInteger("RightWeapon", 0);
			animator.SetInteger("LeftWeapon", 0);
			animator.SetInteger("LeftRight", 0);
			FXSplash.Emit(30);
		}
		else if(collide.transform.parent != null){
			if(collide.transform.parent.name.Contains("Ladder")){
				isNearLadder = true;
				ladder = collide.gameObject;
			}
		}
		else if(collide.transform.name.Contains("Cliff")){
			isNearCliff = true;
			cliff = collide.gameObject;
		}
	}

	void OnTriggerExit(Collider collide){
		//If leaving a water volume
		if(collide.gameObject.layer == 4){
			rpgCharacterState = RPGCharacterState.DEFAULT;
			canAction = true;
			rb.useGravity = true;
			animator.SetInteger("Jumping", 2);
			animator.SetBool("Swimming", false);
			capCollider.radius = 0.5f;
		}
		//If leaving a ladder
		else if(collide.transform.parent != null){
			if(collide.transform.parent.name.Contains("Ladder")){
				isNearLadder = false;
				ladder = null;
			}
		}
	}

	//Movement when in water volume
	void WaterControl(){
		AscendDescend();
		Vector3 motion = inputVec;
		//dampen vertical water movement
		Vector3 dampenVertical = new Vector3(rb.velocity.x, (rb.velocity.y * 0.985f), rb.velocity.z);
		rb.velocity = dampenVertical;
		Vector3 waterDampen = new Vector3((rb.velocity.x * 0.98f), rb.velocity.y, (rb.velocity.z * 0.98f));
		//If swimming, don't dampen movement, and scale capsule collider
		if(moveSpeed < 0.1f){
			rb.velocity = waterDampen;
			capCollider.radius = 0.5f;
		}
		else{
			capCollider.radius = 1.5f;
		}
		rb.velocity = waterDampen;
		//clamp diagonal movement so its not faster
		motion *= (Mathf.Abs(inputVec.x) == 1 && Mathf.Abs(inputVec.z) == 1) ? 0.7f : 1;
		rb.AddForce(motion * inWaterSpeed, ForceMode.Acceleration);
		//limit the amount of velocity we can achieve to water speed
		float velocityX = 0;
		float velocityZ = 0;
		if(rb.velocity.x > inWaterSpeed){
			velocityX = GetComponent<Rigidbody>().velocity.x - inWaterSpeed;
			if(velocityX < 0){
				velocityX = 0;
			}
			rb.AddForce(new Vector3(-velocityX, 0, 0), ForceMode.Acceleration);
		}
		if(rb.velocity.x < minVelocity){
			velocityX = rb.velocity.x - minVelocity;
			if(velocityX > 0){
				velocityX = 0;
			}
			rb.AddForce(new Vector3(-velocityX, 0, 0), ForceMode.Acceleration);
		}
		if(rb.velocity.z > inWaterSpeed){
			velocityZ = rb.velocity.z - maxVelocity;
			if(velocityZ < 0){
				velocityZ = 0;
			}
			rb.AddForce(new Vector3(0, 0, -velocityZ), ForceMode.Acceleration);
		}
		if(rb.velocity.z < minVelocity){
			velocityZ = rb.velocity.z - minVelocity;
			if(velocityZ > 0){
				velocityZ = 0;
			}
			rb.AddForce(new Vector3(0, 0, -velocityZ), ForceMode.Acceleration);
		}
	}

	//Swim up and down
	void AscendDescend(){
		if(doJump){
			//swim down with left control
			if(isStrafing){
				animator.SetBool("Strafing", true);
				animator.SetTrigger("JumpTrigger");
				rb.velocity -= inWaterSpeed * Vector3.up;
			}
			else{
				animator.SetTrigger("JumpTrigger");
				rb.velocity += inWaterSpeed * Vector3.up;
			}
		}
	}

	#endregion

	#region Jumping

	//checks if character is within a certain distance from the ground, and markes it IsGrounded
	void CheckForGrounded(){
		float distanceToGround;
		float threshold = .45f;
		RaycastHit hit;
		Vector3 offset = new Vector3(0, 0.4f, 0);
		if(Physics.Raycast((transform.position + offset), -Vector3.up, out hit, 100f)){
			distanceToGround = hit.distance;
			if(distanceToGround < threshold){
				isGrounded = true;
				canJump = true;
				startFall = false;
				doublejumped = false;
				canDoubleJump = false;
				isFalling = false;
				fallTimer = 0;
				if(!isJumping){
					animator.SetInteger("Jumping", 0);
				}
				//exit climbing on ground
				if(rpgCharacterState == RPGCharacterState.CLIMBING){
					animator.SetTrigger("Climb-Off-BottomTrigger");
					gravity = gravityTemp;
					rb.useGravity = true;
					rpgCharacterState = RPGCharacterState.DEFAULT;
				}
			}
			else{
				fallTimer += 0.009f;
				if(fallTimer >= fallDelay){
					isGrounded = false;
				}
			}
		}
	}

	void Jumping(){
		if(isGrounded){
			if(canJump && doJump){
				StartCoroutine(_Jump());
			}
		}
		else{    
			canDoubleJump = true;
			canJump = false;
			if(isFalling){
				//set the animation back to falling
				animator.SetInteger("Jumping", 2);
				//prevent from going into land animation while in air
				if(!startFall){
					animator.SetTrigger("JumpTrigger");
					startFall = true;
				}
			}
			if(canDoubleJump && doublejumping && Input.GetButtonDown("Jump") && !doublejumped && isFalling){
				// Apply the current movement to launch velocity
				rb.velocity += doublejumpSpeed * Vector3.up;
				animator.SetInteger("Jumping", 3);
				doublejumped = true;
			}
		}
	}

	public IEnumerator _Jump(){
		isJumping = true;
		animator.SetInteger("Jumping", 1);
		animator.SetTrigger("JumpTrigger");
		// Apply the current movement to launch velocity
		rb.velocity += jumpSpeed * Vector3.up;
		canJump = false;
		yield return new WaitForSeconds(0.5f);
		isJumping = false;
	}

	void AirControl(){
		if(!isGrounded){
			Vector3 motion = inputVec;
			motion *= (Mathf.Abs(inputVec.x) == 1 && Mathf.Abs(inputVec.z) == 1) ? 0.7f : 1;
			rb.AddForce(motion * inAirSpeed, ForceMode.Acceleration);
			//limit the amount of velocity we can achieve
			float velocityX = 0;
			float velocityZ = 0;
			if(rb.velocity.x > maxVelocity){
				velocityX = GetComponent<Rigidbody>().velocity.x - maxVelocity;
				if(velocityX < 0){
					velocityX = 0;
				}
				rb.AddForce(new Vector3(-velocityX, 0, 0), ForceMode.Acceleration);
			}
			if(rb.velocity.x < minVelocity){
				velocityX = rb.velocity.x - minVelocity;
				if(velocityX > 0){
					velocityX = 0;
				}
				rb.AddForce(new Vector3(-velocityX, 0, 0), ForceMode.Acceleration);
			}
			if(rb.velocity.z > maxVelocity){
				velocityZ = rb.velocity.z - maxVelocity;
				if(velocityZ < 0){
					velocityZ = 0;
				}
				rb.AddForce(new Vector3(0, 0, -velocityZ), ForceMode.Acceleration);
			}
			if(rb.velocity.z < minVelocity){
				velocityZ = rb.velocity.z - minVelocity;
				if(velocityZ > 0){
					velocityZ = 0;
				}
				rb.AddForce(new Vector3(0, 0, -velocityZ), ForceMode.Acceleration);
			}
		}
	}

	#endregion

	#region MiscMethods

	public void Climbing(){
		rpgCharacterState = RPGCharacterState.CLIMBING;
	}

	public void EndClimbing(){
		rpgCharacterState = RPGCharacterState.DEFAULT;
		gravity = gravityTemp;
		rb.useGravity = true;
		animator.applyRootMotion = false;
		canMove = true;
		isClimbing = false;
	}

	//0 = No side
	//1 = Left
	//2 = Right
	//3 = Dual
	//weaponNumber 0 = Unarmed
	//weaponNumber 1 = 2H Sword
	//weaponNumber 2 = 2H Spear
	//weaponNumber 3 = 2H Axe
	//weaponNumber 4 = 2H Bow
	//weaponNumber 5 = 2H Crowwbow
	//weaponNumber 6 = 2H Staff
	//weaponNumber 7 = Shield
	//weaponNumber 8 = L Sword
	//weaponNumber 9 = R Sword
	//weaponNumber 10 = L Mace
	//weaponNumber 11 = R Mace
	//weaponNumber 12 = L Dagger
	//weaponNumber 13 = R Dagger
	//weaponNumber 14 = L Item
	//weaponNumber 15 = R Item
	//weaponNumber 16 = L Pistol
	//weaponNumber 17 = R Pistol
	//weaponNumber 18 = Rifle
	//weaponNumber 19 == Right Spear
	//weaponNumber 20 == 2H Club
	public void Attack(int attackSide){
		if(canAction && isGrounded){
			//No controller input
			if(inputVec.magnitude == 0f){
				//Armed or Unarmed
				if(weapon == Weapon.UNARMED || weapon == Weapon.ARMED || weapon == Weapon.ARMEDSHIELD){
					int maxAttacks = 3;
					int attackNumber = 0;
					//left attacks
					if(attackSide == 1){
						animator.SetInteger("AttackSide", 1);
						//Left sword has 6 attacks
						if(leftWeapon == 8){
							attackNumber = Random.Range(1, 6);
						}
						else{
							attackNumber = Random.Range(1, maxAttacks);
						}
					}
					//right attacks
					else if(attackSide == 2){
						animator.SetInteger("AttackSide", 2);
						//Right spear has 7 attacks
						if(rightWeapon == 19){
							attackNumber = Random.Range(1, 7);
						}
						//Right sword has 6 attacks
						else if(rightWeapon == 9){
							attackNumber = Random.Range(7, 12);
						}
						else{
							attackNumber = Random.Range(3, maxAttacks + 3);
						}
					}
					//dual attacks
					else if(attackSide == 3){
						attackNumber = Random.Range(1, maxAttacks);
					}
					if(attackSide != 3){
						animator.SetTrigger("Attack" + (attackNumber + 1).ToString() + "Trigger");
						if(leftWeapon == 12 || leftWeapon == 14 || rightWeapon == 13 || rightWeapon == 15 || rightWeapon == 19){
							StartCoroutine(_Lock(true, true, true, 0, 0.75f));
						}
						else{
							StartCoroutine(_Lock(true, true, true, 0, 0.7f));
						}
					}
					//Dual Attacks
					else{
						animator.SetTrigger("AttackDual" + (attackNumber + 1).ToString() + "Trigger");
						StartCoroutine(_Lock(true, true, true, 0, 0.75f));
					}
				}
				else if(weapon == Weapon.SHIELD){
					int maxAttacks = 1;
					int attackNumber = Random.Range(1, maxAttacks);
					if(isGrounded){
						animator.SetTrigger("Attack" + (attackNumber).ToString() + "Trigger");
						StartCoroutine(_Lock(true, true, true, 0, 1.1f));
					}
				}
				else if(weapon == Weapon.TWOHANDSPEAR){
					int maxAttacks = 10;
					int attackNumber = Random.Range(1, maxAttacks);
					if(isGrounded){
						animator.SetTrigger("Attack" + (attackNumber).ToString() + "Trigger");
						StartCoroutine(_Lock(true, true, true, 0, 1.1f));
					}
				}
				else if(weapon == Weapon.TWOHANDCLUB){
					int maxAttacks = 10;
					int attackNumber = Random.Range(1, maxAttacks);
					if(isGrounded){
						animator.SetTrigger("Attack" + (attackNumber).ToString() + "Trigger");
						StartCoroutine(_Lock(true, true, true, 0, 1.1f));
					}
				}
				else if(weapon == Weapon.TWOHANDSWORD){
					int maxAttacks = 11;
					int attackNumber = Random.Range(1, maxAttacks);
					if(isGrounded){
						animator.SetTrigger("Attack" + (attackNumber).ToString() + "Trigger");
						StartCoroutine(_Lock(true, true, true, 0, 1.1f));
					}
				}
				else{
					int maxAttacks = 6;
					int attackNumber = Random.Range(1, maxAttacks);
					if(isGrounded){
						animator.SetTrigger("Attack" + (attackNumber).ToString() + "Trigger");
						if(weapon == Weapon.TWOHANDSWORD){
							StartCoroutine(_Lock(true, true, true, 0, 0.85f));
						}
						else if(weapon == Weapon.TWOHANDAXE){
							StartCoroutine(_Lock(true, true, true, 0, 1.5f));
						}
						else if(weapon == Weapon.STAFF){
							StartCoroutine(_Lock(true, true, true, 0, 1f));
						}
						else{
							StartCoroutine(_Lock(true, true, true, 0, 0.75f));
						}
					}
				}
			}
			//Character is Moving, use running attacks.
			else if(weapon == Weapon.ARMED){
				if(attackSide == 1){
					animator.SetTrigger("Attack1Trigger");
				}
				if(attackSide == 2){
					animator.SetTrigger("Attack4Trigger");
				}
				if(attackSide == 3){
					animator.SetTrigger("AttackDual1Trigger");
				}
			}
			else{
			}
		}
	}

	public void AttackKick(int kickSide){
		if(isGrounded){
			if(kickSide == 1){
				animator.SetTrigger("AttackKick1Trigger");
			}
			else{
				animator.SetTrigger("AttackKick2Trigger");
			}
			StartCoroutine(_Lock(true, true, true, 0, 0.8f));
		}
	}

	public void Special(int n){
		if(special < 1){
			animator.SetTrigger("Special" + n.ToString() + "Trigger");
			special = n;
			canAction = false;
			if(weapon == Weapon.TWOHANDSWORD){
				if(special == 1){
					StartCoroutine(_Lock(false, true, false, 0, 0.6f));
				}
			}
		}
		else{
			animator.SetTrigger("SpecialEndTrigger");
			UnLock(true, true);
			special = 0;
		}
	}

	//0 = No side
	//1 = Left
	//2 = Right
	//3 = Dual
	public void Cast(int attackSide, string type){
		//Cancel current casting
		if(attackSide == 0){
			animator.SetTrigger("CastEndTrigger");
			isCasting = false;
			canAction = true;
			StartCoroutine(_Lock(true, true, true, 0, 0.2f));
			return;
		}
		//Set Left, Right, Dual for variable casts
		if(attackSide == 4){
			if(leftWeapon == 0 && rightWeapon == 0){
				animator.SetInteger("LeftRight", 3);
			}
			else if(leftWeapon == 0){
				animator.SetInteger("LeftRight", 1);
			}
			else if(rightWeapon == 0){
				animator.SetInteger("LeftRight", 2);
			}
		}
		else{
			animator.SetInteger("LeftRight", attackSide);
		}
		if(weapon == Weapon.UNARMED || weapon == Weapon.STAFF || weapon == Weapon.ARMED){
			//Cast Attacks
			if(type == "attack"){
				int maxAttacks = 3;
				if(isGrounded){
					int attackNumber = Random.Range(1, maxAttacks + 1);
					animator.SetTrigger("CastAttack" + (attackNumber).ToString() + "Trigger");
					isCasting = true;
					canAction = false;
					StartCoroutine(_Lock(true, true, false, 0, 0.8f));
				}
			}
			//Cast Buffs, AOE, Summons
			else{
				int maxAttacks = 2;
				int attackNumber = Random.Range(1, maxAttacks + 1);
				if(isGrounded){
					isCasting = true;
					canAction = false;
					if(type == "AOE"){
						animator.SetTrigger("CastAOE" + (attackNumber).ToString() + "Trigger");
					}
					if(type == "buff"){
						animator.SetTrigger("CastBuff" + (attackNumber).ToString() + "Trigger");
					}
					if(type == "summon"){
						animator.SetTrigger("CastSummon" + (attackNumber).ToString() + "Trigger");
					}
					StartCoroutine(_Lock(true, true, false, 0, 0.8f));
				}
			}
		}
	}

	public void Blocking(){
		if(Input.GetAxisRaw("TargetBlock") < -0.1f && canAction && isGrounded){
			if(!isBlocking){
				animator.SetTrigger("BlockTrigger");
			}
			isBlocking = true;
			canJump = false;
			animator.SetBool("Blocking", true);
			rb.velocity = Vector3.zero;
			rb.angularVelocity = Vector3.zero;
			inputVec = Vector3.zero;
		}
		else{
			isBlocking = false;
			canJump = true;
			animator.SetBool("Blocking", false);
		}
	}

	public void GetHit(){
		if(weapon != Weapon.RIFLE){
			int hits = 5;
			int hitNumber = Random.Range(0, hits);
			animator.SetTrigger("GetHit" + (hitNumber + 1).ToString() + "Trigger");
			StartCoroutine(_Lock(true, true, true, 0.1f, 0.4f));
			//apply directional knockback force
			if(hitNumber <= 1){
				StartCoroutine(_Knockback(-transform.forward, 8, 4));
			}
			else if(hitNumber == 2){
				StartCoroutine(_Knockback(transform.forward, 8, 4));
			}
			else if(hitNumber == 3){
				StartCoroutine(_Knockback(transform.right, 8, 4));
			}
			else if(hitNumber == 4){
				StartCoroutine(_Knockback(-transform.right, 8, 4));
			}
		}
		else{
			animator.SetTrigger("GetHit1Trigger");
		}
	}

	IEnumerator _Knockback(Vector3 knockDirection, int knockBackAmount, int variableAmount){
		isKnockback = true;
		StartCoroutine(_KnockbackForce(knockDirection, knockBackAmount, variableAmount));
		yield return new WaitForSeconds(.1f);
		isKnockback = false;
	}

	IEnumerator _KnockbackForce(Vector3 knockDirection, int knockBackAmount, int variableAmount){
		while(isKnockback){
			rb.AddForce(knockDirection * ((knockBackAmount + Random.Range(-variableAmount, variableAmount)) * (knockbackMultiplier * 10)), ForceMode.Impulse);
			yield return null;
		}
	}

	public IEnumerator _Death(){
		animator.SetTrigger("Death1Trigger");
		StartCoroutine(_Lock(true, true, true, 0.1f, 1.5f));
		isDead = true;
		animator.SetBool("Moving", false);
		inputVec = new Vector3(0, 0, 0);
		yield return null;
	}

	public IEnumerator _Revive(){
		animator.SetTrigger("Revive1Trigger");
		StartCoroutine(_Lock(true, true, true, 0f, 1.45f));
		isDead = false;
		yield return null;
	}

	#endregion

	#region Rolling

	void Rolling(){
		if(!isRolling && isGrounded && !isAiming){
			if(Input.GetAxis("DashVertical") > 0.5f || Input.GetAxis("DashVertical") < -0.5f || Input.GetAxis("DashHorizontal") > 0.5f || Input.GetAxis("DashHorizontal") < -0.5f){
				StartCoroutine(_DirectionalRoll());
			}
		}
	}

	public IEnumerator _DirectionalRoll(){
		//check which way the dash is pressed relative to the character facing
		float angle = Vector3.Angle(targetDashDirection, -transform.forward);
		float sign = Mathf.Sign(Vector3.Dot(transform.up, Vector3.Cross(targetDashDirection, transform.forward)));
		// angle in [-179,180]
		float signed_angle = angle * sign;
		//angle in 0-360
		float angle360 = (signed_angle + 180) % 360;
		//deternime the animation to play based on the angle
		if(angle360 > 315 || angle360 < 45){
			StartCoroutine(_Roll(1));
		}
		if(angle360 > 45 && angle360 < 135){
			StartCoroutine(_Roll(2));
		}
		if(angle360 > 135 && angle360 < 225){
			StartCoroutine(_Roll(3));
		}
		if(angle360 > 225 && angle360 < 315){
			StartCoroutine(_Roll(4));
		}
		yield return null;
	}

	public IEnumerator _Roll(int rollNumber){
		if(rollNumber == 1){
			animator.SetTrigger("RollForwardTrigger");
		}
		if(rollNumber == 2){
			animator.SetTrigger("RollRightTrigger");
		}
		if(rollNumber == 3){
			animator.SetTrigger("RollBackwardTrigger");
		}
		if(rollNumber == 4){
			animator.SetTrigger("RollLeftTrigger");
		}
		isRolling = true;
		canAction = false;
		yield return new WaitForSeconds(rollduration);
		isRolling = false;
		canAction = true;
	}

	//Placeholder functions for Animation events
	public void Hit(){
	}

	public void Shoot(){
	}

	public void FootR(){
	}

	public void FootL(){
	}

	public void Land(){
	}

	public void WeaponSwitch(){
	}

	#endregion

	#region _Coroutines

	//method to keep character from moving while attacking, etc
	//timed -1 = infinite, 0 = no, 1 = yes
	public IEnumerator _Lock(bool lockMovement, bool lockAction, bool timed, float delayTime, float lockTime){
		yield return new WaitForSeconds(delayTime);
		if(lockMovement){
			LockMovement();
		}
		if(lockAction){
			LockAction();
		}
		if(timed){
			yield return new WaitForSeconds(lockTime);
			UnLock(lockMovement, lockAction);
		}
	}

	//method to keep character from moving while attacking, etc
	void LockMovement(){
		canMove = false;
		animator.SetBool("Moving", false);
		rb.velocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;
		inputVec = new Vector3(0, 0, 0);
		animator.applyRootMotion = true;
	}

	//method to keep character from moving while attacking, etc
	void LockAction(){
		canAction = false;
	}
		
	//let character move and act again
	void UnLock(bool movement, bool actions){
		if(movement){
			canMove = true;
			animator.applyRootMotion = false;
		}
		if(actions){
			canAction = true;
		}
	}

	//for controller weapon switching
	void SwitchWeaponTwoHand(int upDown){
		if(instantWeaponSwitch){
			HideAllWeapons();
		}
		isSwitchingFinished = false;
		int weaponSwitch = (int)weapon;
		if(upDown == 0){
			weaponSwitch--;
			if(weaponSwitch < 1 || weaponSwitch == 18 || weaponSwitch == 20){
				StartCoroutine(_SwitchWeapon(6));
			}
			else{
				StartCoroutine(_SwitchWeapon(weaponSwitch));
			}
		}
		if(upDown == 1){
			weaponSwitch++;
			if(weaponSwitch > 6 && weaponSwitch < 18){
				StartCoroutine(_SwitchWeapon(1));
			}
			else{
				StartCoroutine(_SwitchWeapon(weaponSwitch));
			}
		}
	}

	//for controller weapon switching
	void SwitchWeaponLeftRight(int leftRight){
		if(instantWeaponSwitch){
			HideAllWeapons();
		}
		int weaponSwitch = 0;
		isSwitchingFinished = false;
		if(leftRight == 0){
			weaponSwitch = leftWeapon;
			if(weaponSwitch < 16 && weaponSwitch != 0 && leftWeapon != 7){
				weaponSwitch += 2;
			}
			else{
				weaponSwitch = 8;
			}
		}
		if(leftRight == 1){
			weaponSwitch = rightWeapon;
			if(weaponSwitch < 17 && weaponSwitch != 0){
				weaponSwitch += 2;
			}
			else{
				weaponSwitch = 9;
			}
		}
		StartCoroutine(_SwitchWeapon(weaponSwitch));
	}

	//function to switch weapons
	//weaponNumber 0 = Unarmed
	//weaponNumber 1 = 2H Sword
	//weaponNumber 2 = 2H Spear
	//weaponNumber 3 = 2H Axe
	//weaponNumber 4 = 2H Bow
	//weaponNumber 5 = 2H Crowwbow
	//weaponNumber 6 = 2H Staff
	//weaponNumber 7 = Shield
	//weaponNumber 8 = L Sword
	//weaponNumber 9 = R Sword
	//weaponNumber 10 = L Mace
	//weaponNumber 11 = R Mace
	//weaponNumber 12 = L Dagger
	//weaponNumber 13 = R Dagger
	//weaponNumber 14 = L Item
	//weaponNumber 15 = R Item
	//weaponNumber 16 = L Pistol
	//weaponNumber 17 = R Pistol
	//weaponNumber 18 = Rifle
	//weaponNumber 19 == Right Spear
	//weaponNumber 20 == 2H Club
	public IEnumerator _SwitchWeapon(int weaponNumber){	
		//character is unarmed
		if(weapon == Weapon.UNARMED){
			StartCoroutine(_UnSheathWeapon(weaponNumber));
		}
		//character has 2 handed weapon
		else if(weapon == Weapon.STAFF || weapon == Weapon.TWOHANDAXE || weapon == Weapon.TWOHANDBOW || weapon == Weapon.TWOHANDCROSSBOW || weapon == Weapon.TWOHANDSPEAR || weapon == Weapon.TWOHANDSWORD || weapon == Weapon.RIFLE || weapon == Weapon.TWOHANDCLUB){
			StartCoroutine(_SheathWeapon(leftWeapon, weaponNumber));
			if(!instantWeaponSwitch){
				yield return new WaitForSeconds(1.1f);
			}
			if(weaponNumber > 0){
				StartCoroutine(_UnSheathWeapon(weaponNumber));
			}
			//switch to unarmed
			else{
				weapon = Weapon.UNARMED;
				animator.SetInteger("Weapon", 0);
			}
		}
		//character has 1 or 2 1hand weapons and/or shield
		else if(weapon == Weapon.ARMED || weapon == Weapon.SHIELD || weapon == Weapon.ARMEDSHIELD){
			//character is switching to 2 hand weapon or unarmed, put put away all weapons
			if(weaponNumber < 7 || weaponNumber > 17 || weaponNumber == 20){
				//check left hand for weapon
				if(leftWeapon != 0){
					StartCoroutine(_SheathWeapon(leftWeapon, weaponNumber));
					if(!instantWeaponSwitch){
						yield return new WaitForSeconds(1.05f);
					}
					if(rightWeapon != 0){
						StartCoroutine(_SheathWeapon(rightWeapon, weaponNumber));
						if(!instantWeaponSwitch){
							yield return new WaitForSeconds(1.05f);
						}
						//and right hand weapon
						if(weaponNumber != 0){
							StartCoroutine(_UnSheathWeapon(weaponNumber));
						}
					}
					if(weaponNumber != 0){
						StartCoroutine(_UnSheathWeapon(weaponNumber));
					}
				}
				//check right hand for weapon if no left hand weapon
				if(rightWeapon != 0){
					StartCoroutine(_SheathWeapon(rightWeapon, weaponNumber));
					if(!instantWeaponSwitch){
						yield return new WaitForSeconds(1.05f);
					}
					if(weaponNumber != 0){
						StartCoroutine(_UnSheathWeapon(weaponNumber));
					}
				}
			}
			//using 1 handed weapon(s)
			else if(weaponNumber == 7){
				if(leftWeapon > 0){
					StartCoroutine(_SheathWeapon(leftWeapon, weaponNumber));
					if(!instantWeaponSwitch){
						yield return new WaitForSeconds(1.05f);
					}
				}
				StartCoroutine(_UnSheathWeapon(weaponNumber));
			}
			//switching left weapon, put away left weapon if equipped
			else if((weaponNumber == 8 || weaponNumber == 10 || weaponNumber == 12 || weaponNumber == 14 || weaponNumber == 16)){
				if(leftWeapon > 0){
					StartCoroutine(_SheathWeapon(leftWeapon, weaponNumber));
					if(!instantWeaponSwitch){
						yield return new WaitForSeconds(1.05f);
					}
				}
				StartCoroutine(_UnSheathWeapon(weaponNumber));
			}
			//switching right weapon, put away right weapon if equipped
			else if((weaponNumber == 9 || weaponNumber == 11 || weaponNumber == 13 || weaponNumber == 15 || weaponNumber == 17 || weaponNumber == 19)){
				if(rightWeapon > 0){
					StartCoroutine(_SheathWeapon(rightWeapon, weaponNumber));
					if(!instantWeaponSwitch){
						yield return new WaitForSeconds(1.05f);
					}
				}
				StartCoroutine(_UnSheathWeapon(weaponNumber));
			}
		}
		yield return null;
	}

	public IEnumerator _SheathWeapon(int weaponNumber, int weaponDraw){
		if((weaponNumber == 8 || weaponNumber == 10 || weaponNumber == 12 || weaponNumber == 14 || weaponNumber == 16)){
			animator.SetInteger("LeftRight", 1);
		}
		else if((weaponNumber == 9 || weaponNumber == 11 || weaponNumber == 13 || weaponNumber == 15 || weaponNumber == 17 || weaponNumber == 19)){
			animator.SetInteger("LeftRight", 2);
		}
		else if(weaponNumber == 7){
			animator.SetInteger("LeftRight", 1);
		} 
		//if switching to unarmed, don't set "Armed" until after 2nd weapon sheath
		if(weaponDraw == 0){
			if(leftWeapon == 0 && rightWeapon != 0){
				animator.SetBool("Armed", false);
			}
			if(rightWeapon == 0 && leftWeapon != 0){
				animator.SetBool("Armed", false);
			}
		}
		if(!instantWeaponSwitch){
			animator.SetTrigger("WeaponSheathTrigger");
			yield return new WaitForSeconds(0.1f);
		}
		else{
			animator.SetTrigger("InstantSwitchTrigger");
		}
		//Sheath 2 handed weapons
		if(weaponNumber < 7 || weaponNumber == 18 || weaponNumber == 19 || weaponNumber == 20){
			leftWeapon = 0;
			animator.SetInteger("LeftWeapon", 0);
			rightWeapon = 0;
			animator.SetInteger("RightWeapon", 0);
			animator.SetBool("Shield", false);
			animator.SetBool("Armed", false);
		}
		//Sheath Shield
		else if(weaponNumber == 7){
			leftWeapon = 0;
			animator.SetInteger("LeftWeapon", 0);
			animator.SetBool("Shield", false);
		}
		//Sheath left weapon
		else if((weaponNumber == 8 || weaponNumber == 10 || weaponNumber == 12 || weaponNumber == 14 || weaponNumber == 16)){
			leftWeapon = 0;
			animator.SetInteger("LeftWeapon", 0);
		}
		//Sheath right weapon
		else if((weaponNumber == 9 || weaponNumber == 11 || weaponNumber == 13 || weaponNumber == 15 || weaponNumber == 17 || weaponNumber == 19)){
			rightWeapon = 0;
			animator.SetInteger("RightWeapon", 0);
		}
		//if switched to unarmed
		if(leftWeapon == 0 && rightWeapon == 0){
			animator.SetBool("Armed", false);
		}
		if(leftWeapon == 0 && rightWeapon == 0){
			animator.SetInteger("LeftRight", 0);
			animator.SetInteger("Weapon", 0);
			animator.SetBool("Armed", false);
			weapon = Weapon.UNARMED;
		}
		if(instantWeaponSwitch){
			StartCoroutine(_WeaponVisibility(weaponNumber, 0f, false));
		}
		else{
			StartCoroutine(_WeaponVisibility(weaponNumber, 0.4f, false));
			StartCoroutine(_Lock(true, true, true, 0, 1));
		}
		yield return null;
	}

	public IEnumerator _UnSheathWeapon(int weaponNumber){
		if(!instantWeaponSwitch){
			animator.SetInteger("Weapon", -1);
		}
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		//two handed weapons
		if(weaponNumber < 7 || weaponNumber == 18 || weaponNumber == 20){
			leftWeapon = weaponNumber;
			animator.SetInteger("LeftRight", 3);
			if(weaponNumber == 0){
				weapon = Weapon.UNARMED;
			}
			if(weaponNumber == 1){
				weapon = Weapon.TWOHANDSWORD;
				if(!instantWeaponSwitch){
					StartCoroutine(_WeaponVisibility(weaponNumber, 0.4f, true));
				}
			}
			else if(weaponNumber == 2){
				weapon = Weapon.TWOHANDSPEAR;
				if(!instantWeaponSwitch){
					StartCoroutine(_WeaponVisibility(weaponNumber, 0.5f, true));
				}
			}
			else if(weaponNumber == 3){
				weapon = Weapon.TWOHANDAXE;
				if(!instantWeaponSwitch){
					StartCoroutine(_WeaponVisibility(weaponNumber, 0.5f, true));
				}
			}
			else if(weaponNumber == 4){
				weapon = Weapon.TWOHANDBOW;
				if(!instantWeaponSwitch){
					StartCoroutine(_WeaponVisibility(weaponNumber, 0.55f, true));
				}
			}
			else if(weaponNumber == 5){
				weapon = Weapon.TWOHANDCROSSBOW;
				if(!instantWeaponSwitch){
					StartCoroutine(_WeaponVisibility(weaponNumber, 0.5f, true));
				}
			}
			else if(weaponNumber == 6){
				weapon = Weapon.STAFF;
				if(!instantWeaponSwitch){
					StartCoroutine(_WeaponVisibility(weaponNumber, 0.6f, true));
				}
			}
			else if(weaponNumber == 18){
				weapon = Weapon.RIFLE;
				if(!instantWeaponSwitch){
					StartCoroutine(_WeaponVisibility(weaponNumber, 0.6f, true));
				}
			}
			else if(weaponNumber == 20){
				weapon = Weapon.TWOHANDCLUB;
				if(!instantWeaponSwitch){
					StartCoroutine(_WeaponVisibility(weaponNumber, 0.6f, true));
				}
			}
			if(!instantWeaponSwitch){
				animator.SetTrigger("WeaponUnsheathTrigger");
				StartCoroutine(_Lock(true, true, true, 0, 1.1f));
			}
			if(instantWeaponSwitch){
				StartCoroutine(_WeaponVisibility(weaponNumber, 0.2f, true));
				animator.SetTrigger("InstantSwitchTrigger");
			}
		}
		//one handed weapons
		else{
			if(weaponNumber == 7){
				leftWeapon = 7;
				animator.SetInteger("LeftWeapon", 7);
				animator.SetInteger("LeftRight", 1);
				if(!instantWeaponSwitch){
					StartCoroutine(_WeaponVisibility(weaponNumber, 0.6f, true));
					animator.SetTrigger("WeaponUnsheathTrigger");
					StartCoroutine(_Lock(true, true, true, 0, 1.1f));
				}
				else{
					StartCoroutine(_WeaponVisibility(weaponNumber, 0.2f, true));
					animator.SetTrigger("InstantSwitchTrigger");
				}
				animator.SetBool("Shield", true);
			}
			//Left hand weapons
			else if(weaponNumber == 8 || weaponNumber == 10 || weaponNumber == 12 || weaponNumber == 14 || weaponNumber == 16){
				animator.SetInteger("LeftRight", 1);
				animator.SetInteger("LeftWeapon", weaponNumber);
				if(!instantWeaponSwitch){
					StartCoroutine(_WeaponVisibility(weaponNumber, 0.6f, true));
					animator.SetTrigger("WeaponUnsheathTrigger");
					StartCoroutine(_Lock(true, true, true, 0, 1.1f));
				}
				else{
					StartCoroutine(_WeaponVisibility(weaponNumber, 0.2f, true));
					animator.SetTrigger("InstantSwitchTrigger");
				}
				leftWeapon = weaponNumber;
				weaponNumber = 7;
			}
			//Right hand weapons
			else if(weaponNumber == 9 || weaponNumber == 11 || weaponNumber == 13 || weaponNumber == 15 || weaponNumber == 17 || weaponNumber == 19){
				animator.SetInteger("LeftRight", 2);
				animator.SetInteger("RightWeapon", weaponNumber);
				rightWeapon = weaponNumber;
				if(!instantWeaponSwitch){
					StartCoroutine(_WeaponVisibility(weaponNumber, 0.6f, true));
					animator.SetTrigger("WeaponUnsheathTrigger");
					StartCoroutine(_Lock(true, true, true, 0, 1.1f));
				}
				else{
					StartCoroutine(_WeaponVisibility(weaponNumber, 0.2f, true));
					animator.SetTrigger("InstantSwitchTrigger");
				}
				weaponNumber = 7;
				//set shield to false for animator, will reset later
				if(leftWeapon == 7){
					animator.SetBool("Shield", false);
				}
			}
		}
		if(weapon == Weapon.RIFLE){
			animator.SetInteger("Weapon", 8);
		}
		else if(weapon == Weapon.TWOHANDCLUB){
			animator.SetInteger("Weapon", 9);
		}
		else{
			animator.SetInteger("Weapon", weaponNumber);
		}
		//Shield
		if(leftWeapon == 7){
			if(rightWeapon == 0){
				animator.SetBool("Shield", true);
				weapon = Weapon.SHIELD;
			}
			else{
				animator.SetBool("Shield", true);
				weapon = Weapon.ARMEDSHIELD;
			}
			animator.SetBool("Shield", true);
		}
		//Set weapon and animator to Armed
		if((leftWeapon > 6 || rightWeapon > 6) && weapon != Weapon.RIFLE && weapon != Weapon.TWOHANDCLUB){
			yield return new WaitForSeconds(0.25f);
			animator.SetBool("Armed", true);
			if(leftWeapon != 7){
				weapon = Weapon.ARMED;
			}
		}
		//For dual blocking
		if(rightWeapon == 9 || rightWeapon == 11 || rightWeapon == 13 || rightWeapon == 15 || rightWeapon == 17){
			if(leftWeapon == 8 || leftWeapon == 10 || leftWeapon == 12 || leftWeapon == 14 || leftWeapon == 16){
				yield return new WaitForSeconds(.1f);
				animator.SetInteger("LeftRight", 3);
			}
		}
		else if(leftWeapon == 8 || leftWeapon == 10 || leftWeapon == 12 || leftWeapon == 14 || leftWeapon == 16){
			if(rightWeapon == 9 || rightWeapon == 11 || rightWeapon == 13 || rightWeapon == 15 || rightWeapon == 17){
				yield return new WaitForSeconds(.1f);
				animator.SetInteger("LeftRight", 3);
			}
		}
		if(instantWeaponSwitch){
			animator.SetTrigger("InstantSwitchTrigger");
		}
		yield return null;
	}

	public IEnumerator _WeaponVisibility(int weaponNumber, float delayTime, bool visible){
		yield return new WaitForSeconds(delayTime);
		if(weaponNumber == 1){
			twoHandSword.SetActive(visible);
		}
		if(weaponNumber == 2){
			twoHandSpear.SetActive(visible);
		}
		if(weaponNumber == 3){
			twoHandAxe.SetActive(visible);
		}
		if(weaponNumber == 4){
			twoHandBow.SetActive(visible);
		}
		if(weaponNumber == 5){
			twoHandCrossbow.SetActive(visible);
		}
		if(weaponNumber == 6){
			staff.SetActive(visible);
		}
		if(weaponNumber == 7){
			shield.SetActive(visible);
		}
		if(weaponNumber == 8){
			swordL.SetActive(visible);
		}
		if(weaponNumber == 9){
			swordR.SetActive(visible);
		}
		if(weaponNumber == 10){
			maceL.SetActive(visible);
		}
		if(weaponNumber == 11){
			maceR.SetActive(visible);
		}
		if(weaponNumber == 12){
			daggerL.SetActive(visible);
		}
		if(weaponNumber == 13){
			daggerR.SetActive(visible);
		}
		if(weaponNumber == 14){
			itemL.SetActive(visible);
		}
		if(weaponNumber == 15){
			itemR.SetActive(visible);
		}
		if(weaponNumber == 16){
			pistolL.SetActive(visible);
		}
		if(weaponNumber == 17){
			pistolR.SetActive(visible);
		}
		if(weaponNumber == 18){
			rifle.SetActive(visible);
		}
		if(weaponNumber == 19){
			spear.SetActive(visible);
		}
		if(weaponNumber == 20){
			twoHandClub.SetActive(visible);
		}
		yield return null;
	}

	void HideAllWeapons(){
		if(twoHandAxe != null){
			twoHandAxe.SetActive(false);
		}
		if(twoHandBow != null){
			twoHandBow.SetActive(false);
		}
		if(twoHandCrossbow != null){
			twoHandCrossbow.SetActive(false);
		}
		if(twoHandSpear != null){
			twoHandSpear.SetActive(false);
		}
		if(twoHandSword != null){
			twoHandSword.SetActive(false);
		}
		if(twoHandClub != null){
			twoHandClub.SetActive(false);
		}
		if(staff != null){
			staff.SetActive(false);
		}
		if(swordL != null){
			swordL.SetActive(false);
		}
		if(swordR != null){
			swordR.SetActive(false);
		}
		if(maceL != null){
			maceL.SetActive(false);
		}
		if(maceR != null){
			maceR.SetActive(false);
		}
		if(daggerL != null){
			daggerL.SetActive(false);
		}
		if(daggerR != null){
			daggerR.SetActive(false);
		}
		if(itemL != null){
			itemL.SetActive(false);
		}
		if(itemR != null){
			itemR.SetActive(false);
		}
		if(shield != null){
			shield.SetActive(false);
		}
		if(pistolL != null){
			pistolL.SetActive(false);
		}
		if(pistolR != null){
			pistolR.SetActive(false);
		}
		if(rifle != null){
			rifle.SetActive(false);
		}
		if(spear != null){
			spear.SetActive(false);
		}
	}

	public IEnumerator _BlockHitReact(){
		int hits = 2;
		int hitNumber = Random.Range(0, hits);
		animator.SetTrigger("BlockGetHit" + (hitNumber + 1).ToString() + "Trigger");
		StartCoroutine(_Lock(true, true, true, 0.1f, 0.4f));
		StartCoroutine(_Knockback(-transform.forward, 3, 3));
		yield return null;
	}

	public IEnumerator _BlockBreak(){
		animator.applyRootMotion = true;
		animator.SetTrigger("BlockBreakTrigger");
		yield return new WaitForSeconds(1f);
		animator.applyRootMotion = false;
	}

	public void Pickup(){
		animator.SetTrigger("PickupTrigger");
		StartCoroutine(_Lock(true, true, true, 0, 1.4f));
	}

	public void Activate(){
		animator.SetTrigger("ActivateTrigger");
		StartCoroutine(_Lock(true, true, true, 0, 1.2f));
	}

	public void SetSheathLocation(int location){
		animator.SetInteger("SheathLocation", location);
	}

	#endregion

}
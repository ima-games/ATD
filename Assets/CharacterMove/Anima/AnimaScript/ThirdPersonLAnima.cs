using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonLAnima : MonoBehaviour
{
	//记录---------------------------------------------------------------
	Animator animator;
	int speedXHash = Animator.StringToHash("SpeedX");
	int speedYHash = Animator.StringToHash("SpeedY");
	//内部调用-----------------------------------------------------------
	void walkAndRunUpdateSet()//Update中调用 设置 animator中 IsWalk和IsRun的 bool
	{
		//if (Input.GetAxisRaw("Vertical") < 0)
		//{
		//	animator.SetFloat(speedXHash, -Input.GetAxisRaw("Horizontal"));
		//}
		//else
		//{
		//	animator.SetFloat(speedXHash, Input.GetAxisRaw("Horizontal"));
		//}
		
		//animator.SetFloat(speedYHash, Input.GetAxisRaw("Vertical"));

	}
	//Behaviour----------------------------------------------------------
	// Start is called before the first frame update
	void Start()
	{
		//animator = gameObject.GetComponent<Animator>();
	}

	// Update is called once per frame
	void Update()
	{
		walkAndRunUpdateSet();

	}
	private void OnAnimatorMove()
	{

	}
}

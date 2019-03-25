using UnityEngine;
using System.Collections;

public class IKHands : MonoBehaviour {

	public Transform leftHandObj;
	public Transform rightHandObj;
	public Transform attachLeft;
	public Transform attachRight;

	public float leftHandPositionWeight;
	public float leftHandRotationWeight;
	public float rightHandPositionWeight;
	public float rightHandRotationWeight;
	
	private Animator animator;
	
	void Start() {
		animator = this.gameObject.GetComponent<Animator>();
	}
	
	void OnAnimatorIK(int layerIndex) {
		if(leftHandObj != null){
			animator.SetIKPositionWeight(AvatarIKGoal.LeftHand,leftHandPositionWeight);
			animator.SetIKRotationWeight(AvatarIKGoal.LeftHand,leftHandRotationWeight);
			animator.SetIKPosition(AvatarIKGoal.LeftHand,attachLeft.position);                    
			animator.SetIKRotation(AvatarIKGoal.LeftHand,attachLeft.rotation);
		}
		if(rightHandObj != null){
			animator.SetIKPositionWeight(AvatarIKGoal.RightHand,rightHandPositionWeight);
			animator.SetIKRotationWeight(AvatarIKGoal.RightHand,rightHandRotationWeight);     
			animator.SetIKPosition(AvatarIKGoal.RightHand,attachRight.position);                    
			animator.SetIKRotation(AvatarIKGoal.RightHand,attachRight.rotation);
		}
	}
}
﻿using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(Animator))] 

public class IKControl : MonoBehaviour {

	protected Animator animator;

	public bool ikActive = false;

	public Transform rightHandObj = null;
	public Transform leftHandObj = null;
	public Transform rightFootObj = null;
	public Transform leftFootObj = null;
	public Transform bodyObj = null;

	public Transform lookObj = null;

	//public SimulateWalk simulateWalk;
	//public Transform simulateRightObj = null;
	//public Transform simulateLeftObj = null;

	void Start () 
	{
		animator = GetComponent<Animator>();
	}

	//a callback for calculating IK
	void OnAnimatorIK()
	{
		if(animator) {

			//if the IK is active, set the position and rotation directly to the goal. 
			if(ikActive) {

				// Set the look target position, if one has been assigned
				if(lookObj != null) {
					animator.SetLookAtWeight(1);
					animator.SetLookAtPosition(lookObj.position);
				}    

				// Set the right hand target position and rotation, if one has been assigned
				SetIKPositionWeightAndRotationWeight( AvatarIKGoal.RightHand, rightHandObj );
				SetIKPositionWeightAndRotationWeight( AvatarIKGoal.LeftHand, leftHandObj );
                if( rightFootObj )
			    	SetIKPositionWeightAndRotationWeight( AvatarIKGoal.RightFoot, rightFootObj );
                if (leftFootObj)
                    SetIKPositionWeightAndRotationWeight( AvatarIKGoal.LeftFoot, leftFootObj );

				if( bodyObj )
				{
					animator.bodyPosition = bodyObj.transform.position;
					animator.bodyRotation = bodyObj.transform.rotation;
				}   
			}

			//if the IK is not active, set the position and rotation of the hand and head back to the original position
			else {          
				SetIKPositionWeightAndRotationWeight( AvatarIKGoal.RightHand, null );
				SetIKPositionWeightAndRotationWeight( AvatarIKGoal.LeftHand, null );
                if (rightFootObj)
                    SetIKPositionWeightAndRotationWeight( AvatarIKGoal.RightFoot, null );
                if (leftFootObj)
                    SetIKPositionWeightAndRotationWeight( AvatarIKGoal.LeftFoot, null );

				animator.SetLookAtWeight(0);
			}
		}
	}   

	private void SetIKPositionWeightAndRotationWeight( AvatarIKGoal avatarIKGoal, Transform target)
	{
		if( target != null )
		{
			animator.SetIKPositionWeight(avatarIKGoal, 1);
			animator.SetIKRotationWeight(avatarIKGoal, 1);  
			animator.SetIKPosition(avatarIKGoal, target.position);
			animator.SetIKRotation(avatarIKGoal, target.rotation);
		}
		else
		{
			animator.SetIKPositionWeight(avatarIKGoal, 0);
			animator.SetIKRotationWeight(avatarIKGoal, 0);  
		}
	}

	public void SetSimulateFoot()
	{
	//	simulateWalk.enabled = true;
	//	rightFootObj = simulateRightObj;
	//	leftFootObj = simulateLeftObj;
	}
}

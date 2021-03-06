﻿using UnityEngine;
using System.Collections;

public class Input_Interact : InputMachine {

	public override void CheckUpdate(StateMachine checkMachine){
		//checkMachine.UpdateState (StateMaster.instance.animalRunningAway, checkMachine);
	}
	public override void InstanceUpdate(StateMachine checkMachine){
	}

	public override void ExitState(StateMachine checkMachine){
	}
	public override void EnterState(StateMachine checkMachine){
		checkMachine.timer.StartTimer (0.3f);
	}

	public override void SwipeUp(GameObject obj, Vector3 point, StateMachine checkMachine){
		checkMachine.UpdateState (InputMachine.instance.swipeUp, checkMachine);
	}
	public override void SwipeDown(GameObject obj, Vector3 point, StateMachine checkMachine){
		checkMachine.UpdateState (InputMachine.instance.swipeDown, checkMachine);
	}
	public override void SwipeForward(GameObject obj, Vector3 point, StateMachine checkMachine){
		checkMachine.UpdateState (InputMachine.instance.swipeForward, checkMachine);
	}
	public override void SwipeBack(GameObject obj, Vector3 point, StateMachine checkMachine){
		checkMachine.UpdateState (InputMachine.instance.swipeBack, checkMachine);
	}
	public override void Tap(GameObject obj, Vector3 point, StateMachine checkMachine){}
	public override void CheckInteract(GameObject obj, Vector3 point, StateMachine checkMachine){
		if (obj == null) {
			canInteract = false;
			return;
		}
		if (obj.GetComponentInParent<Ground> () != null) {
			canInteract = false;
			return;
		}
		if (obj.GetComponentInParent<StateMachine> () == null) {
			canInteract = false;
			return;
		}
		canInteract = true;
	}
	public override void Release(GameObject obj, Vector3 point, StateMachine checkMachine){
		if (checkMachine.timer.CheckTimer()) {
			return;
		}
		if (obj == null) {
			return;
		}
		if (obj.GetComponent<Ground>() != null) {
			return;
		} else if (obj.transform.parent.GetComponent<Ground> () != null) {
			return;
		}
		if (obj.GetComponent<StateMachine> () == null) {
			if (obj.GetComponentInParent<StateMachine> () != null) {
				obj.GetComponentInParent<StateMachine> ().Interact (obj, point);
			}
			return;
		}
		obj.GetComponent<StateMachine> ().Interact (obj, point);
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class StateMachine: MonoBehaviour {
	
	public Timer timer;
	public int phase = 0;
	public float stepDelay = 0;
	public Action interact;
	public static StateMaster master;

	public StateMachine currentState;

	public void UpdateState(StateMachine newState, StateMachine thisMachine){
		if (currentState != null) {
			currentState.ExitState (thisMachine);
		}
		currentState = newState;
		currentState.EnterState (thisMachine);
	}

	public virtual void CheckUpdate(StateMachine checkMachine){}

	public virtual void ExitState(StateMachine checkMachine){}
	public virtual void EnterState(StateMachine checkMachine){}
	public virtual void InstanceInitiate(StateMachine checkMachine){}
	public virtual void InstanceUpdate(StateMachine checkMachine){}
	public virtual void InstanceInteract(GameObject obj, Vector3 point, StateMachine checkMachine){}
	public virtual List<InputMachine> InstanceHover(){
		return null;
	}

	public void Initiate(){
		timer = new Timer (this);
		InstanceInitiate (this);
	}

	public void Update(){
		if (GetComponent<StateMaster> () != null) {
			return;
		}
		if (currentState != null) {
			currentState.CheckUpdate (this);
		}
		InstanceUpdate (this);
	}

	public void Interact(GameObject obj, Vector3 point){
		if (GetComponent<StateMaster> () != null) {
			return;
		}
		InstanceInteract(obj, point, this);
	}
}

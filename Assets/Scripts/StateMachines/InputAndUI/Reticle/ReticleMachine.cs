using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ReticleMachine : StateMachine {

	public HandMachine left, right;
	public bool is_nearObjects = false, is_onGround;
	private InputMachine currentInteractionState;
	private ReticleInstance reticleInstance;

	void Start(){
		Initiate ();
	}

	public override void InstanceInitiate(StateMachine checkMachine){
	}

	public void SetReticle(InputMachine setValue){
		if (currentInteractionState == setValue) {
			return;
		}
		left.SetHand (setValue);
		right.SetHand (setValue);
		if (reticleInstance != null) {
			Destroy (reticleInstance.gameObject);
		}
		currentInteractionState = setValue;
		GameObject newHand = (GameObject)GameObject.Instantiate (setValue.setReticle, transform);
		newHand.transform.localPosition = Vector3.zero;
		reticleInstance = newHand.GetComponent<ReticleInstance> ();
	}

	public override void InstanceUpdate(StateMachine checkMachine) {
		Vector3 targetLocation = transform.position + 2 * (transform.position - PlayerMachine.playerObject.transform.position);
		transform.LookAt (new Vector3(targetLocation.x, transform.position.y, targetLocation.z));
		if (timerStart + timerDuration < Time.time) {
			is_nearObjects = false;
		}
		if (reticleInstance != null) {
			if (!InputMachine.instance.is_holding) {
				reticleInstance.SetNoHold ();
			} else {
				if (currentInteractionState.canInteract) {
					reticleInstance.SetHoldTrue (right.transform.position, targetLocation);
				} else {
					reticleInstance.SetHoldFalse ();
				}
			}
		}
	}

	public void OnTriggerStay(Collider col){
		if (col.GetComponent<Ground> () == null && col.GetComponent<PlayerMachine> () == null) {
			timerStart = Time.time;
			is_nearObjects = true;
		}
		if (col.GetComponent<Ground> () != null) {
			is_onGround = true;
		}
	}

	public void OnTriggerExit(Collider col){
		if (col.GetComponent<Ground> () != null) {
			is_onGround = false;
		}
	}
}
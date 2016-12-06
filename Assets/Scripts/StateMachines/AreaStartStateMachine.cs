using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AreaStartStateMachine : StateMachine {

	public GameObject eventSystem;
	public GameObject usedRig;
	public RoomMachine startRoom;
	public void Awake(){
		Initiate ();
	}

	public override void CheckUpdate(StateMachine checkMachine){
		
	}

	public override void ExitState(StateMachine checkMachine){}
	public override void EnterState(StateMachine checkMachine){}
	public override void InstanceInitiate(StateMachine checkMachine){
		if (eventSystem == null) {
			eventSystem = (GameObject) Resources.Load("EventSystem", typeof(GameObject));
		}
		GameObject.Instantiate (eventSystem);
		if (PlayerMachine.playerObject == null) {	
			if (usedRig == null) {
				usedRig = (GameObject) Resources.Load("UsedRig", typeof(GameObject));
			}
			GameObject.Instantiate (usedRig, transform.position + Vector3.up * InputMachine.playerHeight, transform.rotation);
		} else {
			PlayerMachine.playerObject.transform.position = transform.position + Vector3.up * InputMachine.playerHeight;
			PlayerMachine.playerObject.transform.rotation = transform.rotation;
			InputMachine.instance.UpdateState (InputMachine.instance.swipeForward, checkMachine);
		}
		StartCoroutine ("SetGos");	
	}
	IEnumerator SetGos(){
		yield return null;
		InputMachine.instance.gos = new List<GameObject> ();
		foreach (GameObject go in FindObjectsOfType<GameObject> ()) {
			if (go.GetComponentInParent<HOTweenManager> () == null
			    && go.GetComponentInParent<OVRTouchpadHelper> () == null) {
			} else if(go.GetComponentInParent<NoCull> () != null
				&& go.GetComponentInParent<RoomMachine> () == null) {
				if (!go.GetComponentInParent<NoCull> ().includeChildren
				    && go.GetComponent<NoCull> () == null) {
					InputMachine.instance.gos.Add (go);
				}
			} else if (go.GetComponentInParent<RoomMachine> () == null) {
				InputMachine.instance.gos.Add (go);
			}
		}
		foreach (RoomMachine room in FindObjectsOfType<RoomMachine> ()) {
			InputMachine.instance.rooms.Add (room);
			room.gameObject.SetActive (false);
		}
		InputMachine.instance.SetRoom (startRoom);
	}
}
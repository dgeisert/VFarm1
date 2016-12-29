using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ADMIN : StateMachine {
	
	public override List<InputMachine> InstanceHover(){
		return new List<InputMachine>(){StateMaster.instance.inputInteract };
	}

	public string action = "";

	public override void InstanceInteract(GameObject obj, Vector3 point, StateMachine checkMachine){
		switch (action) {
		case "DestroyGos":
			DestroyGos ();
			break;
		case "CreateGos":
			CreateGos ();
			break;
		case "Save":
			Save ();
			break;
		case "Load":
			Load ();
			break;
		default:
			break;
		}
	}

	public override void InstanceUpdate(StateMachine checkMachine){
		switch (action) {
		case "SetText":
			SetText ();
			break;
		default:
			break;
		}
	}

	public void DestroyGos(){
		foreach(GameObject go in InputMachine.instance.gos){
			Destroy(go);
		}
	}
	public void CreateGos(){
		foreach (GameObject go in InputMachine.instance.spawners) {
			GameObject.Instantiate (go);
		}
		InputMachine.instance.CheckObjects ();
	}
	public void Save(){
		PlayerMachine.instance.SaveGos ();
	}
	public void Load(){
		PlayerMachine.instance.LoadGos ();
	}
	public void SetText(){
		GetComponent<TextMesh> ().text = "Wood" + PlayerMachine.instance.GetResource ("Wood");
	}
}
